using UnityEngine;
using System;
using System.Collections;


namespace Examples.FSM
{
	public abstract class EnumDelegateBehaviour : MonoBehaviour
	{
		protected virtual float Framerate
		{
			get
			{
				return 20.0f;
			}
		}
		protected abstract Type InitializeStateType ();


		protected int CurrentState
		{
			get
			{
				return m_StateMachine == null ? -1 : m_StateMachine.CurrentState;
			}
			set
			{
				if (m_StateMachine != null)
				{
					m_StateMachine.CurrentState = value;
				}
			}
		}


		EnumDelegate m_StateMachine;


		IEnumerator Start ()
		{
			m_StateMachine = new EnumDelegate (this, InitializeStateType ());

			while (enabled && Application.isPlaying)
			{
				m_StateMachine.Update ();
				yield return new WaitForSeconds (1.0f / Framerate);
			}
		}	
	}
}
