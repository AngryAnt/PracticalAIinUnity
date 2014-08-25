using UnityEngine;
using System.Collections;


namespace Examples.Actions
{
	[RequireComponent (typeof (NavMeshAgent))]
	public class Example1 : MonoBehaviour
	{
		public Transform[] m_PatrolPoints;


		bool m_ActionSuccess = true;
		int m_PatrolIndex = -1;


		Vector3 PatrolPointPosition
		{
			get
			{
				return m_PatrolPoints[m_PatrolIndex].position;
			}
		}


		IEnumerator Start ()
		{
			// Simple behaviour loop
			while (enabled && Application.isPlaying)
			{
				// Pick a patrol point
				yield return StartCoroutine (PickPatrolPointAction ());
				if (!m_ActionSuccess)
				{
					Debug.LogError ("PickPatrolPoint failed!");
					yield break;
				}

				// Go there
				yield return StartCoroutine (GoToAction (PatrolPointPosition));
				if (!m_ActionSuccess)
				{
					Debug.LogError ("GoTo failed!");
					yield break;
				}

				// Count to three
				yield return StartCoroutine (CountAction (3));
			}
		}


		IEnumerator PickPatrolPointAction ()
		{
			// Init
			if (m_PatrolPoints.Length < 2)
			{
				Debug.LogError ("This behaviour requires at least two patrol points");
				m_ActionSuccess = false;
				yield break;
			}

			// Run
			m_PatrolIndex = ++m_PatrolIndex % m_PatrolPoints.Length;

			m_ActionSuccess = true;
		}


		IEnumerator GoToAction (Vector3 destination)
		{
			// Init
			const float kArrivalDistance = 2;

			Debug.Log ("Going to " + destination);

			NavMeshHit hit;
			m_ActionSuccess = NavMesh.SamplePosition (destination, out hit, kArrivalDistance, ~0);
			if (m_ActionSuccess)
			{
				destination = hit.position;
			}
			else
			{
				yield break;
			}

			// Run
			NavMeshAgent agent = GetComponent<NavMeshAgent> ();
			agent.destination = destination;

			while (agent.remainingDistance > kArrivalDistance)
			{
				yield return null;
			}

			// Reset
			agent.Stop ();
		}
		

		IEnumerator CountAction (int count)
		{
			// Init
			m_ActionSuccess = true;
			WaitForSeconds delay = new WaitForSeconds (1.0f);

			// Run
			for (int current = 1; current <= count; ++current)
			{
				yield return delay;
				Debug.Log (current);
			}
		}
	}
}
