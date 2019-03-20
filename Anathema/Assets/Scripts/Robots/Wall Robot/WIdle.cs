using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.WallRobot
{
    public class WIdle : Anathema.Fsm.WallRobotState
    {
        public override void Enter() { }

        /// <summary>
        /// In this class, Update is used to switch the state.
        /// </summary>
        void FixedUpdate()
        {

            animator.SetBool("isWalking", true);

            right = RightRaycast();
            left = LeftRaycast();
            up = UpRaycast();
            down = DownRaycast();

            if (down == true)
            {
                fsm.Transition<Down>();
            }
            else if (up == true)
            {
                fsm.Transition<Up>();
            }
            else if (right == true)
            {
                fsm.Transition<Right>();
            }
            else if (left == true)
            {
                fsm.Transition<Left>();
            }

        }

        private RaycastHit2D RightRaycast()
        {
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y);

            Debug.DrawRay(startPos, Vector2.right, Color.red);
            return Physics2D.Raycast(startPos, Vector2.right, rayWallMaxDist, LayerMask.GetMask("Wall", "Ground"));
        }

        private RaycastHit2D LeftRaycast()
        {
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y);

            Debug.DrawRay(startPos, Vector2.left, Color.blue);
            return Physics2D.Raycast(startPos, Vector2.left, rayWallMaxDist, LayerMask.GetMask("Wall", "Ground"));
        }
        private RaycastHit2D UpRaycast()
        {
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y);

            Debug.DrawRay(startPos, Vector2.up, Color.green);
            return Physics2D.Raycast(startPos, Vector2.up, rayWallMaxDist, LayerMask.GetMask("Wall", "Ground"));
        }
        private RaycastHit2D DownRaycast()
        {
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y);

            Debug.DrawRay(startPos, Vector2.down, Color.black);
            return Physics2D.Raycast(startPos, Vector2.down, rayWallMaxDist, LayerMask.GetMask("Ground", "Wall"));
        }
        public override void Exit() { }
    }
}