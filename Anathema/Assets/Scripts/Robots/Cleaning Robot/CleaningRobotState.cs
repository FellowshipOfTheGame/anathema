using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Fsm
{
	public abstract class CleaningRobotState : FsmState
	{
		protected Animator animator;
		protected Rigidbody2D myrBody;
		protected SpriteRenderer sRenderer;

		[Tooltip("The max distance that the robot will detect the player.")]
		[SerializeField] protected float raycastMaxDist = 20f;

		//Distance from the robot, that affects where the raycast will start
		protected float origindir = 0.01f;

		//Distance between the player and the robot	
		protected Vector2 playerDist;

		//The current scale of the robot, if it's (> 0) or (< 0)
		protected Vector2 currentScale;

		//Transform to get the position of the player
		protected Transform player;

		//Raycast that detects the player
		protected RaycastHit2D hit;
	
		new void Awake()
		{
			base.Awake();
		
			animator = GetComponent<Animator>();
			myrBody = GetComponent<Rigidbody2D>();
			sRenderer = GetComponent<SpriteRenderer>();
		}
	}
}