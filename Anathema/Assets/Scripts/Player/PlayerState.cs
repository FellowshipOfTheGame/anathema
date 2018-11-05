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
			UnityEngine.Experimental.Input.InputSystem.RegisterControlProcessor(typeof(Input.DeadzoneFloatProcessor), "DeadzoneFloat");
			UnityEngine.Experimental.Input.InputSystem.RegisterInteraction(typeof(Input.KeepPressedInteraction), "KeepPressed");
			UnityEngine.Experimental.Input.InputSystem.RegisterInteraction(typeof(Input.StickFloatInteraction), "StickFloat");
		}
	}

}
