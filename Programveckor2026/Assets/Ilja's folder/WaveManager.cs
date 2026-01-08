using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int currentWave = 0;
    public WeaponPoolManager weaponPool;

    public void CompleteWave()
    {
        currentWave++;
        weaponPool.UpdateWeaponPool(currentWave);
    }
}
