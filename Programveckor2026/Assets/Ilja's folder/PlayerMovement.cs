using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayers;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movement
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.A)) moveInput = -1f;
        if (Input.GetKey(KeyCode.D)) moveInput = 1f;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Flip sprite
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);

        // Jump
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Drop through platform
        if (Input.GetKeyDown(KeyCode.S) && isGrounded)
        {
            StartCoroutine(DropThroughPlatform());
        }
    }

    IEnumerator DropThroughPlatform()
    {
        Collider2D playerCollider = GetComponent<Collider2D>();

        // Find all overlapping platforms
        Collider2D[] platforms = Physics2D.OverlapCircleAll(transform.position, 0.5f, LayerMask.GetMask("Platform"));

        foreach (Collider2D platform in platforms)
        {
            Physics2D.IgnoreCollision(playerCollider, platform, true);
        }

        yield return new WaitForSeconds(0.3f);

        foreach (Collider2D platform in platforms)
        {
            Physics2D.IgnoreCollision(playerCollider, platform, false);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
