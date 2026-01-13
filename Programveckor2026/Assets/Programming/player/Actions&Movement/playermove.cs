using UnityEngine;

public class playermove : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float moveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 vel = Vector2.zero;

        if (Input.GetKey(KeyCode.D))
        {
            vel.x = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            vel.x = -1;
        }

        rb.linearVelocity = vel.normalized * moveSpeed;
    }
}
