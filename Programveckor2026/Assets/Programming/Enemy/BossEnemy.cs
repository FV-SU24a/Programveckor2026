using System.Collections;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float patrolLeftX;
    public float patrolRightX;
    private bool movingRight = true;
    private float directionChangeTimer = 0f;
    public float directionChangeIntervalMin = 1f;
    public float directionChangeIntervalMax = 2f;

    [Header("Retreat")]
    public float retreatDistance = 1f;
    public float retreatSpeed = 10f;
    private Vector2 retreatTarget;
    private bool isRetreating = false;

    [Header("Attack")]
    public int slashDamage = 40;
    public float attackRange = 1.5f;        // radius for hit detection
    public Vector2 attackOffset = new Vector2(1.5f, 0f); // where the arm hits relative to pivot
    public float chargeSpeed = 12f;
    public float attackCooldown = 5f;
    private float meleeCooldownTimer = 0f;
    private float chargeCooldownTimer = 0f;
    private bool isAttacking = false;
    private bool isCharging = false;

    [Header("References")]
    public Transform player;
    private Playerhealth1 playerHealth;
    private Rigidbody2D rb;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<Playerhealth1>();

            // Ignore collisions with player
            Collider2D playerCollider = playerObj.GetComponent<Collider2D>();
            Collider2D myCollider = GetComponent<Collider2D>();
            if (playerCollider != null && myCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, playerCollider, true);
            }
        }
        else
        {
            Debug.LogWarning("Player not found!");
            enabled = false;
        }

        directionChangeTimer = Random.Range(directionChangeIntervalMin, directionChangeIntervalMax);
    }

    void Update()
    {
        if (player == null) return;

        // Cooldowns
        meleeCooldownTimer -= Time.deltaTime;
        chargeCooldownTimer -= Time.deltaTime;

        float distanceToPlayer = Mathf.Abs(rb.position.x - player.position.x);

        // --- ATTACK ---
        if (!isAttacking && distanceToPlayer <= attackRange && meleeCooldownTimer <= 0f)
        {
            StartCoroutine(PerformMeleeAttack());
            return; // skip movement this frame
        }

        // --- CHARGE ---
        if (!isAttacking && !isRetreating && isCharging)
        {
            ChargeMovement();
            return;
        }

        // --- RETREAT ---
        if (!isAttacking && isRetreating)
        {
            RetreatMovement();
            return;
        }

        // --- PATROL ---
        if (!isAttacking && !isCharging && !isRetreating)
        {
            if (chargeCooldownTimer <= 0f)
            {
                StartChargeAttack();
                chargeCooldownTimer = attackCooldown;
            }
            else
            {
                PatrolMovement();
            }
        }
    }

    void PatrolMovement()
    {
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0f)
        {
            movingRight = !movingRight;
            directionChangeTimer = Random.Range(directionChangeIntervalMin, directionChangeIntervalMax);
        }

        Vector2 moveDir = movingRight ? Vector2.right : Vector2.left;
        rb.position += moveDir * moveSpeed * Time.deltaTime;

        if (rb.position.x >= patrolRightX) movingRight = false;
        if (rb.position.x <= patrolLeftX) movingRight = true;
    }

    void StartChargeAttack()
    {
        isCharging = true;
    }

    void ChargeMovement()
    {
        if (isAttacking) return;

        Vector2 target = new Vector2(player.position.x, rb.position.y);
        rb.position = Vector2.MoveTowards(rb.position, target, chargeSpeed * Time.deltaTime);

        if (Mathf.Abs(rb.position.x - player.position.x) <= attackRange)
        {
            isCharging = false;
            StartCoroutine(PerformMeleeAttack());
        }
    }

    private IEnumerator PerformMeleeAttack()
    {
        isAttacking = true;

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f); // hit frame

        // Use attack offset to match arm reach
        Vector2 attackPos = (Vector2)rb.position + attackOffset;

        // Physics check: is the player inside the attack circle?
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPos, attackRange, LayerMask.GetMask("Player"));
        if (hitPlayer != null)
        {
            Playerhealth1 ph = hitPlayer.GetComponent<Playerhealth1>();
            ph?.TakeDamage(slashDamage);
        }

        yield return new WaitForSeconds(0.3f); // finish animation

        isAttacking = false;
        meleeCooldownTimer = attackCooldown;

        StartRetreat();
    }

    void StartRetreat()
    {
        isRetreating = true;
        float direction = rb.position.x < player.position.x ? -1f : 1f;
        retreatTarget = new Vector2(rb.position.x + direction * retreatDistance, rb.position.y);
    }

    void RetreatMovement()
    {
        rb.position = Vector2.MoveTowards(rb.position, retreatTarget, retreatSpeed * Time.deltaTime);

        if (Mathf.Abs(rb.position.x - retreatTarget.x) <= 0.1f)
        {
            isRetreating = false;
            directionChangeTimer = Random.Range(directionChangeIntervalMin, directionChangeIntervalMax);
        }
    }

    // Optional: visualize attack range in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 attackPos = (Vector2)transform.position + attackOffset;
        Gizmos.DrawWireSphere(attackPos, attackRange);
    }
}
