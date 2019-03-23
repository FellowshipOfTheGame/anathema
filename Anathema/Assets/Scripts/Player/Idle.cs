using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Player
{
	public class Idle : Anathema.Fsm.PlayerState
	{
		private JumpFall jumpFallState;
		private PlayerUpgrades playerUpgrades;

		// FIXME: Gambiarra
		private bool jumpCorrection;

		private void Start()
		{
			jumpFallState = GetComponent<JumpFall>();
			
			playerUpgrades = GetComponent<PlayerUpgrades>();
			if (!playerUpgrades) Debug.LogError($"{gameObject.name}: {nameof(Crouch)}: Couldn't find {nameof(PlayerUpgrades)}.");
		}

		public override void Enter() {	}

		// FIXME: Gambiarra
		private void Update()
		{
			if(Input.GetButtonDown("Jump"))
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

			if(playerUpgrades.HasScythe && Input.GetButton("NormalAttack"))
			{
				animator.SetBool("IsAttacking", true);
				fsm.Transition<Attack>();
				return;
			}

			if(playerUpgrades.HasFireAttack && Input.GetButton("FireAttack"))
			{
				animator.SetBool("IsFire", true);
				fsm.Transition<FireAttack>();
				return;
			}

			if(Input.GetButton("Crouch"))
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