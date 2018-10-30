﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Player
{
	public class Idle : Anathema.Fsm.PlayerState
	{
		public override void Enter() {	}

		/// <summary>
		/// 	In this class, the FixedUpdate is being used to transition between the idle state and other states.
		/// </summary>
		void FixedUpdate()
		{
			float HorizontalAxis = Input.GetAxisRaw("Horizontal");

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
			if(Input.GetKey(KeyCode.Space))
			{
				animator.SetBool("IsRising", true);
				rBody.velocity = new Vector2(rBody.velocity.x, 0f);
				fsm.Transition<JumpRise>();
				return;
			}

			if(Input.GetKeyDown(KeyCode.J))
			{
				Debug.Log("Attack!!!!");
				animator.SetBool("IsAttacking", true);
				fsm.Transition<Attack>();
			}

			if(Input.GetKey(KeyCode.S))
			{
				animator.SetBool("IsCrouching", true);
				fsm.Transition<Crouch>();
			}
		}

		public override void Exit() {	}
	}
}