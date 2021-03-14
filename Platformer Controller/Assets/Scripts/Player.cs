using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

	[SerializeField] CharacterPhysics playerPhysics;

	float smoothDampX;

	Vector2 moveDir;
	Vector2 currentVelocity;

	Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		currentVelocity = rb.velocity;

		MovePlayer();

		rb.velocity = currentVelocity;
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
		else
		{
			currentVelocity.x = Mathf.SmoothDamp(currentVelocity.x, 0f, ref smoothDampX, playerPhysics.TimeToStop, playerPhysics.MaxSpeed);
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
			rb.velocity = new Vector2(rb.velocity.x, playerPhysics.MinJumpVelocity);
		}
		else if (context.canceled)
		{
			rb.gravityScale = playerPhysics.FastFallFactor;
		}
	}

}
