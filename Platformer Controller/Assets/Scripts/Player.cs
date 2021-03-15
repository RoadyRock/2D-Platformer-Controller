using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

	[SerializeField] CharacterPhysics playerPhysics;
	[SerializeField] LayerMask groundMask;

	float smoothDampX;

	bool isGrounded;
	bool jumpQueued;

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
		MovePlayer();
		Jump();

		rb.velocity = currentVelocity;
	}

	void GroundCheck()
	{
		isGrounded = Physics2D.OverlapBox(rb.position - Vector2.up * .01f, bc.size - Vector2.right * .01f, 0f, groundMask); // TODO: make this line readable
	}

	void MovePlayer()
	{
		if (moveDir.x != 0)
		{
			currentVelocity.x += playerPhysics.Acceleration * Time.fixedDeltaTime * moveDir.x;
			if (Mathf.Abs(currentVelocity.x) > playerPhysics.MaxSpeed)
			{
				currentVelocity.x = playerPhysics.MaxSpeed * moveDir.x;
			}
		}
		else if (isGrounded)
		{
			currentVelocity.x = Mathf.SmoothDamp(currentVelocity.x, 0f, ref smoothDampX, playerPhysics.TimeToStop);
		}

	}

	void Jump()
	{
		if (jumpQueued)
		{
			jumpQueued = false;
			if (isGrounded)
			{
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

	public void OnMove(InputAction.CallbackContext context)
	{
		moveDir = context.ReadValue<Vector2>();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			rb.gravityScale = 1f;
			jumpQueued = true;
		}
		else if (context.canceled)
		{
			rb.gravityScale = playerPhysics.FastFallFactor;
		}
	}

}
