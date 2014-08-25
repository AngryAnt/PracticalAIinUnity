using UnityEngine;
using System.Collections;


namespace Examples.Utility
{
	public class Example1 : UtilityBehaviour
	{
		public float m_Health = 1.0f, m_Enemies = 5;
		public AnimationCurve m_PanicCurve = new AnimationCurve (
			new Keyframe (0, 0),
			new Keyframe (5, 0.2f),
			new Keyframe (7, 0.5f),
			new Keyframe (10, 1)
		);
	
	
		public float Health
		{
			get
			{
				return m_Health;
			}
		}
	
	
		public float Enemies
		{
			get
			{
				return m_Enemies;
			}
		}
	
	
		public float Stress
		{
			get
			{
				return Max ((1.0f - m_Health) * 10.0f, m_Enemies);
			}
		}
	
	
		public float Panic
		{
			get
			{
				return m_PanicCurve.Evaluate (Stress);
			}
		}
	
	
		void Start ()
		{
			Debug.Log ("Health: " + Health);
			Debug.Log ("Enemies: " + Enemies);
			Debug.Log ("Stress: " + Stress);
			Debug.Log ("Panic: " + Panic);
	
			Debug.Log ("Flee? " + (Panic > 0.5f ? "Yes, definitely." : "Nah."));
		}
	}
}
