using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.SpearAngel {
	public class KnockBack : Anathema.Fsm.SpearAngelState {
        [Tooltip("Time duration of knockback.")]
        [SerializeField] private float knockbackDur;

        [Tooltip("The power of the knockback. Affects how far the robot will go.")]
        [SerializeField] private int knockbackPwr;
        public override void Enter() {
            animator.SetBool("isTakingDamage", true);

            if (player.transform.position.x - this.transform.position.x > 0) {
                StartCoroutine(KnockBackRobot(knockbackDur, -knockbackPwr, Vector2.left));
            } else {
                StartCoroutine(KnockBackRobot(knockbackDur, knockbackPwr, Vector2.left));
            }
        }

        private IEnumerator KnockBackRobot(float duration, float power, Vector2 direction) {
            // float timer = 0;

            // while (duration > timer) {
            //     Debug.LogWarning("KnockBacking");
            //     timer += Time.deltaTime;
            //     rBody.AddForce(new Vector2(direction.x + power, 0f));
            // }
            rBody.AddForce(new Vector2(direction.x + power, 0f), ForceMode2D.Impulse);
            yield return new WaitForSeconds(duration);
            fsm.Transition<Daze>();
        }



        // Overrides base Update
		void Update() {

		}
        public override void Exit() { 
            animator.SetBool("isTakingDamage", false);
        }
    }
}