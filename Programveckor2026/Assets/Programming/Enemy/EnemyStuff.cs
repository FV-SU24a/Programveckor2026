using System.Collections;
using System.Xml.Linq;
using UnityEngine;

public class EnemyStuff : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private float speed = 3f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackRate = 3f;

    private bool isAlive = true;
    private bool isAttacking = false;

    private Transform target;
    private Playerhealth1 playerHealth;
    private float attackCooldown;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        currentHealth = maxHealth;
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        playerHealth = target?.GetComponent<Playerhealth1>();
        attackCooldown = 0f;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isAlive || target == null || CompareTag("Boss")) return;

        float distanceX = Mathf.Abs(target.position.x - rb.position.x);

        if (distanceX > attackRange)
        {
            MoveTowardsPlayer();
        }
        else if (attackCooldown <= 0f)
        {
            AttackPlayer();
        }

        if (attackCooldown > 0f)
            attackCooldown -= Time.deltaTime;
    }

    private void MoveTowardsPlayer()
    {
        if (target == null) return;

        // Calculate horizontal distance only
        float distanceX = target.position.x - transform.position.x;

        // Only move if outside attack range
        if (Mathf.Abs(distanceX) > attackRange)
        {
            float moveStep = Mathf.Sign(distanceX) * speed * Time.fixedDeltaTime;

            // Clamp movement so enemy stops at attackRange
            if (Mathf.Abs(moveStep) > Mathf.Abs(distanceX) - attackRange)
                moveStep = distanceX - Mathf.Sign(distanceX) * attackRange;

            Vector2 newPos = new Vector2(rb.position.x + moveStep, rb.position.y);
            rb.MovePosition(newPos);
        }
    }




    private void AttackPlayer()
    {
        if (playerHealth == null) return;

        isAttacking = true;
        playerHealth.TakeDamage(damage);
        Debug.Log($"{name} attacks the player for {damage} damage");
        attackCooldown = attackRate;

        StartCoroutine(ResetAttackState());
    }

    private IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
    }

    public void TakeDamage(int amount)
    {
        if (!isAlive) return;
        currentHealth -= amount;
        Debug.Log($"{name} took {amount} damage, remaining health: {currentHealth}");

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        isAlive = false;
        Debug.Log($"{name} died!");
        Destroy(gameObject);
    }

    public bool IsAttacking() => isAttacking;
}
