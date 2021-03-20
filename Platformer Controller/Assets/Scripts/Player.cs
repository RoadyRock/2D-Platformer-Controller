using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterPhysics playerPhysics;
    [SerializeField] LayerMask groundMask;

    float smoothDampX;
    float jumpBuffer;
    float coyoteTimer;

    bool isGrounded;

    Vector2 moveDir;
    Vector2 lastMoveDir;

    Rigidbody2D rb;
    BoxCollider2D bc;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        if (moveDir.x != 0f)
            AcceleratePlayer();
        else if (isGrounded)
            DeceleratePlayer();
        CapMoveSpeed();
        Jump();
        CapFallSpeed();
    }

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapBox(rb.position - Vector2.up * .001f, bc.size - Vector2.right * .01f, 0f, groundMask);
    }

    void AcceleratePlayer()
    {
        if (isGrounded)
        {
            rb.AddForce(playerPhysics.Acceleration * moveDir.x);
        }
        else
        {
            rb.AddForce(playerPhysics.AirAcceleration * moveDir.x);
        }
    }

    void DeceleratePlayer()
    {
        if (rb.velocity.x != 0f)
        {
            rb.AddForce(playerPhysics.Deceleraton * Mathf.Sign(rb.velocity.x));
            if (Mathf.Sign(rb.velocity.x) == 1f && rb.velocity.x < 1f)
            {
                rb.velocity *= Vector2.up;
            }
            else if (Mathf.Sign(rb.velocity.x) == -1f && rb.velocity.x > -1f)
            {
                rb.velocity *= Vector2.up;
            }
        }
    }

    void CapMoveSpeed()
    {
        if (isGrounded || moveDir.x != 0f)
        {
            if (Mathf.Abs(rb.velocity.x) > playerPhysics.MaxSpeed && Mathf.Sign(rb.velocity.x) == lastMoveDir.x)
            {
                rb.AddForce(playerPhysics.Acceleration * -moveDir.x * 1.1f);
            }
        }
    }

    void Jump()
    {
        jumpBuffer -= Time.fixedDeltaTime;
        if (isGrounded)
        {
            coyoteTimer = playerPhysics.CoyoteTime;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }
        if (jumpBuffer > 0f)
        {
            if (coyoteTimer > 0f)
            {
                jumpBuffer = 0f;
                coyoteTimer = 0f;
                float extraJump;
                float speedPercentage = Mathf.Abs(rb.velocity.x) / playerPhysics.MaxSpeed;

                if (speedPercentage <= 1f)
                    extraJump = speedPercentage * playerPhysics.JumpVelocityRange;
                else
                    extraJump = playerPhysics.JumpVelocityRange;

                rb.velocity *= Vector2.right;
                rb.AddForce(Vector2.up * (playerPhysics.MinJumpVelocity + extraJump), ForceMode2D.Impulse);
            }
        }
    }

    void CapFallSpeed()
    {        
        if (rb.gravityScale > 1f)
        {
            if (rb.velocity.y < -playerPhysics.MaxFallSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -playerPhysics.MaxFallSpeed);
            }
        }
        else
        {
            if (rb.velocity.y < -playerPhysics.MaxSlowFallSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -playerPhysics.MaxSlowFallSpeed);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
        if (context.performed){
            lastMoveDir = moveDir;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rb.gravityScale = 1f;
            jumpBuffer = playerPhysics.CoyoteTime;
        }
        else if (context.canceled)
        {
            rb.gravityScale = playerPhysics.FastFallFactor;
        }
    }

}
