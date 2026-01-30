using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData weapon;

    [Header("Pickup Settings")]
    public KeyCode pickupKey = KeyCode.F;

    private bool playerInRange;
    private PlayerAttack playerAttack;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(pickupKey))
        {
            PickupWeapon();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerAttack = other.GetComponent<PlayerAttack>();

        if (playerAttack != null)
        {
            playerInRange = true;
            Debug.Log("Player in range of weapon: " + weapon.weaponName);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        playerAttack = null;
    }

    void PickupWeapon()
    {
        if (weapon == null)
        {
            Debug.LogError("WeaponPickup has NO weapon assigned!");
            return;
        }

        playerAttack.weaponDamage = weapon.damage;
        playerAttack.attackRange = weapon.range;

        Debug.Log("Picked up weapon: " + weapon.weaponName +
                  " | Damage: " + weapon.damage +
                  " | Range: " + weapon.range);

        WeaponVisual weaponVisual = playerAttack.GetComponentInChildren<WeaponVisual>();

        if (weaponVisual != null)
        {
            weaponVisual.SetWeaponSprite(weapon.weaponSprite);
        }

        Destroy(gameObject);
    }
}
