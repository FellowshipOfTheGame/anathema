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

		[Tooltip("Mark this bool if you want the robot that patrol in the walls. Remember that if you want it to start going to th left, change the x scale to -1.")]
		[SerializeField] protected bool wallsWalkingBot;	

		[Tooltip("Speed in which the robot will move.")]
		[SerializeField] protected float speed = 3f;
	 	//[SerializeField] protected float rayMaxdistance = 1f;

		//The max distance of the Raycast that gets ground information.
		protected float rayGroundMaxDist = 1f;

		//The max distance of the Raycast that gets wall information.
		protected float rayWallMaxDist = 0.5f;

		//Distance from the robot, that affects where the raycast will start
		protected float origindir = 0.1f;

		//Raycast that detects the player
		protected RaycastHit2D hit;

		//Stores the direction of the Raycast that gets wall information
	 	protected Vector2 direction;

		protected SpotControl spotControl;

		//The current scale of the robot, if it's (> 0) or (< 0)
		protected Vector2 currentScale;

		[Tooltip("Here goes the amount and the GameObjects which limits the robot patrol area.")]
		[SerializeField] protected Transform[] moveSpots;

		[Tooltip("Here goes the GameObject where the Raycast that gets ground information will start.")]
		[SerializeField] protected Transform groundDetection;

		//The direction os the Raycast that gets ground information
		protected Vector2 groundDetectionDir;

		//Raycast that gets the wall information
		protected RaycastHit2D wallInfo;

		new void Awake()
		{
			base.Awake();
			animator = GetComponent<Animator>();
			myrBody = GetComponent<Rigidbody2D>();
			sRenderer = GetComponent<SpriteRenderer>();
		}
	}
}
