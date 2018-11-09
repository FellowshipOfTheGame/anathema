using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Fsm
{
	public abstract class DroneState : FsmState
	{
		protected Animator animator;
		protected Rigidbody2D myrBody;
		protected SpriteRenderer sRenderer;

		new void Awake()
		{
			base.Awake();
		
			animator = GetComponent<Animator>();
			myrBody = GetComponent<Rigidbody2D>();
			sRenderer = GetComponent<SpriteRenderer>();
		}
	}
}