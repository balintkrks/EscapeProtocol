using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float sprintSpeed = 6.5f;
    public float jumpForce = 11.5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.12f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private float horizontalInput;
    private float currentSpeed;
    private bool isGrounded;
    private bool jumpRequested;

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

        if (rb.linearVelocity.y <= 0.1f)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            isGrounded = false;
        }

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
            jumpRequested = true;
            isGrounded = false;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Attack();
        }

        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
    }

    private void FixedUpdate()
    {
        float verticalVelocity = rb.linearVelocity.y;
        if (jumpRequested == true)
        {
            verticalVelocity = jumpForce;
            jumpRequested = false;
        }

        rb.linearVelocity = new Vector2(horizontalInput * currentSpeed, verticalVelocity);
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
