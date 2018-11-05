using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

namespace Anathema.Player
{
	public class Idle : Anathema.Fsm.PlayerState
	{
		[SerializeField] private Controls controls;
		public override void Enter()
		{
			controls.Enable();
			controls.main.Attack.performed += Attack;
			controls.main.Crouch.started += Crouch;
			controls.main.Jump.started += Jump;
			controls.main.HorizontalMovement.started += HorizontalMovement;
		}

		private void HorizontalMovement(InputAction.CallbackContext context)
		{
			float HorizontalAxis = context.ReadValue<float>();
			// Transitions between the idle state and the walking state
			if(HorizontalAxis != 0f)
			{
				if (HorizontalAxis > 0f)
					fsm.Transition<WalkingRight>();
				else
					fsm.Transition<WalkingLeft>();

				sRenderer.flipX = HorizontalAxis < 0f ? true : false;

				animator.SetBool("IsWalking", true);
			}
			else
				rBody.velocity = new Vector2(0f, rBody.velocity.y);
		}

		// Transitions between the idle state and the jumping state, more specifically the Jump Ascension portion of the state
		private void Jump(InputAction.CallbackContext context)
		{
			animator.SetBool("IsRising", true);
			rBody.velocity = new Vector2(rBody.velocity.x, 0f);
			fsm.Transition<JumpRise>();
			return;
		}

		private void Attack(InputAction.CallbackContext context)
		{
			Debug.Log("Attack!!!!");
			animator.SetBool("IsAttacking", true);
			fsm.Transition<Attack>();
		}

		private void Crouch(InputAction.CallbackContext context)
		{
			animator.SetBool("IsCrouching", true);
			fsm.Transition<Crouch>();
		}

		public override void Exit()
		{	
			controls.main.Attack.performed -= Attack;
			controls.main.Crouch.started -= Crouch;
			controls.main.Jump.started -= Jump;
			controls.main.HorizontalMovement.started -= HorizontalMovement;
		}
	}
}