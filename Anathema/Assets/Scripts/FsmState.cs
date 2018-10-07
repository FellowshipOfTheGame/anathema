using System;
using UnityEngine;

namespace Anathema.Fsm
{
	/// <summary>
	/// A state for a Finite State Machine.
	/// </summary>
	public abstract class FsmState : MonoBehaviour
	{
		/// <summary>
		/// A reference to this state's FSM.
		/// </summary>
		protected FiniteStateMachine fsm;

		// Protected to avoid overrides or overshadowing
		protected void Awake()
		{
			this.fsm = GetComponent<FiniteStateMachine>();
			ErrorIfNoFsmAttached();
			this.enabled = false;
		}

		// Protected to avoid overrides or overshadowing
		protected void Reset()
		{
			this.enabled = false;
		}

		/// <summary>
		/// Called when the FSM transitions into this state.
		/// </summary>
		public abstract void Enter();

		/// <summary>
		/// Called when the FSM transitions out of this state.
		/// </summary>
		public abstract void Exit();

		void ErrorIfNoFsmAttached()
		{
			if (this.fsm == null)
				throw new InvalidOperationException(
					"Added an FSM script to an object without an FSM");
		}
	}
}