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
	Vector2 currentVelocity;

	Rigidbody2D rb;
	BoxCollider2D bc;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<BoxCollider2D>();
	}

	private void FixedUpdate()
	{
		currentVelocity = rb.velocity;

		GroundCheck();
		AcceleratePlayer();
		Jump();
		CapPlayerSpeed();

		rb.velocity = currentVelocity;
	}

	void GroundCheck()
	{
		isGrounded = Physics2D.OverlapBox(rb.position - Vector2.up * .001f, bc.size - Vector2.right * .01f, 0f, groundMask); // TODO: make this line readable
	}

	void AcceleratePlayer()
	{
		if (isGrounded)
		{
			if (moveDir.x != 0)
			{
				currentVelocity.x += playerPhysics.Acceleration * Time.fixedDeltaTime * moveDir.x;
			}
			else
			{
				currentVelocity.x = Mathf.SmoothDamp(currentVelocity.x, 0f, ref smoothDampX, playerPhysics.TimeToStop);
			}
		}
		else
		{
			if (moveDir.x != 0)
			{
				currentVelocity.x += playerPhysics.AirAcceleration * Time.fixedDeltaTime * moveDir.x;
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
				float speedPercentage = Mathf.Abs(currentVelocity.x) / playerPhysics.MaxSpeed;

				if (speedPercentage <= 1f)
					extraJump = speedPercentage * playerPhysics.JumpVelocityRange;
				else
					extraJump = playerPhysics.JumpVelocityRange;

				currentVelocity.y = playerPhysics.MinJumpVelocity + extraJump;
			}
		}
	}

	void CapPlayerSpeed()
	{
		if (Mathf.Abs(currentVelocity.x) > playerPhysics.MaxSpeed)
		{
			currentVelocity.x = playerPhysics.MaxSpeed * moveDir.x;
		}
		if (rb.gravityScale > 1f)
		{
			if (currentVelocity.y < -playerPhysics.MaxFallSpeed)
			{
				currentVelocity.y = -playerPhysics.MaxFallSpeed;
			}
		}
		else
		{
			if (currentVelocity.y < -playerPhysics.MaxSlowFallSpeed)
			{
				currentVelocity.y = -playerPhysics.MaxSlowFallSpeed;
			}
		}
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		moveDir = context.ReadValue<Vector2>();
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
