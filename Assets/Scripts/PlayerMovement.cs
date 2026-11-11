using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float sprintSpeed = 5f;
    public float jumpForce = 8f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private float horizontalInput;
    private float currentSpeed;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        horizontalInput = 0f;
        if (Keyboard.current.aKey.isPressed)
        {
            horizontalInput = -1f;
            sr.flipX = true;
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            horizontalInput = 1f;
            sr.flipX = false;
        }

        if (Keyboard.current.leftShiftKey.isPressed)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded == true)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Attack();
        }

        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * currentSpeed, rb.linearVelocity.y);
    }

    private void Attack()
    {
        animator.SetBool("IsAttacking", true);
        Invoke(nameof(EndAttack), 0.4f);
    }

    public void EndAttack()
    {
        animator.SetBool("IsAttacking", false);
    }
}
