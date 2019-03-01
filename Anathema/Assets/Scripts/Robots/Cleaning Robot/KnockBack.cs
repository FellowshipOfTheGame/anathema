using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Anathema.ChasingRobot
{
    public class KnockBack : Anathema.Fsm.CleaningRobotState
    {
        [Tooltip("Time duration of knockback.")]
        [SerializeField] private float knockbackDur;

        [Tooltip("The power of the knockback. Affects how far the robot will go.")]
        [SerializeField] private int knockbackPwr;
        public override void Enter()
        {
            if (sRenderer.flipX == false)
            {
                StartCoroutine(KnockBackRobot(knockbackDur, -knockbackPwr, Vector2.left));
            }
            else if (sRenderer.flipX == true)
            {
                StartCoroutine(KnockBackRobot(knockbackDur, knockbackPwr, Vector2.left));
            }
            fsm.Transition<Chase>();
        }

        private IEnumerator KnockBackRobot(float duration, float power, Vector2 direction)
        {
            float timer = 0;

            while (duration > timer)
            {
                timer += Time.deltaTime;
                myrBody.AddForce(new Vector2(direction.x + power, transform.position.y));
            }
            yield return 0;
        }

        public override void Exit() { }
    }
}