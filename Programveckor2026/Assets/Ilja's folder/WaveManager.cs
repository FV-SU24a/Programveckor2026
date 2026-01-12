using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
public class WaveManager : MonoBehaviour
{
    private HashSet<GameObject> spawnedBosses = new HashSet<GameObject>();


    public List<WaveEnemy> waveEnemies;

    public TMP_Text timerText; 
    public TMP_Text skipText; 

    public int currentWave = 0;
    public WeaponPoolManager weaponPool;

    public enemyspawn enemySpawner;

    public float downtimeDuration = 180f;
    public float waveDuration = 300f;

    public float downtimeTimer;
    public float waveTimer;

    private enum WaveState { DownTime, ActiveWave} private WaveState currentState;

    private Keyboard keyboard; //for detecting key presses


    void Start()
    {
        keyboard = Keyboard.current;
        StartDownTime();
    }

    private void Update()
    {
        switch (currentState)
        {
            case WaveState.DownTime:HandleDownTime();
                break;
            case WaveState.ActiveWave: HandleActiveWave();
                break;
        }

        UpdateUi();
    }

    void StartDownTime()
    {
        currentState = WaveState.DownTime;
        downtimeTimer = downtimeDuration;
        Debug.Log($"Wave {currentWave} starting downtime({downtimeTimer} sec)");
    }

    void HandleDownTime()
    {
        downtimeTimer -= Time.deltaTime;

        if(keyboard != null && keyboard.cKey.wasPressedThisFrame)
        {
            downtimeTimer = 0f;
        }

        if(downtimeTimer <= 0)
        {
            StartWave();
        }
    }

    void StartWave()
    {
        currentState = WaveState.ActiveWave;
        waveTimer = waveDuration;
        currentWave++;

        bool bossSpawningThisWave = false;
        foreach (WaveEnemy we in waveEnemies)
        {
            if (we.enemyPrefab.CompareTag("Boss") && currentWave + 1 >= we.startWave && !spawnedBosses.Contains(we.enemyPrefab))
            {
                bossSpawningThisWave = true;
                break;
            }
        }

        //pawn enemies based on waveEnemies list
        foreach (WaveEnemy we in waveEnemies)
        {
            if (currentWave + 1 >= we.startWave) //+1 because currentWave starts at 0
            {

                if (we.enemyPrefab.CompareTag("Boss"))
                {
                    if (!spawnedBosses.Contains(we.enemyPrefab))
                    {
                        ClearNormalEnemies();
                        enemySpawner.SpawnWave(currentWave, we);
                        spawnedBosses.Add(we.enemyPrefab);
                    }
                }
                else
                {
                    // Only spawn normal enemies if no boss is spawning this wave
                    if (!bossSpawningThisWave)
                        enemySpawner.SpawnWave(currentWave, we);
                }
            }
        }

        Debug.Log($"Wave {currentWave} started");

        if (weaponPool != null)
            weaponPool.UpdateWeaponPool(currentWave);
    }

    void HandleActiveWave()
    {
        waveTimer -= Time.deltaTime;
        if(waveTimer <= 0f)
        {
            Debug.Log($"Wave {currentWave} ended (timer expired). Starting downtime.");
            StartDownTime();
        }
    }

    void UpdateUi()
    {
        switch (currentState)
        {
            case WaveState.DownTime:
                //putting time as minutes:seconds
                int minutes = Mathf.FloorToInt(downtimeTimer / 60f);
                int seconds = Mathf.FloorToInt(downtimeTimer % 60f);
                timerText.text = $"downtime: {minutes:00}:{seconds:00}";
                skipText.gameObject.SetActive(true);
                break;

            case WaveState.ActiveWave:
                minutes = Mathf.FloorToInt(waveTimer / 60f);
                seconds = Mathf.FloorToInt(waveTimer % 60f);
                timerText.text = $"wave {currentWave}: {minutes:00}:{seconds:00}";
                skipText.gameObject.SetActive(false);
                break;
        }
    }

    private void ClearNormalEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
