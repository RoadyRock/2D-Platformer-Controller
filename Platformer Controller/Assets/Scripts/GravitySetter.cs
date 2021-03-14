using UnityEngine;

public class GravitySetter : MonoBehaviour
{
	[SerializeField] CharacterPhysics physics;

	private void Start()
	{
		ResetGravity();
	}

	public void ResetGravity()
	{
		SetGravity(Vector2.down, physics.Gravity);
	}

	public void SetGravity(Vector2 direction, float magnitude)
	{
		Physics2D.gravity = direction.normalized * magnitude;
	}
}
