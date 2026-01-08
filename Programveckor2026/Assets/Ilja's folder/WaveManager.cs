using UnityEngine;

public class WaveManager : MonoBehaviour
{ //working on a script thats called enemyspawn that will be called in this script..so this script controls when enemies should spawn jst saying
    public int currentWave = 0;
    public WeaponPoolManager weaponPool;

    public void CompleteWave()
    {
        currentWave++;
        weaponPool.UpdateWeaponPool(currentWave);
    }
}
