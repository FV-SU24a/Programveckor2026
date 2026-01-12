using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weapon; // Assign this in the inspector

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerAttack playerAttack = other.GetComponent<PlayerAttack>();
        if (playerAttack != null)
        {
            // Give the player the weapon
            playerAttack.weaponDamage = weapon.damage;
            playerAttack.attackRange = weapon.range;

            Debug.Log("Picked up weapon: " + weapon.weaponName);

            // Remove weapon from scene
            Destroy(gameObject);
        }
    }
}
