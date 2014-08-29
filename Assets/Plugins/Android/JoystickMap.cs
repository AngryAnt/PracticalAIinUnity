using UnityEngine;
using System.Collections;


public class JoystickMap : MonoBehaviour
{
	void OnGUI ()
	{
		LabelLabel ("Frame", Time.frameCount.ToString ());

		GUILayout.Label ("General buttons");
		ButtonMap ("A: Joystick Button 0", Input.GetKey (KeyCode.JoystickButton0));
		ButtonMap ("B: Joystick Button 1", Input.GetKey (KeyCode.JoystickButton1));
		ButtonMap ("X: Joystick Button 2", Input.GetKey (KeyCode.JoystickButton2));
		ButtonMap ("Y: Joystick Button 3", Input.GetKey (KeyCode.JoystickButton3));
		ButtonMap ("L1: Joystick Button 4", Input.GetKey (KeyCode.JoystickButton4));
		ButtonMap ("R1: Joystick Button 5", Input.GetKey (KeyCode.JoystickButton5));
		ButtonMap ("L3: Joystick Button 8", Input.GetKey (KeyCode.JoystickButton8));
		ButtonMap ("R3: Joystick Button 9", Input.GetKey (KeyCode.JoystickButton9));

		GUILayout.Label ("Control buttons");
		ButtonMap ("Back", Input.GetKey (KeyCode.Escape));
		ButtonMap ("Menu", Input.GetKey (KeyCode.Menu));

		GUILayout.Label ("Axes");
		AxisMap ("L X: Joystick Axis X", Input.GetAxis ("Joystick Axis X"));
		AxisMap ("L Y: Joystick Axis Y", Input.GetAxis ("Joystick Axis Y"));
		AxisMap ("R X: Joystick Axis 3", Input.GetAxis ("Joystick Axis 3"));
		AxisMap ("R Y: Joystick Axis 4", Input.GetAxis ("Joystick Axis 4"));
		AxisMap ("DPAD H: Joystick Axis 5", Input.GetAxis ("Joystick Axis 5"));
		AxisMap ("DPAD V: Joystick Axis 6", Input.GetAxis ("Joystick Axis 6"));
		AxisMap ("L2: Joystick Axis 7", Input.GetAxis ("Joystick Axis 7"));
		AxisMap ("R2: Joystick Axis 8", Input.GetAxis ("Joystick Axis 8"));
	}


	void ButtonMap (string label, bool value)
	{
		LabelLabel (label, value ? "Down" : "Up");
	}


	void AxisMap (string label, float value)
	{
		LabelLabel (label, value.ToString ("F3"));
	}


	void LabelLabel (string label, string otherLabel)
	{
		GUILayout.BeginHorizontal (GUI.skin.box, GUILayout.Width (400.0f));
			GUILayout.Label (label);
			GUILayout.FlexibleSpace ();
			GUILayout.Label (otherLabel);
		GUILayout.EndHorizontal ();
	}
}
