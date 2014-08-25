using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Examples.Actions
{
	public class Example2 : MonoBehaviour
	{
		public float m_SightRange = 10.0f;
		public LayerMask m_TargetMask;
		public bool m_Verbose = false;


		List<Example2> m_Targets = new List<Example2> ();


		IEnumerator Start ()
		{
			WaitForSeconds delay = new WaitForSeconds (0.5f);

			while (enabled && Application.isPlaying)
			{
				if (m_Verbose)
				{
					Debug.Log ("Targets: " + m_Targets.Count);
				}

				yield return delay;
			}
		}


		void FixedUpdate ()
		{
			// .75 sets the vision cone to 45 degrees to forward - leading to a 90 degrees field of view
			float dotTarget = 0.75f;

			NavMeshHit hit;

			// Clear old target list and add new hits
			m_Targets.Clear ();
			m_Targets.AddRange (

				// Get all colliders matching the target layer mask, within sight range
				Physics.OverlapSphere (
					transform.position,
					m_SightRange,
					m_TargetMask

				// We are interested in the Example2 components in the transform trees of these colliders
				).Select (
					collider => collider.transform.root.GetComponentInChildren<Example2> ()

				// Sort out bad hits (null and this), check sight cone and accessibility
				).Where (
					other => other != null &&
					other != this &&
					Vector3.Dot ((other.transform.position - transform.position).normalized, transform.forward) > dotTarget &&
					!NavMesh.Raycast (transform.position, other.transform.position, out hit, m_TargetMask)
				)
			);
		}		
	}
}
