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

        /// <summary>
		///		In this class, this method handles the entire attack by casting a box in front of the player that calls for the damage handling in the hit targets.
		/// </summary>
		public override void Enter()
		{
			animator.Play("AirAttack", -1, 0);
			
            rBody.velocity = Vector3.zero;
			foreach(var hit in Physics2D.BoxCastAll(this.transform.position + attackHeightOffset * Vector3.up,
				new Vector2(attackRange, attackHeight), 0f,
				sRenderer.flipX ? Vector2.left : Vector2.right, 1f,
				LayerMask.GetMask("Enemy", "Breakable")))
			{
				hit.transform.GetComponent<Health>().Damage(baseDamage, -hit.normal, Health.DamageType.EnemyAttack);
				Debug.Log("Deu damage");
			}

			// Debug Rays for better visualization
			Debug.DrawRay(this.transform.position + attackHeightOffset * Vector3.up + Vector3.up * attackHeight/2, sRenderer.flipX ? Vector2.left * attackRange : Vector2.right * attackRange , Color.cyan, 2f);
			Debug.DrawRay(this.transform.position + attackHeightOffset * Vector3.up + Vector3.down * attackHeight/2, sRenderer.flipX ? Vector2.left * attackRange : Vector2.right * attackRange, Color.cyan, 2f);
		}

		public override void Exit()
		{
			animator.SetBool("IsAttacking", false);
		}

        public void EndAirAttack()
		{
            animator.SetBool("IsFalling", true);
			fsm.Transition<JumpFall>();
		}

    }
}