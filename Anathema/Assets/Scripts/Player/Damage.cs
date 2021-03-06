﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Player
{
	public class Damage : Anathema.Fsm.PlayerState
	{
		[HideInInspector] public Vector2 knockbackDirection;

		[SerializeField] private float knockbackForce;
		[SerializeField] private float decelerationRate;
		[SerializeField] private float duration;

		private Health playerHealth;

		private void Start()
		{
			playerHealth = GetComponent<Health>();
		}

		public override void Enter()
		{
			Invoke("EndKnockback", duration);
			animator.SetBool("IsDamaged", true);
			playerHealth = GetComponent<Health>();
			playerHealth.isInvulnerable = true;
			rBody.velocity = knockbackDirection * knockbackForce;
		}

		private void FixedUpdate()
		{
			if(playerHealth.isInvulnerable)
				rBody.velocity *= decelerationRate;
		}

		void EndKnockback()
		{
			rBody.velocity = Vector2.zero;
			fsm.Transition<Idle>();
		}

		public override void Exit()
		{
			Debug.Log("Exiting");
			animator.SetBool("IsDamaged", false);
			animator.SetBool("IsWalking", false);
			animator.SetBool("IsCrouching", false);
			animator.SetBool("IsAttacking", false);
			animator.SetBool("IsFalling", false);
			animator.SetBool("IsRising", false);
			animator.SetBool("IsFireAttacking", false);
			playerHealth.isInvulnerable = false;
		}
	}
}