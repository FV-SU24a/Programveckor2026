using UnityEngine;

public class HealDrops : MonoBehaviour
{
    [SerializeField] private int healAmount = 30;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Playerhealth1 playerHealth = other.GetComponent<Playerhealth1>();
        if (playerHealth == null)
            return;

        playerHealth.Heal(healAmount);
        Destroy(gameObject);
    }
}
