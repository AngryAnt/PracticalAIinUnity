using UnityEngine;
using System.Collections;


public interface IRunnable
{
	void Run ();
}


public class RunOnStart : MonoBehaviour
{
	public Object m_Runnable;
	

	void Start ()
	{
		IRunnable runnable = m_Runnable as IRunnable;

		if (runnable == null)
		{
			Debug.LogError ("Invalid runnable provided.");
		}

		runnable.Run ();
	}
}
