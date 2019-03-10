using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Player
{
	public class Idle : Anathema.Fsm.PlayerState
	{
		private JumpFall jumpFallState;

		// FIXME: Gambiarra
		private bool jumpCorrection;

		private void Start()
		{
			jumpFallState = GetComponent<JumpFall>();
		}

		public override void Enter() {	}

		// FIXME: Gambiarra
		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.Space))
				jumpCorrection = true;
		}

		/// <summary>
		/// 	In this class, the FixedUpdate is being used to transition between the idle state and other states.
		/// </summary>
		void FixedUpdate()
		{
			float HorizontalAxis = Input.GetAxisRaw("Horizontal");
			
			if(!jumpFallState.CheckIfGrounded())
			{
				animator.SetBool("IsFalling", true);
				fsm.Transition<JumpFall>();
				return;
			}

			// Transitions between the idle state and the walking state
			if(HorizontalAxis != 0f)
			{
				fsm.Transition<Walking>();

				sRenderer.flipX = HorizontalAxis < 0f ? true : false;

				animator.SetBool("IsWalking", true);
			}
			else
				rBody.velocity = new Vector2(0f, rBody.velocity.y);

			// Transitions between the idle state and the jumping state, more specifically the Jump Ascension portion of the state
			if(jumpCorrection)
			{
				jumpCorrection = false;
				animator.SetBool("IsRising", true);
				rBody.velocity = new Vector2(rBody.velocity.x, 0f);
				fsm.Transition<JumpRise>();
				return;
			}

			if(Input.GetKey(KeyCode.J))
			{
				animator.SetBool("IsAttacking", true);
				fsm.Transition<Attack>();
				return;
			}

			if(Input.GetKey(KeyCode.K))
			{
				animator.SetBool("IsFire", true);
				fsm.Transition<FireAttack>();
				return;
			}

			if(Input.GetKey(KeyCode.S))
			{
				animator.SetBool("IsCrouching", true);
				fsm.Transition<Crouch>();
			}
		}

		public override void Exit()
		{
			// FIXME: Gambiarra
			jumpCorrection = false;
		}
	}
}