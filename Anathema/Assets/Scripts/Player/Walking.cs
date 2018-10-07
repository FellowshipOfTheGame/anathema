using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : Anathema.Fsm.PlayerState
{
	[Tooltip("Speed in which the player is able to move.")]
	[SerializeField] float baseSpeed;

	[Tooltip("The EXACT measure between the center of the player and the ground level when he's grounded.")]
	[SerializeField] float groundRayDist;

	[Tooltip("An added distance that is added to the Ground Ray Distance to detect ground in uneven ground, such as stairs and slopes.")]
	[Range(0f, 1f)] [SerializeField] float safetyGroundThreshold;

	[Tooltip("The maximum slope angle in which the player can walk on, without falling.")]
	[SerializeField] float steepSlopeAngle;

	// Stores the result of the raycast
	private RaycastHit2D rayHit;

	// Stores the vector in which the player's velocity needs to be multiplied by
	private Vector2 moveDirection;

	public override void Enter() {	}

	/// <summary>
	/// 	Gets ground information, more specifically the angle of the current, being walked at, tile.
	/// 	Based on the normal vector of said tile, returns, by parameter, the vector in which the player's velocity, on movement, needs to move at.
	/// </summary>
	/// <param name="rayHit"> The result of the raycast. </param>
	/// <param name="moveDirection"> The resulting vector of the player's movement. </param>
	void GetGroundInformation(RaycastHit2D rayHit, out Vector2 moveDirection)
	{
		float slopeAngle = Vector2.Angle(Vector2.up, rayHit.normal);
		if(slopeAngle <= steepSlopeAngle)
		{
			(moveDirection = Quaternion.Euler(0, 0, 90) * rayHit.normal).Normalize();
			moveDirection.Set(Mathf.Abs(moveDirection.x), Mathf.Abs(moveDirection.y));
		}
		else
			moveDirection = Vector2.right;
	}

	/// <summary>
	/// 	In this class, the Update is being used to raycast the groundcheck and to get the vector in which possible player movement needs to be at.
	/// </summary>
	void Update()
	{
		rayHit = Physics2D.Raycast(transform.position, Vector2.down, (groundRayDist + safetyGroundThreshold), LayerMask.GetMask("Ground"));
		Debug.DrawRay(transform.position, Vector2.down * (groundRayDist + safetyGroundThreshold), Color.green);

		GetGroundInformation(rayHit, out moveDirection);
	}

	/// <summary>
	/// 	In this class, the FixedUpdate is being used to switch out of movement states and to process the player's movement, while snapping the player to the floor.
	/// </summary>
	void FixedUpdate()
	{
		float HorizontalAxis = Input.GetAxisRaw("Horizontal");

		//	Checks if the player is grounded, if not, changes to the falling state
		if(!rayHit)
		{
			animator.SetBool("IsFalling", true);
			fsm.Transition<JumpFall>();
		}

		//	Changes state if the player Jumps
		if(Input.GetKey(KeyCode.Space))
		{
			animator.SetBool("IsRising", true);
			rBody.velocity = new Vector2(rBody.velocity.x, 0f);
			fsm.Transition<JumpRise>();
			return;
		}

		//	Ground Correction: Corrects the player's Y component when he's sunk into the ground or isn't close enough to it, but still considered grounded
		if(rayHit.distance > groundRayDist - safetyGroundThreshold && rayHit.distance < groundRayDist + safetyGroundThreshold)
			this.transform.position += ((Vector3)rayHit.normal * (groundRayDist - rayHit.distance));

		
		//	Controls normal movement and sprite flipping
		if(HorizontalAxis == 0f)
		{
			rBody.velocity = Vector2.zero;
			fsm.Transition<Idle>();
			animator.SetBool("IsWalking", false);
		}
		else if(HorizontalAxis > 0f)
		{
			sRenderer.flipX = false;
			rBody.velocity = moveDirection * baseSpeed;
		}
		else
		{
			sRenderer.flipX = true;
			rBody.velocity = moveDirection * -1 * baseSpeed;
		}

	}

	public override void Exit() {	}
}
