using UnityEngine;
using System;


namespace Examples.FSM
{
	public class Example1 : EnumDelegateBehaviour
	{
		enum StateType
		{
			Start,
			Running,
			End
		};


		protected override Type InitializeStateType ()
		{
			return typeof (StateType);
		}


		int m_RunCount = 0;


		void UpdateStartState ()
		{
			Debug.Log ("Start state.");
			CurrentState = (int)StateType.Running;
		}


		void UpdateRunningState ()
		{
			Debug.Log ("Running state.");
			if (++m_RunCount >= 3)
			{
				m_RunCount = 0;
				CurrentState = (int)StateType.End;
			}
		}


		void UpdateEndState ()
		{
			Debug.Log ("End state.");
			enabled = false;
		}
	}
}
