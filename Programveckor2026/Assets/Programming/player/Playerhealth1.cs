using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
          SceneManager.LoadScene(1);
        }
    }

    void UpdateHealthUI()
    {
        if(healthText != null)
        {
            healthText.text = $"HP: {health}/{maxHealth}";
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }


}
