using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;

    [Header("Unlock rules")]
    public int unlockWave;   // weapon becomes available at this wave
}
