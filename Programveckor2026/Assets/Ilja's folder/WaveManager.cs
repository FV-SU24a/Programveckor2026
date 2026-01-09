using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class WaveManager : MonoBehaviour
{

    public TMP_Text timerText; 
    public TMP_Text skipText; 

    public int currentWave = 0;
    public WeaponPoolManager weaponPool;

    //reference to the enemy spawning script that keeps track and spawns enemies once called
    //also jst saying u need two empty game objects to run this, check my scene to see what i did if u need to 
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
        Debug.Log($"wave {currentWave} started");

        if (weaponPool != null)
            weaponPool.UpdateWeaponPool(currentWave);

        enemySpawner.SpawnWave(currentWave);
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
}
