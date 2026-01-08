using UnityEngine;

public class WeaponGamble : MonoBehaviour
{
    public WeaponData guaranteedFirstWeapon;
    public WeaponPoolManager poolManager;

    private bool firstRollDone = false;

    public void RollWeapon()
    {
        WeaponData selectedWeapon;

        if (!firstRollDone)
        {
            selectedWeapon = guaranteedFirstWeapon;
            firstRollDone = true;
        }
        else
        {
            selectedWeapon = poolManager.GetRandomWeapon();
        }

        SpawnWeapon(selectedWeapon);
    }

    private void SpawnWeapon(WeaponData weapon)
    {
        Instantiate(weapon.weaponPrefab, transform.position, Quaternion.identity);
    }
}
