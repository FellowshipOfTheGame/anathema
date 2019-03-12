using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Player
{
	public class AirAttack : Anathema.Fsm.PlayerState
	{
        [Tooltip("The horizontal attack range in unity units.")]
		[SerializeField] private float attackRange;

		[Tooltip("The vertical range of your attack.")]
		[SerializeField] private float attackHeight;

		[Tooltip("Higher values mean your attack's hitbox is placed higher on the y axis.")]
		[SerializeField] private float attackHeightOffset;

		[Tooltip("Your base attack damage.")]
		[SerializeField] public int baseDamage;

		private PlayerUpgrades playerUpgrades;

		// FIXME: Gambiarra
		private bool jumpCorrection;

        /// <summary>
		///		In this class, this method handles the entire attack by casting a box in front of the player that calls for the damage handling in the hit targets.
		/// </summary>
		public override void Enter()
		{
			animator.Play("AirAttack", -1, 0);
			
			foreach(var hit in Physics2D.BoxCastAll(this.transform.position + attackHeightOffset * Vector3.up,
				new Vector2(attackRange, attackHeight), 0f,
				sRenderer.flipX ? Vector2.left : Vector2.right, 1f,
				LayerMask.GetMask("Enemy", "Breakable")))
			{
				hit.transform.GetComponent<Health>().Damage(baseDamage, sRenderer.flipX ? Vector2.left : Vector2.right, Health.DamageType.EnemyAttack);
				Debug.Log("Deu damage");
			}

			// Debug Rays for better visualization
			Debug.DrawRay(this.transform.position + attackHeightOffset * Vector3.up + Vector3.up * attackHeight/2, sRenderer.flipX ? Vector2.left * attackRange : Vector2.right * attackRange , Color.cyan, 2f);
			Debug.DrawRay(this.transform.position + attackHeightOffset * Vector3.up + Vector3.down * attackHeight/2, sRenderer.flipX ? Vector2.left * attackRange : Vector2.right * attackRange, Color.cyan, 2f);
		}

		private void Start()
		{
			playerUpgrades = GetComponent<PlayerUpgrades>();
			if (!playerUpgrades) Debug.LogError($"{gameObject.name}: {nameof(Crouch)}: Couldn't find {nameof(PlayerUpgrades)}.");
		}

		// FIXME: Gambiarra
		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.Space))
				jumpCorrection = true;
		}

		private void FixedUpdate()
		{
			JumpFall jumpInfo = GetComponent<JumpFall>();

			if(jumpCorrection && !jumpInfo.hasDoubleJumped && playerUpgrades.HasDoubleJump)
			{
				jumpCorrection = false;
				animator.SetBool("IsRising", true);
				animator.SetBool("IsFalling", false);
				jumpInfo.hasDoubleJumped = true;
				jumpInfo.hasAttacked = false;
				jumpInfo.hasFireAttacked = false;
				rBody.velocity = new Vector2(rBody.velocity.x, 0f);
				fsm.Transition<JumpRise>();
				return;
			}
		}

		public override void Exit()
		{
			animator.SetBool("IsAttacking", false);
			// FIXME: Gambiarra
			jumpCorrection = false;
		}

        public void EndAirAttack()
		{
            animator.SetBool("IsFalling", true);
			fsm.Transition<JumpFall>();
		}

    }
}