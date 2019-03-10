using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Player
{
	public class Walking : Anathema.Fsm.PlayerState
	{
		[Tooltip("Speed in which the player is able to move.")]
		[SerializeField] float baseSpeed;

		[Tooltip("The EXACT measure between the center of the player and the ground level when he's grounded.")]
		[SerializeField] float groundRayDist;

		[Tooltip("An added distance that is added to the Ground Ray Distance to detect ground in uneven ground, such as stairs and slopes.")]
		[SerializeField] float safetyGroundThreshold;

		[Tooltip("An offset distance between the center of the player and where the raycast actually begins casting. Has to be lower than groundRayDist.")]
		[SerializeField] float raycastOffset;

		[Tooltip("The maximum slope angle in which the player can walk on, without falling.")]
		[SerializeField] float steepSlopeAngle;

		// Stores the result of the raycast
		private RaycastHit2D rayHit;

		// Stores the vector in which the player's velocity needs to be multiplied by
		private Vector2 moveDirection;

		private PlayerUpgrades playerUpgrades;

		// FIXME: Gambiarra
		private bool jumpCorrection;

		private void Start()
		{
			playerUpgrades = GetComponent<PlayerUpgrades>();
			if (!playerUpgrades) Debug.LogError($"{gameObject.name}: {nameof(Crouch)}: Couldn't find {nameof(PlayerUpgrades)}.");
		}

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
				(moveDirection = Quaternion.Euler(0, 0, 270f) * rayHit.normal).Normalize();
			}
			else
				moveDirection = Vector2.right;
		}

		/// <summary>
		/// 	In this class, the Update is being used to raycast the groundcheck and to get the vector in which possible player movement needs to be at.
		/// </summary>
		void Update()
		{
			// FIXME: Gambiarra
			if(Input.GetKeyDown(KeyCode.Space))
				jumpCorrection = true;

			rayHit = Physics2D.Raycast(transform.position + Vector3.down * raycastOffset, Vector2.down,
			(groundRayDist + safetyGroundThreshold - raycastOffset), LayerMask.GetMask("Ground"));
			Debug.DrawRay(transform.position + Vector3.down * raycastOffset, Vector2.down * (groundRayDist + safetyGroundThreshold - raycastOffset), Color.green);

			GetGroundInformation(rayHit, out moveDirection);
			// Debug.DrawRay(this.transform.position, moveDirection, Color.blue, 2f);


			// FIXME: Oh, the Gambiarra. It hurts so much. It's painful.
			Vector3 leftOrRight = sRenderer.flipX ? Vector3.left : Vector3.right;
			RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.down * groundRayDist, leftOrRight, 0.2f, LayerMask.GetMask("Ground"));
			Debug.DrawRay(transform.position + Vector3.down * groundRayDist, leftOrRight * 0.1f, Color.blue, 1f);

			if(hit && Vector2.Angle(Vector2.up, hit.normal) < 60f)
			{
				GetGroundInformation(hit, out moveDirection);
				Debug.DrawRay(transform.position + Vector3.down * groundRayDist, moveDirection, Color.red, 1f);
			}
			// GAMBIARRA AHHHH
			
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
			if(jumpCorrection)
			{
				jumpCorrection = false;
				animator.SetBool("IsRising", true);
				rBody.velocity = new Vector2(rBody.velocity.x, 0f);
				fsm.Transition<JumpRise>();
				return;
			}

			if(playerUpgrades.HasScythe && Input.GetKey(KeyCode.J))
			{
				animator.SetBool("IsAttacking", true);
				animator.SetBool("IsWalking", false);
				rBody.velocity = Vector2.zero;
				fsm.Transition<Attack>();
				return;
			}

			if(playerUpgrades.HasFireAttack && Input.GetKey(KeyCode.K))
			{
				animator.SetBool("IsFire", true);
				animator.SetBool("IsWalking", false);
				rBody.velocity = Vector2.zero;
				fsm.Transition<FireAttack>();
				return;
			}

			if(Input.GetKey(KeyCode.S))
			{
				animator.SetBool("IsCrouching", true);
				animator.SetBool("IsWalking", false);
				rBody.velocity = Vector2.zero;
				fsm.Transition<Crouch>();
				return;
			}

			// Debug.Log(rayHit.distance);

			//	Ground Correction: Corrects the player's Y component when he's sunk into the ground or isn't close enough to it, but still considered grounded
			if(rayHit.distance > ((groundRayDist - raycastOffset) - safetyGroundThreshold) &&
			rayHit.distance < ((groundRayDist - raycastOffset) + safetyGroundThreshold))
				this.transform.position += ((Vector3)rayHit.normal * ((groundRayDist - raycastOffset) - rayHit.distance));

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
				rBody.velocity = Quaternion.Euler(0, 0, 180f) * moveDirection * baseSpeed;
			}

		}

		public override void Exit() 
		{
			// FIXME: Gambiarra
			jumpCorrection = false;
		}
	}
}