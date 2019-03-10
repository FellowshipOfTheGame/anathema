using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Player
{
	public class AirFireAttack : Anathema.Fsm.PlayerState
	{
		[Tooltip("The horizontal position of the fire")]
		[SerializeField] private float fireRange;

		[Tooltip("The vertical position of the fire.")]
		[SerializeField] private float fireHeight;

		[Tooltip("Your base attack damage.")]
		[SerializeField] public int fireDamage;

        [SerializeField] private GameObject firePrefab;

        private GameObject currentFireObject;
		private bool canDoubleJump;
		private bool jumpCorrection;


		/// <summary>
		///		In this class, this method handles the entire attack by casting a box in front of the player that calls for the damage handling in the hit targets.
		/// </summary>
		public override void Enter()
		{
            animator.SetBool("IsFire", true);
			animator.Play("AirFireAttack", -1, 0);
		}
 
        public void SpawnFireMidair()
        {
            Vector3 firePos = (sRenderer.flipX ? new Vector3(-fireRange, fireHeight) : new Vector3(fireRange, fireHeight));

            currentFireObject = GameObject.Instantiate(firePrefab, this.transform.position + firePos, Quaternion.identity);
			currentFireObject.GetComponent<SpriteRenderer>().flipX = sRenderer.flipX;
			currentFireObject.GetComponent<Fire>().damage = fireDamage;
        }

		private void Start()
		{
			PlayerUpgrades playerUpgrades = GetComponent<PlayerUpgrades>();

			if (playerUpgrades) canDoubleJump = playerUpgrades.HasDoubleJump;
			else Debug.LogWarning($"{gameObject.name}: {nameof(Crouch)}: Couldn't find {nameof(PlayerUpgrades)}.");
		}

		private void FixedUpdate()
		{
			JumpFall jumpInfo = GetComponent<JumpFall>();

			if(jumpCorrection && !jumpInfo.hasDoubleJumped && canDoubleJump)
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

		/// <summary>
		/// 	This method is called by unity when the attack animation ends, triggering an animation event
		/// </summary>
		public void EndAirFireAttack()
		{
			animator.SetBool("IsFalling", true);
			fsm.Transition<JumpFall>();
		}

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawIcon(new Vector3(fireRange, fireHeight) + this.transform.position, "Fire", true);
        }

		public override void Exit()
		{
			animator.SetBool("IsFire", false);
			// FIXME: Gambiarra
			jumpCorrection = false;
		}

	}
}