using UnityEngine;

public class SeperateAnimEnemy : MonoBehaviour
{
    [SerializeField] public Animator animator;
    [SerializeField] private string attackTriggerName = "Attack";

    private EnemyStuff enemyScript;

    private void Awake()
    {
        enemyScript = GetComponent<EnemyStuff>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (enemyScript == null) return;

        // Play attack animation when EnemyStuff is attacking
        if (enemyScript.IsAttacking())
        {
            animator.SetTrigger(attackTriggerName);
        }
    }
}
