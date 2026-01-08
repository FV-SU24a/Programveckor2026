using UnityEngine;

public class EnemyStuff : MonoBehaviour
{
    private int health;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float speed = 3f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackRate = 3f;

    private bool isAlive = true;

    private Transform target;
    private float attackCooldown;

    private void Start()
    {
        health = maxHealth;
        target = GameObject.FindWithTag("Player")?.transform;
        attackCooldown = 0f;
    }

    private void Update()
    {
        if (!isAlive || target == null) return;

        //distance to player
        float distance = Vector2.Distance(transform.position, target.position);
        //move if outside the attack range
        if(distance > attackRange) 
        {
            MoveTowardsPlayer();
        }
        else
        {
            //try attacking if cooldown allows
            if(attackCooldown <= 0)
            {
                AttackPlayer();
                attackCooldown = attackRate;
            }
        }

        //reduce attack cool down each frame
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;

    }

    private void MoveTowardsPlayer() 
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
    private void AttackPlayer() 
    {
        //place holder
        Debug.Log($"{gameObject.name} attacks the player for {damage} damage");
    }
    private void TakeDamage(int amount)
    {
        if (!isAlive) return;
        health -= amount;
        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        Debug.Log($"{gameObject.name} died");

        Destroy(gameObject, 1f); //delay if death animation comes
    }
}
