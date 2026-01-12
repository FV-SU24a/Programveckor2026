using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance; // singleton for easy access

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

    private enum WaveState { DownTime, ActiveWave }
    private WaveState currentState;

    private Keyboard keyboard; //for detecting key presses

    [HideInInspector] public bool isBossWaveThisRound = false; // flag for enemyspawn

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        keyboard = Keyboard.current;
        StartDownTime();
    }

    private void Update()
    {
        switch (currentState)
        {
            case WaveState.DownTime: HandleDownTime(); break;
            case WaveState.ActiveWave: HandleActiveWave(); break;
        }

        UpdateUi();
    }

    void StartDownTime()
    {
        currentState = WaveState.DownTime;
        downtimeTimer = downtimeDuration;
        Debug.Log($"Wave {currentWave} starting downtime ({downtimeTimer} sec)");
    }

    void HandleDownTime()
    {
        downtimeTimer -= Time.deltaTime;

        if (keyboard != null && keyboard.cKey.wasPressedThisFrame)
        {
            downtimeTimer = 0f;
        }

        if (downtimeTimer <= 0)
        {
            StartWave();
        }
    }

    void StartWave()
    {
        currentState = WaveState.ActiveWave;
        waveTimer = waveDuration;
        currentWave++;

        WaveEnemy bossToSpawn = null;

        // check if a boss is spawning this wave
        foreach (WaveEnemy we in waveEnemies)
        {
            if (we.enemyPrefab.CompareTag("Boss") && currentWave >= we.startWave && !spawnedBosses.Contains(we.enemyPrefab))
            {
                bossToSpawn = we;
                break;
            }
        }

        if (bossToSpawn != null)
        {
            isBossWaveThisRound = true;
            ClearNormalEnemies();
            enemySpawner.SpawnWave(currentWave, bossToSpawn);
            spawnedBosses.Add(bossToSpawn.enemyPrefab);
            Debug.Log($"Wave {currentWave} has a boss. Normal enemies skipped.");
            return;
        }
        else
        {
            isBossWaveThisRound = false;
        }

        // spawn normal enemies only if no boss
        foreach (WaveEnemy we in waveEnemies)
        {
            if (!we.enemyPrefab.CompareTag("Boss") && currentWave >= we.startWave)
            {
                enemySpawner.SpawnWave(currentWave, we);
            }
        }

        Debug.Log($"Wave {currentWave} started");

        weaponPool?.UpdateWeaponPool(currentWave);
    }

    void HandleActiveWave()
    {
        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0f)
        {
            Debug.Log($"Wave {currentWave} ended (timer expired). Starting downtime.");
            StartDownTime();
        }
    }

    void UpdateUi()
    {
        int minutes, seconds;
        switch (currentState)
        {
            case WaveState.DownTime:
                minutes = Mathf.FloorToInt(downtimeTimer / 60f);
                seconds = Mathf.FloorToInt(downtimeTimer % 60f);
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
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (!enemy.CompareTag("Boss")) Destroy(enemy);
        }
    }
}
