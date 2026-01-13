using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float patrolLeftX;
    public float patrolRightX;

    public int slashDamage = 40;

    private Playerhealth1 playerHealth;

    public Animator animator;

    //forgot to actually make the erratic movement random...so here is the stuff for that
    public float directionChangeInteralMin = 1f;
    public float directionChangeIntervalMax = 2f;
    private float directionChangeTimer = 0f;
    private bool movingRight = true; //direction for its "patrol"

    private bool isRetreating = false;
    public float retreatDistance = 1f;
    public float retreatSpeed = 6f;
    private Vector3 retreatTarget; //the x position it retreats to

    public float attackRange = 2f;
    public float chargeSpeed = 8f;
    public float attackCooldown = 5f;
    private bool isAttacking = false;


    public Transform player;

    private float meleeCooldownTimer = 0f;
    private float chargeCooldownTimer = 0f;
    private bool isCharging = false;
    private Vector3 targetPosition; //for the charge attack

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                playerHealth = playerObj.GetComponent<Playerhealth1>();
            }
            else
            {
                Debug.LogWarning("Player not found! Make sure it has the 'Player' tag.");
                enabled = false; // stop this script safely
                return;
            }
        }
        else
        {
            // only get Playerhealth if player was already assigned in inspector
            playerHealth = player.GetComponent<Playerhealth1>();
        }

        directionChangeTimer = Random.Range(directionChangeInteralMin, directionChangeIntervalMax);
    }



    private void Update()
    {
        meleeCooldownTimer -= Time.deltaTime;
        chargeCooldownTimer -= Time.deltaTime;

        float distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x);

        if (!isCharging && !isAttacking && meleeCooldownTimer <= 0f && distanceToPlayer <= attackRange)
        {
            TriggerMeleeAttack();
        }

        if (isRetreating)
        {
            RetreatMovement();
            return; // skip patrols while retreating
        }

        if (!isCharging && chargeCooldownTimer <= 0f)
        {
            StartChargeAttack();
            chargeCooldownTimer = attackCooldown; // reset charge cooldown
        }

        if (isCharging)
            ChargeMovement();
        else
            PatrolMovement();
    }


    void PatrolMovement()
    {
        directionChangeTimer -= Time.deltaTime;

        if(directionChangeTimer <= 0f)
        {
            movingRight = !movingRight; //flips the direction its "patrolling" in
            directionChangeTimer = Random.Range(directionChangeInteralMin, directionChangeIntervalMax);
        }

        float step = moveSpeed * Time.deltaTime;
        transform.position += (movingRight ? Vector3.right : Vector3.left) * step;

        //stay inside its little bubble..or yknow doesnt move past specific points so it doesnt run away
        if (transform.position.x >= patrolRightX) movingRight = false;
        if (transform.position.x <= patrolLeftX) movingRight = true;

    }

    void StartChargeAttack()
    {
        isCharging = true;
        targetPosition = player.position; //for the charge attack
    }

    void ChargeMovement()
    {
        float step = chargeSpeed * Time.deltaTime;

        //this will track the players position
        Vector3 currentTarget = new Vector3(player.position.x, transform.position.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, currentTarget, step);

        //stop when close enough to the player to attack
        if (Mathf.Abs(transform.position.x - player.position.x) <= attackRange)
        {
            isCharging = false;
            StartCoroutine(PerformSlashAttack());
            StartRetreat();
        }
    }



    private IEnumerator PerformSlashAttack()
    {
        animator.SetTrigger("Attack");

        // wait until the slash hits
        yield return new WaitForSeconds(0.5f);
        playerHealth?.TakeDamage(slashDamage);

        // wait until animation ends
        yield return new WaitForSeconds(0.3f);

        // allow boss to attack again after cooldown is fully respected
        isAttacking = false;
    }

    private void TriggerMeleeAttack()
    {
        //block further attacks immediately
        isAttacking = true;
        meleeCooldownTimer = attackCooldown;

        //start the attack coroutine
        StartCoroutine(PerformSlashAttack());
    }


    void StartRetreat()
    {
        isRetreating = true;

        //move opposite the player
        float direction = transform.position.x < player.position.x ? -1f : 1f;

        retreatTarget = new Vector3(transform.position.x + direction * retreatDistance, transform.position.y, transform.position.z);
    }


    void RetreatMovement()
    {
        float step = retreatSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, retreatTarget, step);

        //stops retreating once reaching the target
        if(Mathf.Abs(transform.position.x - retreatTarget.x) <= 0.1f)
        {
            isRetreating = false;

            //goes back to patrolling
            directionChangeTimer = Random.Range(directionChangeInteralMin, directionChangeIntervalMax);
        }
    }
}
