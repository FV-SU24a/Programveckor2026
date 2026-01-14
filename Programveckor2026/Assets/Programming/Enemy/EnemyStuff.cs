using System.Collections;
using UnityEngine;

public class EnemyStuff : MonoBehaviour
{
    private Playerhealth1 PlayerHealth;

    private bool isAttacking = false;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth = 100;
    [SerializeField] private float speed = 3f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackRate = 3f;

    private bool isAlive = true;

    private Transform target;
    private float attackCooldown;



    private void Start()
    {
        currentHealth = maxHealth;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            target = player.transform;
            PlayerHealth = player.GetComponent<Playerhealth1>();
        }
        attackCooldown = 0f;
    }

    private void Update()
    {

        if (CompareTag("Boss")) return; //if its a boss it ignores all normal behaviour

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
        if (PlayerHealth == null) return;

        isAttacking = true; // start attack

        PlayerHealth.TakeDamage(damage);
        Debug.Log($"{gameObject.name} attacks the player for {damage} damage");

        // Reset attack state after a short delay so animation can play
        StartCoroutine(ResetAttackState());
    }

    private IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(0.2f); // match roughly the animation timing
        isAttacking = false;
    }

    // public accessor for other scripts
    public bool IsAttacking()
    {
        return isAttacking;
    }
    public void TakeDamage(int amount)
    {
        if (!isAlive) return;
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakePlayerDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage from player, remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        Debug.Log($"{gameObject.name} died from player attack!");
        Destroy(gameObject);
    }
}
