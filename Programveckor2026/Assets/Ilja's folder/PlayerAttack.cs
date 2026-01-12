using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int baseDamage = 1; // Default damage, can be overridden by weapon
    public LayerMask enemyLayers;
    public float attackCooldown = 0.4f;

    private float nextAttackTime = 0f;
    private SpriteRenderer sr;

    // Optional: weapon reference (we’ll use this later)
    [HideInInspector] public int weaponDamage;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        weaponDamage = baseDamage; // start with default damage
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack()
    {
        // Determine attack position based on facing direction
        Vector3 direction = sr.flipX ? Vector3.left : Vector3.right;
        Vector3 attackPos = attackPoint.position + direction * attackRange / 2f;

        // Detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, attackRange, enemyLayers);

        foreach (Collider2D hit in hitEnemies)
        {
            EnemyHealth enemyHealth = hit.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(weaponDamage);
            }
        }

        Debug.Log("Attacked " + (sr.flipX ? "left" : "right") + " for " + weaponDamage + " damage.");
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null || sr == null) return;

        Vector3 direction = sr.flipX ? Vector3.left : Vector3.right;
        Vector3 attackPos = attackPoint.position + direction * attackRange / 2f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos, attackRange);
    }
}
