using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

	[SerializeField] CharacterPhysics playerPhysics;

	Vector2 moveDir;

	Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		float targetvelocity = moveDir.x * playerPhysics.MaxSpeed;
		rb.velocity = new Vector2(targetvelocity, rb.velocity.y);
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		Debug.Log("is moving");
		moveDir = context.ReadValue<Vector2>();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Debug.Log("is jumping");
			rb.gravityScale = 1f;
			rb.velocity = new Vector2(rb.velocity.x, playerPhysics.MinJumpVelocity);
		}
		else if (context.canceled)
		{
			Debug.Log("stopped jumping");
			rb.gravityScale = playerPhysics.FastFallFactor;
		}
	}

}
