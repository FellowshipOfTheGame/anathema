using System.Collections;
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
		}

		private void FixedUpdate()
		{
			if(playerHealth.isInvulnerable)
				rBody.velocity *= decelerationRate;
			else
			{
				animator.SetBool("IsDamaged", true);
				playerHealth = GetComponent<Health>();
				playerHealth.isInvulnerable = true;
				rBody.velocity = knockbackDirection * knockbackForce;
			}
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
			playerHealth.isInvulnerable = false;
		}
	}
}