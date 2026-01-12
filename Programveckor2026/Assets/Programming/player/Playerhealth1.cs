using UnityEngine;

public class Playerhealth1 : MonoBehaviour
{

    public int health;
    public int maxHealth = 150;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        health = Mathf.Clamp(health, 0, maxHealth);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
