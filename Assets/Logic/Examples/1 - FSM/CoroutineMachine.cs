using UnityEngine;
using System.Collections;
using System;


namespace Examples.FSM
{
	public delegate IEnumerator StateRoutine ();
	public delegate IEnumerator TransitionRoutine (StateRoutine from, StateRoutine to);


	public class TransitionTo
	{
		public StateRoutine Target
		{ get; protected set; }


		public TransitionRoutine Transition
		{ get; protected set; }


		public TransitionTo (StateRoutine newState, TransitionRoutine transition)
		{
			Target = newState;
			Transition = transition;
		}
	}


	public abstract class CoroutineMachine : MonoBehaviour
	{
		protected abstract StateRoutine InitialState
		{ get; }


		StateRoutine m_CurrentState;


		IEnumerator Start ()
		{
			m_CurrentState = InitialState;

			if (m_CurrentState == null)
			{
				throw new ArgumentException ("Initial state is null");
			}

			while (m_CurrentState != null)
			{
				yield return StartCoroutine (Wrap (m_CurrentState ()));
			}
		}


		IEnumerator Wrap (IEnumerator coroutine)
		{
			while (true)
			{
				if (!coroutine.MoveNext ())
				{
					Debug.LogError ("Broke out of the current state. Will resume.");
					yield break;
				}

				TransitionTo transitionTo = coroutine.Current as TransitionTo;

				if (transitionTo != null)
				{
					yield return StartCoroutine (transitionTo.Transition (m_CurrentState, transitionTo.Target));
					m_CurrentState = transitionTo.Target;
					yield break;
				}

				yield return coroutine.Current;
			}
		}
	}
}
