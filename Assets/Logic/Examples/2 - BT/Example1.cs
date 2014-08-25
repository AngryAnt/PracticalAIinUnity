using UnityEngine;
using System.Collections;

namespace Examples.BT
{
	public class Example1 : CoroutineTree
	{
		public override float Frequency
		{
			get
			{
				return 1.0f;
			}
		}


		void Start ()
		{
			StartTree (ExecutionMode.SingleRun);
		}


		protected override IEnumerator Root ()
		{
			return Sequence (
				LogAction ("A"),
				Sequence (
					LogAction ("AA"),
					LogAction ("AB"),
					LogAction ("AC")
				),
				CountAction (3),
				Selector (
					PassAction (Failure),
					PassAction (Success),
					PassAction (Failure)
				)
			);
		}


		IEnumerator LogAction (string message)
		{
			yield return null; // No init

			Debug.Log (message);

			yield return Success;
		}


		IEnumerator PassAction (ResultType result)
		{
			yield return null; // No init

			Debug.Log (result);

			yield return result;
		}


		IEnumerator CountAction (int count)
		{
			// Init

			Debug.Log ("Init counter");

			int current = 0;
			NodeData data = new NodeData ();

			yield return data;

			// Run
			
			while (!data.ShouldReset && ++current <= count)
			{
				Debug.Log (current);

				yield return Running;
			}

			yield return Success;
		}
	}
}
