using System.Collections;
using System.Xml.Linq;
using UnityEngine;

public class EnemyStuff : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private float speed = 3f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackRate = 3f;

    private bool isAlive = true;
    private bool isAttacking = false;
    [SerializeField] private Transform attackPoint; // assign in inspector

    private Vector2 knockbackVelocity = Vector2.zero; // only horizontal
    private float knockbackTimer = 0f;


    private Transform target;
    private Playerhealth1 playerHealth;
    private float attackCooldown;

    private Rigidbody2D rb;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
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

        if (knockbackTimer > 0f)
        {
            rb.MovePosition(rb.position + knockbackVelocity * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;
            return;
        }

        // Check attack range first
        Vector2 origin = attackPoint != null ? attackPoint.position : rb.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, attackRange, LayerMask.GetMask("Player"));

        if (hits.Length > 0)
        {
            // Stop moving if in range
            isAttacking = true;

            if (attackCooldown <= 0f)
            {
                AttackPlayer();
            }
        }
        else
        {
            // Only move if player not in attack range
            isAttacking = false;
            MoveTowardsPlayer();
        }

        if (attackCooldown > 0f)
            attackCooldown -= Time.deltaTime;
    }


    private void MoveTowardsPlayer()
    {
        if (target == null) return;

        // Calculate horizontal distance only
        float distanceX = target.position.x - transform.position.x;

        // Flip sprite to face player
        if (distanceX > 0)
            sr.flipX = false;
        else if (distanceX < 0)
            sr.flipX = true;

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

    public void ApplyKnockBack(float horizontalForce, float duration = 0.2f)
    {
        knockbackVelocity = new Vector2(horizontalForce, 0f); // only push left/right
        knockbackTimer = duration;
    }


    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


}
