using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Examples.BT
{
	public class CoroutineTree : MonoBehaviour
	{
		public enum ResultType // TODO: Consider doing a class here in stead - for better comparison than Success.Equals (obj)
		{
			Success,
			Failure,
			Running
		};


		public enum ExecutionMode
		{
			Continuous,
			SingleRun
		};


		public const ResultType
			Success = ResultType.Success,
			Failure = ResultType.Failure,
			Running = ResultType.Running;


		NodeData m_RootData = null;


		public virtual float Frequency { get { return 20.0f; } }


		public void StartTree (ExecutionMode mode = ExecutionMode.Continuous)
		{
			StartCoroutine (Run (mode));
		}


		protected IEnumerator Run (ExecutionMode mode)
		{
			WaitForSeconds frameDelay = new WaitForSeconds (1.0f / Frequency);

			do
			{
				// Start running the tree root - grabbing its node data yielded on the init frame
				IEnumerator rootRoutine = Root ();
				rootRoutine.MoveNext ();
				m_RootData = rootRoutine.Current as NodeData;

				// Keep running until we're disabled
				while (Application.isPlaying && enabled)
				{
					// Break out if the root was reset
					if (m_RootData != null && m_RootData.ShouldReset)
					{
						break;
					}

					// Break out if the tree breaks or completes with success or failure
					if (!rootRoutine.MoveNext () || Success.Equals (rootRoutine.Current) || Failure.Equals (rootRoutine.Current))
					{
						break;
					}

					yield return frameDelay;
				}

				// Reset the root
				if (m_RootData != null)
				{
					m_RootData.Reset ();
					rootRoutine.MoveNext ();
				}
			}
			while (mode == ExecutionMode.Continuous && Application.isPlaying && enabled);
		}


		public void ResetTree ()
		{
			m_RootData.Reset ();
		}


		protected virtual IEnumerator Root ()
		{
			yield return Failure;
		}


		protected IEnumerator Sequence (params IEnumerator[] children)
		{
			SequenceData sequence = new SequenceData ();

			return Control (
				sequence,
				() => {
					if (sequence.Index >= children.Length)
					{
						return Success;
					}
	
					IEnumerator child = children[sequence.Index];
	
					// Fail on break or explicit failure
					if (!child.MoveNext () || Failure.Equals (child.Current))
					{
						return Failure;
					}
	
					// Move to next on success
					if (Success.Equals (child.Current))
					{
						sequence.Index = sequence.Index + 1;
					}
	
					return Running;
				},
				children
			);
		}


		protected IEnumerator Selector (params IEnumerator[] children)
		{
			SelectorData selector = new SelectorData ();

			return Control (
				selector,
				() => {
					if (selector.Index >= children.Length)
					{
						return Success;
					}
	
					IEnumerator child = children[selector.Index];
	
					// Move to next on break or explicit failure
					if (!child.MoveNext () || Failure.Equals (child.Current))
					{
						selector.Index = selector.Index + 1;
					}
	
					// Success on success
					if (Success.Equals (child.Current))
					{
						return Success;
					}
	
					return Running;
				},
				children
			);
		}


		IEnumerator Control (NodeData data, System.Func<ResultType> logic, IEnumerator[] children)
		{
			// Collect node data from children (for use in reset)
			Dictionary<IEnumerator, NodeData> childData = new Dictionary<IEnumerator, NodeData> ();
			foreach (IEnumerator child in children)
			{
				child.MoveNext ();
				childData[child] = child.Current as NodeData;
			}

			// Yield own node data
			yield return data;

			// Fail on no children
			if (children.Length < 1)
			{
				yield return Failure;
			}
			
			ResultType result = Success;

			// Main flow
			while (!data.ShouldReset)
			{
				result = logic ();

				if (Success.Equals (result) || Failure.Equals (result))
				{
					break;
				}

				yield return Running;
			}

			// Reset children
			foreach (KeyValuePair<IEnumerator, NodeData> pair in childData)
			{
				if (pair.Value == null)
				{
					continue;
				}

				pair.Value.Reset ();
				pair.Key.MoveNext ();
			}

			yield return result;
		}


		public class NodeData
		{
			public bool ShouldReset { get; protected set; }

			public virtual void Reset ()
			{
				ShouldReset = true;
			}
		}


		class SequenceData : NodeData
		{
			public int Index { get; set; }

			public SequenceData ()
			{
				Index = 0;
			}
		}


		class SelectorData : NodeData
		{
			public int Index { get; set; }

			public SelectorData ()
			{
				Index = 0;
			}
		}
	}
}
