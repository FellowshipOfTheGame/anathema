using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Fsm
{
    public abstract class WallRobotState : FsmState
    {
        protected Animator animator;
        protected Rigidbody2D myrBody;
        protected SpriteRenderer sRenderer;

        [Tooltip("Mark this bool if you want the robot that rotates in platforms. Remember that if you want it to start going to th left, change the x scale to -1.")]
        [SerializeField] protected bool platformsWalkingBot;

        [Tooltip("Mark this bool if you want the robot that patrol in the walls. It goes in counterclockwise direction by default. If you want chage it, mark the Sprite Renderer Flip X")]
        [SerializeField] protected bool wallsWalkingBot;

        [Tooltip("Speed in which the robot will move.")]
        [SerializeField] protected float speed = 3f;
        //[SerializeField] protected float rayMaxdistance = 1f;

        [Tooltip("attack damage of the wall robot.")]
        [SerializeField] protected int damage;

        //The max distance of the Raycast that gets wall information.
        [SerializeField] protected float rayWallMaxDist = 1f;
        [SerializeField] protected float timer = 0.8f;
        protected SpotControl spotControl;

        [Tooltip("Here goes the amount and the GameObjects which limits the robot patrol area.")]
        [SerializeField] protected Transform[] moveSpots;

        //These are the raycasts, that name the states. If a certain raycast is true, then it will go to the state with the same name.
        protected RaycastHit2D right;
        protected RaycastHit2D left;
        protected RaycastHit2D up;
        protected RaycastHit2D down;

        new void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            myrBody = GetComponent<Rigidbody2D>();
            sRenderer = GetComponent<SpriteRenderer>();
            spotControl = GetComponent<SpotControl>();
        }
    }
}
