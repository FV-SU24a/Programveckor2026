using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;

    [Header("Attack Settings")]
    public float knockbackForce = 8f;
    public Transform attackPoint;
    public float attackRange = 2f;
    public int baseDamage = 1; // Default damage, can be overridden by weapon
    public LayerMask enemyLayers;
    public float attackCooldown = 0.4f;

    private float nextAttackTime = 0f;
    private SpriteRenderer sr;

    // Optional: weapon reference (we’ll use this later)
    [HideInInspector] public int weaponDamage;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        weaponDamage = baseDamage; // start with default damage
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextAttackTime)
        {
            anim.SetTrigger("Attack");
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void Attack()
    {
        Debug.Log("ATTACK EVENT WORKING");
        // Determine attack position based on facing direction
        Vector3 direction = sr.flipX ? Vector3.right : Vector3.left;
        Vector3 attackPos = attackPoint.position + direction * attackRange / 2f;

        Debug.Log($"Attack position: {attackPos}, attack range: {attackRange}");

        // Detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, attackRange, enemyLayers);
        print("length enemies: " + hitEnemies.Length);
        Debug.Log($"Enemies hit count: {hitEnemies.Length}");

        foreach (Collider2D hit in hitEnemies)
        {
            EnemyStuff enemyHealth = hit.GetComponentInParent<EnemyStuff>();
            if (enemyHealth != null)
            {
                Debug.Log($"Hit enemy: {enemyHealth.name}");
                enemyHealth.TakeDamage(weaponDamage);

                // Apply horizontal-only knockback
                float horizontalKnockback = direction.x * knockbackForce;
                enemyHealth.ApplyKnockBack(horizontalKnockback, 0.2f);

            }

        }

        Debug.Log("Attacked " + (sr.flipX ? "right" : "left") + " for " + weaponDamage + " damage.");
    }

    void OnDrawGizmos()
    {
        if (attackPoint == null) return;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite == null) return;

        // LEFT-facing sprites logic
        Vector3 direction = sprite.flipX ? Vector3.right : Vector3.left;

        Vector3 attackPos = attackPoint.position + direction * attackRange * 0.5f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos, attackRange);
    }

}
