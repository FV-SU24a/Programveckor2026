using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
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

        Debug.Log($"Attack position: {attackPos}, attack range: {attackRange}");

        // Detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, attackRange, enemyLayers);

        Debug.Log($"Enemies hit count: {hitEnemies.Length}");

        foreach (Collider2D hit in hitEnemies)
        {
            EnemyStuff enemyHealth = hit.GetComponentInParent<EnemyStuff>();
            if (enemyHealth != null)
            {
                Debug.Log($"Hit enemy: {enemyHealth.name}");
                enemyHealth.TakePlayerDamage(weaponDamage);
            }
            else
            {
                Debug.Log($"Hit something without EnemyStuff: {hit.name}");
            }
        }

        Debug.Log("Attacked " + (sr.flipX ? "left" : "right") + " for " + weaponDamage + " damage.");
    }

    void OnDrawGizmos()
    {
        if (attackPoint == null || sr == null) return;

        Vector3 direction = sr.flipX ? Vector3.left : Vector3.right;
        Vector3 attackPos = attackPoint.position + direction * attackRange / 2f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos, attackRange);
    }
}
