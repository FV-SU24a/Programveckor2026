using UnityEngine;

public class WaveManager : MonoBehaviour
{ 
    public int currentWave = 0;
    public WeaponPoolManager weaponPool;

    //reference to the enemy spawning script that keeps track and spawns enemies once called
    //also jst saying u need two empty game objects to run this, check my scene to see what i did if u need to 
    public enemyspawn enemySpawner;

    void Start()
    {
        enemySpawner.SpawnWave(currentWave); //actually starts the first wave
    }

    public void CompleteWave()
    {
        currentWave++;
        weaponPool.UpdateWeaponPool(currentWave);
    }
}
