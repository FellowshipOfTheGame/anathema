using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Fsm
{
	/// <summary>
	/// A component implementing a finite state machine.
	/// All states must be added to the same object as the machine component.
	/// </summary>
	public class FiniteStateMachine : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("The current state of the machine")]
		private FsmState _state;

		/// <summary>
		/// The current state of the machine.
		/// Trying to access this value before calling StartMachine will
		/// result in an InvalidOperationException.
		/// </summary>
		/// <value>The current state of the machine.</value>
		public FsmState State
		{
			get
			{
				ErrorIfNotInitialized();
				return _state;
			}
			private set { this._state = value; }
		}

		/// <summary>
		/// Whether or not the machine has been started.
		/// </summary>
		/// <value>true if StartMachine was called, false otherwise.</value>
		public bool Started
		{
			get { return this._state != null; }
		}

		public bool Transitioning
		{
			get;
			private set;
		}

		void Awake()
		{
			this.Transitioning = false;
		}

		void Start()
		{
			if (this._state != null)
				StartMachine(this._state);
		}

		/// <summary>
		/// Starts the machine with a given initial state.
		/// Prior to calling this, all states the machine might transition to
		/// must have been added to the same game object as the machine.
		/// </summary>
		/// <param name="initialState">The initial state of the machine.</param>
		public void StartMachine(FsmState initialState)
		{
			foreach (var state in GetComponents<FsmState>())
				AcknowledgeState(state);
			EnterState(initialState);
		}

		void AcknowledgeState(FsmState state)
		{
			state.enabled = false;
		}

		/// <summary>
		/// Starts the machine with an initial state of type T.
		/// The machine looks for a component of type T in its game object,
		/// and starts itself with it.
		/// </summary>
		/// <typeparam name="T">The type of the initial state.</typeparam>
		public void StartMachine<T>() where T : FsmState
		{
			StartMachine(GetComponent<T>());
		}

		/// <summary>
		/// Transition to a given state.
		/// Calls Exit() on the current state, and Enter() on the next state,
		/// in that order.
		/// </summary>
		/// <param name="nextState">The new state of the machine.</param>
		public void Transition(FsmState nextState)
		{
			// Debug.Log("Changing from " + _state + " to " + nextState);
			ErrorIfNotInitialized();
			ErrorIfMidTransition();
			this.Transitioning = true;
			ExitState();
			this.Transitioning = false;
			EnterState(nextState);
		}

		/// <summary>
		/// Transition to a state of the given type.
		/// The machine looks for a component of type T in its game object, and
		/// transitions to it.
		/// Calls Exit() on the current state, and Enter() on the next state,
		/// in that order.
		/// </summary>
		/// <typeparam name="T">The type of the state to transition to.</typeparam>
		public void Transition<T>() where T : FsmState
		{
			Transition(GetComponent<T>());
		}

		void EnterState(FsmState nextState)
		{
			ErrorIfNullArgument(nextState, nameof(nextState));
			this.State = nextState;
			this.State.enabled = true;
			this.State.Enter();
		}

		void ExitState()
		{
			if (this.State != null)
			{
				this.State.Exit();
				this.State.enabled = false;
			}
			this.State = null;
		}

		void ErrorIfNotInitialized()
		{
			if (!this.Started)
				throw new InvalidOperationException("FSM was not initialized");
		}

		void ErrorIfNullArgument(object value, string paramName)
		{
			if (value == null)
				throw new ArgumentNullException(paramName, "Unknown state");
		}

		void ErrorIfMidTransition()
		{
			if (this.Transitioning)
				throw new InvalidOperationException(
					"Cannot initiate a transition while exiting a state");
		}
	}
}