using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;

    [Header("Combat")]
    public int damage = 1;
    public float range = 0.5f;

    [Header("Visual / Prefab")]
    public GameObject weaponPrefab;

    [Header("Unlock rules")]
    public int unlockWave; // weapon becomes available this round
}
