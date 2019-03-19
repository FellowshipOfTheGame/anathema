using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.SpearAngel {
	public class KnockBack : Anathema.Fsm.SpearAngelState {
        [Tooltip("Time duration of knockback.")]
        [SerializeField] private float knockbackDur;

        [Tooltip("The power of the knockback. Affects how far the robot will go.")]
        [SerializeField] private int knockbackPwr;

        /// <summary>
        /// Changes the animation bool, and calls the knockback function with the correct direction as parameter
        /// </summary>
        public override void Enter() {
            animator.SetBool("isTakingDamage", true);

            if (player.transform.position.x - this.transform.position.x > 0) {
                StartCoroutine(KnockBackRobot(knockbackDur, knockbackPwr, Vector2.right));
            } else {
                StartCoroutine(KnockBackRobot(knockbackDur, knockbackPwr, Vector2.left));
            }
        }

        /// <summary>
        /// Adds force on the angel to left or right and stops it after a while
        /// </summary>
        /// <param name="duration">Time until angel's recovery from knockback</param>
        /// <param name="power">Force in which angel is thrown</param>
        /// <param name="direction">Direction of the knockback (right or left)</param>
        /// <returns></returns>
        private IEnumerator KnockBackRobot(float duration, float power, Vector2 direction) {
            rBody.AddForce(new Vector2(direction.x + power, 0f), ForceMode2D.Impulse);
            yield return new WaitForSeconds(duration);
            fsm.Transition<Patrol>();
        }



        // Overrides base Update
		void Update() { }

        /// <summary>
        /// Just changes the animation bool
        /// </summary>
        public override void Exit() { 
            animator.SetBool("isTakingDamage", false);
        }
    }
}