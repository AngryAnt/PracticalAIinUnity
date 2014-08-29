using UnityEngine;
using System.Collections;
using UnityAssets;


public class Control : MonoBehaviour
{
	public const string kLastLevelPref = "Loaded level", kPreviousButtonName = "Previous", kNextButtonName = "Next";

	
	public GameObject[] build;
	public bool autoBuildFirst = false;


	int buildIndex = -1;


	public int BuildIndex
	{
		get
		{
			return buildIndex;
		}
	}


	void Awake ()
	{
#if !UNITY_ANDROID
		int targetLevel = PlayerPrefs.GetInt (kLastLevelPref, -1);
		if (targetLevel != -1 && targetLevel != Application.loadedLevel)
		{
			LoadLevel (targetLevel);
			return;
		}
#endif

		foreach (GameObject item in build)
		{
			item.SetActive (false);
		}
	}


	void Start ()
	{
		if (autoBuildFirst && build.Length > 0)
		{
			Next ();
		}
	}


	public void Previous ()
	{
		if (Application.loadedLevel > 0)
		{
			LoadLevel (Application.loadedLevel - 1);
		}
#if UNITY_ANDROID
		else
		{
			LoadLevel (Application.levelCount - 1);
		}
#endif
	}


	public void Next ()
	{
		if (build.Length > ++buildIndex)
		{
			build[buildIndex].SetActive (true);
			return;
		}

		if (Application.loadedLevel < Application.levelCount - 1)
		{
			LoadLevel (Application.loadedLevel + 1);
		}
	}


	public void Skip ()
	{
		LoadLevel (Application.loadedLevel + 1);
	}


	void LoadLevel (int index)
	{
		PlayerPrefs.SetInt (kLastLevelPref, index);
		Application.LoadLevel (index);
	}


	void OnGesture (Gesture gesture)
	{
		switch (gesture.type)
		{
			case GestureType.Swipe:
				if (Vector2.Dot (gesture.direction, new Vector2 (1.0f, 0.0f)) > 0.8f)
				{
					Previous ();
				}
				else if (Vector2.Dot (gesture.direction, new Vector2 (-1.0f, 0.0f)) > 0.8f)
				{
					Next ();
				}
			break;
		}
	}


#if UNITY_ANDROID
	const float kControllerPressDelay = 1.0f;
	float m_LastControllerPress = 0;
#endif


	void Update ()
	{
#if UNITY_ANDROID
		if (Time.time - m_LastControllerPress > kControllerPressDelay)
		{
			if (
				Input.GetAxis ("Joystick Axis 5") < -0.5f ||
				Input.GetAxis ("Joystick Axis X") < -0.5f ||
				Input.GetAxis ("Joystick Axis 3") < -0.5f ||
				Input.GetKey (KeyCode.JoystickButton4)
			)
			{
				m_LastControllerPress = Time.time;
				Previous ();
			}
			else if (
				Input.GetAxis ("Joystick Axis 5") > 0.5f ||
				Input.GetAxis ("Joystick Axis X") > 0.5f ||
				Input.GetAxis ("Joystick Axis 3") > 0.5f ||
				Input.GetKey (KeyCode.JoystickButton0)
			)
			{
				m_LastControllerPress = Time.time;
				Next ();
			}
			else if (Input.GetKey (KeyCode.JoystickButton5))
			{
				m_LastControllerPress = Time.time;
				Skip ();
			}
		}
#else
		if (Input.GetButtonDown (kPreviousButtonName))
		{
			Previous ();
		}
		else if (Input.GetButtonDown (kNextButtonName))
		{
			if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.LeftShift))
			{
				Skip ();
			}
			else
			{
				Next ();
			}
		}
#endif
	}
}
