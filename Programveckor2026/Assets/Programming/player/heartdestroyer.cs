using UnityEngine;

public class heartdestroyer : MonoBehaviour
{
    [Header("Player Reference")]
    public Playerhealth1 player;

    [Header("Life Images (Top ? Bottom)")]
    public GameObject[] lifeImages;

    public int lifeChunk = 30;

    void Start()
    {
        UpdateLives();
    }

 
    void Update()
    {
        UpdateLives();
    }

    void UpdateLives()
    {
        if (player == null) return;

        int hp = player.health;

        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (hp > 0)
                lifeImages[i].SetActive(true);
            else
                lifeImages[i].SetActive(false);

            hp -= lifeChunk;
        }

    }
}
