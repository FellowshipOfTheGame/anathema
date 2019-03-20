using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Fsm
{
	/// <summary>
	/// 	Base PlayerState that shares some basic components that every PlayerState uses.
	/// </summary>
	public abstract class PlayerState : FsmState
	{
		protected Animator animator;
		protected Rigidbody2D rBody;
		protected SpriteRenderer sRenderer;

		new void Awake()
		{
			base.Awake();
			animator = GetComponent<Animator>();
			rBody = GetComponent<Rigidbody2D>();
			sRenderer = GetComponent<SpriteRenderer>();
		}
	}

}
