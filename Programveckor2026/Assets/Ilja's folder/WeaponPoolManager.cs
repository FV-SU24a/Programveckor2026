using System.Collections.Generic;
using UnityEngine;

public class WeaponPoolManager : MonoBehaviour
{
    public List<WeaponData> allWeapons;

    private List<WeaponData> unlockedWeapons = new List<WeaponData>();

    public void UpdateWeaponPool(int currentWave)
    {
        unlockedWeapons.Clear();

        foreach (WeaponData weapon in allWeapons)
        {
            if (weapon.unlockWave <= currentWave)
            {
                unlockedWeapons.Add(weapon);
            }
        }
    }

    public WeaponData GetRandomWeapon()
    {
        if (unlockedWeapons.Count == 0)
            return null;

        int index = Random.Range(0, unlockedWeapons.Count);
        return unlockedWeapons[index];
    }
}
