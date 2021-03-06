﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Anathema.Player
{
	public class JumpRise : Anathema.Fsm.PlayerState
	{

		[Tooltip("The force that defines how fast the player is lauched into the air.")]
		[SerializeField] float jumpForce;

		[Tooltip("The base, initial gravity value that pulls the player down to the ground. Is is recommended to be a little lower than the base gravity value of the falling state.")]
		[SerializeField] float baseGravity;

		[Tooltip("The speed in which, given that the player holds down the jump button, the gravity is increased by. This defines how high and for how long the player can maintain his jump.")]
		[SerializeField] float gravityFallOff;

		[Tooltip("Speed in which the player is able to control himself during the jump ascension.")]
		[SerializeField] float horizontalSpeed;

		private float currentGravity;
		private bool isLimitedToOneAttack;
		private PlayerUpgrades playerUpgrades;

		private void Start()
		{
			playerUpgrades = GetComponent<PlayerUpgrades>();
			if (!playerUpgrades) Debug.LogError($"{gameObject.name}: {nameof(Crouch)}: Couldn't find {nameof(PlayerUpgrades)}.");
			
			isLimitedToOneAttack = this.gameObject.GetComponent<JumpFall>().isLimitedToOneAttack;
		}

		public override void Enter()
		{
			animator.Play("JumpAscension", -1, 0);
			rBody.AddForce(Vector2.up * jumpForce);
			currentGravity = baseGravity;
		}

		/// <summary>
		/// 	In this class, the FixedUpdate handles the forces responsable for making the player jump and maintain that jump as long as the input
		/// is kept pressed, until the peak is reached through gravity manipulation.
		/// 	It also handles the transition to the descent portion of the jump and the air movement.
		/// </summary>
		void FixedUpdate()
		{
			float HorizontalAxis = Input.GetAxisRaw("Horizontal");

			// Handles attacking while in the air
			if(playerUpgrades.HasScythe && Input.GetButton("NormalAttack"))
			{
				rBody.velocity = Vector3.zero;
				animator.SetBool("IsAttacking", true);

				if(isLimitedToOneAttack)
					this.gameObject.GetComponent<JumpFall>().hasAttacked = true;
				
				fsm.Transition<AirAttack>();
				return;
			}

			Debug.Log("Can fire attack: " + playerUpgrades.HasFireAttack);
			if(playerUpgrades.HasFireAttack && Input.GetButton("FireAttack"))
			{
				rBody.velocity = Vector2.zero;
				animator.SetBool("IsFire", true);

				if(isLimitedToOneAttack)
					this.gameObject.GetComponent<JumpFall>().hasFireAttacked = true;
				
				fsm.Transition<AirFireAttack>();
				return;
			}

			// Handles gravity, given current value, which is modified
			rBody.AddForce(Vector2.down * currentGravity);

			// If the jump input is kept pressed, the jump is extended but the gravity is modified to give it a better feel
			// Else if the player stops holding down the input, the transition to the descent portion of the jump is instant
			if(Input.GetButton("Jump"))
				currentGravity += gravityFallOff;
			else
			{
				rBody.velocity = new Vector2(rBody.velocity.x, 0f);
				animator.SetBool("IsFalling", true);
				fsm.Transition<JumpFall>();
				return;
			}
			
			// If the player bumps into the ceiling or the jump reaches it's peak, transitions to the descent portion of the jump
			if(rBody.velocity.y <= 0f)
			{
				animator.SetBool("IsFalling", true);
				fsm.Transition<JumpFall>();
			}

			// Handles Air Movement
			if(HorizontalAxis == 0f)
				rBody.velocity = new Vector2(0f, rBody.velocity.y);
			else if(HorizontalAxis > 0f)
			{
				sRenderer.flipX = false;
				rBody.velocity = new Vector2(horizontalSpeed, rBody.velocity.y);
			}
			else
			{
				sRenderer.flipX = true;
				rBody.velocity = new Vector2(-horizontalSpeed, rBody.velocity.y);
			}
		}

		public override void Exit()
		{
			animator.SetBool("IsRising", false);
		}
	}
}