using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Player
{
	public class FireAttack : Anathema.Fsm.PlayerState
	{
		[Tooltip("The horizontal position of the fire")]
		[SerializeField] private float fireRange;

		[Tooltip("The vertical position of the fire.")]
		[SerializeField] private float fireHeight;

		[Tooltip("Your base attack damage.")]
		[SerializeField] public int fireDamage;

        [SerializeField] private GameObject firePrefab;

        private GameObject currentFireObject;


		/// <summary>
		///		In this class, this method handles the entire attack by casting a box in front of the player that calls for the damage handling in the hit targets.
		/// </summary>
		public override void Enter()
		{
            animator.SetBool("IsFire", true);
			animator.Play("FireAttack", -1, 0);
		}
 
        public void SpawnFire()
        {
            Vector3 firePos = (sRenderer.flipX ? new Vector3(-fireRange, fireHeight) : new Vector3(fireRange, fireHeight));

            currentFireObject = GameObject.Instantiate(firePrefab, this.transform.position + firePos, Quaternion.identity);
			currentFireObject.GetComponent<SpriteRenderer>().flipX = sRenderer.flipX;
			currentFireObject.GetComponent<Fire>().damage = fireDamage;
        }

		/// <summary>
		/// 	This method is called by unity when the attack animation ends, triggering an animation event
		/// </summary>
		public void EndFireAttack()
		{
			fsm.Transition<Idle>();
		}

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawIcon(new Vector3(fireRange, fireHeight) + this.transform.position, "Fire", true);
        }

		public override void Exit()
		{
			animator.SetBool("IsFire", false);
		}

	}
}