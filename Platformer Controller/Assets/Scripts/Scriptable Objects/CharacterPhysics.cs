using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPhysics : ScriptableObject
{
	[Header("Player Ground Physics")]
	[SerializeField] float maxSpeed = 8f;
	[SerializeField] float timeToReachMaxSpeed = .25f;
	[SerializeField] float timeToStop = .15f;

	[Header("Player Jump Physics")]
	[SerializeField] float runningJumpHeight = 5.5f;
	[SerializeField] float standingJumpHeight = 4.2f;
	[SerializeField] float runningJumpDistance = 8f;

	[Header("Player In-Air Physics")]
	[SerializeField] float timeToReachMaxSpeedAir = .25f;
	[SerializeField] float fastFallGravityScale = 2f;
	[SerializeField] float maxFallSpeed = 18f;
	[SerializeField] float maxSlowFallSpeed = 15f;

	public float MaxSpeed => maxSpeed;
	public float TimeToMaxSpeed => timeToReachMaxSpeed;
	public float TimeToStop => timeToStop;
	public float Acceleration => maxSpeed / timeToReachMaxSpeed;
	public float Decceleraton => maxSpeed / timeToStop;
	public float JumpTime => (runningJumpDistance / 2f) / maxSpeed;
	public float TrueMaxJumpVelocity => 2f * runningJumpHeight / JumpTime;
	public float Gravity => TrueMaxJumpVelocity / JumpTime;
	public float MaxJumpVelocity => Mathf.Sqrt(2 * Gravity * runningJumpHeight) + Gravity * Time.fixedDeltaTime;
	public float MinJumpVelocity => Mathf.Sqrt(2 * Gravity * standingJumpHeight) + Gravity * Time.fixedDeltaTime;
	public float TimeToMaxSpeedAir => timeToReachMaxSpeedAir;
	public float AirAcceleration => maxSpeed / timeToReachMaxSpeed;
	public float FastFallFactor => fastFallGravityScale;
	public float MaxFallSpeed => maxFallSpeed;
	public float MaxSlowFallSpeed => maxSlowFallSpeed;

}
