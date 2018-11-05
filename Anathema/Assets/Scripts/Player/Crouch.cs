using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

namespace Anathema.Player
{
	public class Crouch : Anathema.Fsm.PlayerState
	{
		[Tooltip("The attached collider for when the player is standing/walking")]
		[SerializeField] Collider2D standingCollider;

		[Tooltip("The attached collider for when the player is crouching.")]
		[SerializeField] Collider2D crouchCollider;
		[SerializeField] private Controls controls;

		private void Start()
		{
		}	
		/// <summary>
		/// 	In this class the Enter method disables the normal collider and enables the crouching collider
		/// </summary>
		public override void Enter()
		{
			standingCollider.enabled = false;
			crouchCollider.enabled = true;
			controls.main.Crouch.performed += Stand;
		}

		private void Stand(InputAction.CallbackContext context)
		{
			animator.SetBool("IsCrouching", false);
			fsm.Transition<Idle>();
		}

		/// <summary>
		/// 	In this class the Exit method disables the crouching collider and enables the standing collider
		/// </summary>
		public override void Exit()
		{
			standingCollider.enabled = true;
			crouchCollider.enabled = false;
			controls.main.Crouch.performed -= Stand;
		}
	}
}