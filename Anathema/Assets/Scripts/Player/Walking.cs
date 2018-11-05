using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

namespace Anathema.Player
{
	public abstract class Walking : Anathema.Fsm.PlayerState
	{
		[Tooltip("Speed in which the player is able to move.")]
		[SerializeField] protected float baseSpeed;

		[Tooltip("The EXACT measure between the center of the player and the ground level when he's grounded.")]
		[SerializeField] float groundRayDist;

		[Tooltip("An added distance that is added to the Ground Ray Distance to detect ground in uneven ground, such as stairs and slopes.")]
		[Range(0f, 1f)] [SerializeField] float safetyGroundThreshold;

		[Tooltip("An offset distance between the center of the player and where the raycast actually begins casting. Has to be lower than groundRayDist.")]
		[SerializeField] float raycastOffset;

		[Tooltip("The maximum slope angle in which the player can walk on, without falling.")]
		[SerializeField] float steepSlopeAngle;
		[SerializeField] private Controls controls;

		// Stores the result of the raycast
		private RaycastHit2D rayHit;

		// Stores the vector in which the player's velocity needs to be multiplied by
		protected Vector2 moveDirection;

		public sealed override void Enter()
		{
			controls.Enable();
			controls.main.Attack.performed += Attack;
			controls.main.Crouch.started += Crouch;
			controls.main.Jump.started += Jump;
			controls.main.HorizontalMovement.cancelled += StopMovement;
			StartMovement();
		}
		
		protected abstract void StartMovement();

		/// <summary>
		/// 	Gets ground information, more specifically the angle of the current, being walked at, tile.
		/// 	Based on the normal vector of said tile, returns, by parameter, the vector in which the player's velocity, on movement, needs to move at.
		/// </summary>
		/// <param name="rayHit"> The result of the raycast. </param>
		/// <param name="moveDirection"> The resulting vector of the player's movement. </param>
		void GetGroundInformation(RaycastHit2D rayHit, out Vector2 moveDirection)
		{
			float slopeAngle = Vector2.Angle(Vector2.up, rayHit.normal);
			// Debug.DrawRay(this.transform.position, rayHit.normal, Color.green, 2f);
					
			if(slopeAngle <= steepSlopeAngle)
			{
				// Debug.Log(slopeAngle);
				(moveDirection = Quaternion.Euler(0, 0, 270f) * rayHit.normal).Normalize();
				//moveDirection.Set(Mathf.Abs(moveDirection.x), Mathf.Abs(moveDirection.y));
			}
			else
				moveDirection = Vector2.right;
		}

		/// <summary>
		/// 	In this class, the Update is being used to raycast the groundcheck and to get the vector in which possible player movement needs to be at.
		/// </summary>
		void Update()
		{
			rayHit = Physics2D.Raycast(transform.position + Vector3.down * raycastOffset, Vector2.down,
			(groundRayDist + safetyGroundThreshold - raycastOffset), LayerMask.GetMask("Ground"));
			Debug.DrawRay(transform.position + Vector3.down * raycastOffset, Vector2.down * (groundRayDist + safetyGroundThreshold - raycastOffset), Color.green);

			GetGroundInformation(rayHit, out moveDirection);
			// Debug.DrawRay(this.transform.position, moveDirection, Color.blue, 2f);
			
		}

		/// <summary>
		/// 	In this class, the FixedUpdate is being used to switch out of movement states and to process the player's movement, while snapping the player to the floor.
		/// </summary>
		void FixedUpdate()
		{
			//	Checks if the player is grounded, if not, changes to the falling state
			if(!rayHit)
			{
				animator.SetBool("IsFalling", true);
				fsm.Transition<JumpFall>();
			}

			// Debug.Log(rayHit.distance);

			//	Ground Correction: Corrects the player's Y component when he's sunk into the ground or isn't close enough to it, but still considered grounded
			if(rayHit.distance > ((groundRayDist - raycastOffset) - safetyGroundThreshold) &&
			rayHit.distance < ((groundRayDist - raycastOffset) + safetyGroundThreshold))
				this.transform.position += ((Vector3)rayHit.normal * ((groundRayDist - raycastOffset) - rayHit.distance));
		}

		//	Changes state if the player Jumps
		private void Jump(InputAction.CallbackContext context)
		{
			animator.SetBool("IsRising", true);
			rBody.velocity = new Vector2(rBody.velocity.x, 0f);
			fsm.Transition<JumpRise>();
			return;
		}

		private void Attack(InputAction.CallbackContext context)
		{
			animator.SetBool("IsAttacking", true);
			animator.SetBool("IsWalking", false);
			rBody.velocity = Vector2.zero;
			fsm.Transition<Attack>();
			return;
		}

		private void Crouch(InputAction.CallbackContext context)
		{
			animator.SetBool("IsCrouching", true);
			animator.SetBool("IsWalking", false);
			rBody.velocity = Vector2.zero;
			fsm.Transition<Crouch>();
			return;
		}
		private void StopMovement(InputAction.CallbackContext context)
		{
			Debug.Log("Cancelling: " + context.ReadValue<float>());
			rBody.velocity = Vector2.zero;
			fsm.Transition<Idle>();
			animator.SetBool("IsWalking", false);
		}
		public sealed override void Exit()
		{	
			controls.main.Attack.performed -= Attack;
			controls.main.Crouch.started -= Crouch;
			controls.main.Jump.started -= Jump;
			controls.main.HorizontalMovement.cancelled -= StopMovement;
		}
	}
}