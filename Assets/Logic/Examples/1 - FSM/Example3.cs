using UnityEngine;
using System.Collections;


namespace Examples.FSM
{
	public class Example3 : CoroutineMachine
	{
		WaitForSeconds m_FramerateWait = new WaitForSeconds (1.0f / 20.0f);


		protected override StateRoutine InitialState
		{
			get
			{
				return StartState;
			}
		}


		int m_RunCount = 0;


		IEnumerator StartState ()
		{
			Debug.Log ("Start state.");

			yield return new TransitionTo (RunningState, DefaultTransition);
		}


		IEnumerator RunningState ()
		{
			while (++m_RunCount <= 3)
			{
				Debug.Log ("Running state.");
				yield return m_FramerateWait;
			}

			yield return new TransitionTo (EndState, DefaultTransition);
		}


		IEnumerator EndState ()
		{
			Debug.Log ("End state.");
			yield return new TransitionTo (null, DefaultTransition);
		}


		IEnumerator DefaultTransition (StateRoutine from, StateRoutine to)
		{
			Debug.Log (string.Format ("Transitioning from {0} to {1}", from.Method.Name, to == null ? "null" : to.Method.Name));
			if (from == RunningState)
			{
				m_RunCount = 0;
			}
			yield return m_FramerateWait;
		}
	}
}
