﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Player
{
	public class Crouch : Anathema.Fsm.PlayerState
	{
		[Tooltip("The attached collider for when the player is standing/walking")]
		[SerializeField] Collider2D standingCollider;

		[Tooltip("The attached collider for when the player is crouching.")]
		[SerializeField] Collider2D crouchCollider;

		private PlayerUpgrades playerUpgrades;

		private void Start()
		{
			playerUpgrades = GetComponent<PlayerUpgrades>();
			if (!playerUpgrades) Debug.LogError($"{gameObject.name}: {nameof(Crouch)}: Couldn't find {nameof(PlayerUpgrades)}.");	
		}

		/// <summary>
		/// 	In this class the Enter method disables the normal collider and enables the crouching collider
		/// </summary>
		public override void Enter()
		{
			standingCollider.enabled = false;
			crouchCollider.enabled = true;
		}

		void Update()
		{
			if(!Input.GetButton("Crouch"))
			{
				animator.SetBool("IsCrouching", false);
				fsm.Transition<Idle>();
			}

			if(playerUpgrades.HasScythe && Input.GetButtonDown("NormalAttack"))
			{
				animator.SetBool("IsAttacking", true);
				fsm.Transition<CrouchAttack>();
			}

			if(playerUpgrades.HasFireAttack && Input.GetButtonDown("FireAttack"))
			{
				animator.SetBool("IsFire", true);
				animator.SetBool("IsCrouching", false);
				fsm.Transition<FireAttack>();
				return;
			}
		}

		/// <summary>
		/// 	In this class the Exit method disables the crouching collider and enables the standing collider
		/// </summary>
		public override void Exit()
		{
			standingCollider.enabled = true;
			crouchCollider.enabled = false;
		}
	}
}