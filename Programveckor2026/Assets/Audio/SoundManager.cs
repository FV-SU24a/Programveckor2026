using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Enemy Sounds")]
    public AudioClip enemyHitSound;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayEnemyAttack()
    {
        audioSource.PlayOneShot(enemyHitSound);
    }
}
