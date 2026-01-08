using UnityEngine;

public class WaveManager : MonoBehaviour
{ 
    public int currentWave = 0;
    public WeaponPoolManager weaponPool;

    //reference to the enemy spawning script that keeps track and spawns enemies once called
    public enemyspawn enemySpawner;
    public void CompleteWave()
    {
        currentWave++;
        weaponPool.UpdateWeaponPool(currentWave);
    }
}
