using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weapon;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerAttack playerAttack = other.GetComponent<PlayerAttack>();
        if (playerAttack == null) return;

        playerAttack.weaponDamage = weapon.damage;
        playerAttack.attackRange = weapon.range;

        Debug.Log("Picked up: " + weapon.weaponName);

        Destroy(gameObject);
    }
}
