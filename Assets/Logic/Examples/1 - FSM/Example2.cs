using UnityEngine;
using System.Collections;


namespace Examples.FSM
{
	public class Example2 : MonoBehaviour
	{
		delegate IEnumerator StateRoutine ();


		StateRoutine m_CurrentRoutine;
		int m_RunCount = 0;
		WaitForSeconds m_FramerateWait = new WaitForSeconds (1.0f / 20.0f);


		IEnumerator Start ()
		{
			m_CurrentRoutine = StartState;

			while (enabled && Application.isPlaying && m_CurrentRoutine != null)
			{
				yield return StartCoroutine (m_CurrentRoutine ());
			}
		}


		IEnumerator StartState ()
		{
			Debug.Log ("Start state.");
			yield return m_FramerateWait;

			m_CurrentRoutine = RunningState;
		}


		IEnumerator RunningState ()
		{
			while (++m_RunCount <= 3)
			{
				Debug.Log ("Running state.");
				yield return m_FramerateWait;
			}

			m_RunCount = 0;
			m_CurrentRoutine = EndState;
		}


		IEnumerator EndState ()
		{
			Debug.Log ("End state.");
			yield return m_FramerateWait;

			enabled = false;
		}
	}
}
