using UnityEngine;
using TMPro;
public class Playerhealth1 : MonoBehaviour
{

    public int health;
    public int maxHealth = 150;

    public TMP_Text healthText;

    void Start()
    {
        health = maxHealth;

        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthUI();

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void UpdateHealthUI()
    {
        if(healthText != null)
        {
            healthText.text = $"HP: {maxHealth}/{health}";
        }
    }

}
