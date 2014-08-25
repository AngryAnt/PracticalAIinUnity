using System;
using System.Collections.Generic;


namespace Examples.FSM
{
	public class EnumDelegate
	{
		public int CurrentState
		{
			get
			{
				return m_CurrentState;
			}
			set
			{
				if (m_CurrentState == value)
				{
					return;
				}
				
				if (m_StateTypeValues == null)
				{
					throw new ApplicationException ("Tried to set current state before initialization");
				}
				
				if (Array.IndexOf (m_StateTypeValues, value) < 0)
				{
					throw new ArgumentException ("Given value is invalid for the given StateType");
				}

				m_CurrentState = value;
			}
		}


		Type m_StateType;
		int[] m_StateTypeValues;
		Dictionary<int, Action> m_StateHandlers;
		int m_CurrentState;


		public EnumDelegate (object handler, Type stateType, Type delegateType = null)
		{
			m_StateType = stateType;

			if (m_StateType == null || !m_StateType.IsEnum || Enum.GetUnderlyingType (m_StateType) != typeof (int))
			{
				throw new ArgumentException ("StateType must be a valid enum with int as backing type");
			}

			m_StateTypeValues = (int[])Enum.GetValues (m_StateType);

			if (m_StateTypeValues.Length < 1)
			{
				throw new ArgumentException ("StateType must define at least one enum value");
			}

			m_CurrentState = m_StateTypeValues[0];
			m_StateHandlers = Utilities.FSM.GetStateHandlers (handler, m_StateType, "Update", "State");
		}


		public void Update ()
		{
			m_StateHandlers[m_CurrentState] ();
		}
	}
}
