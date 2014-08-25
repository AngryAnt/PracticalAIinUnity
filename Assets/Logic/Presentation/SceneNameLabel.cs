using UnityEngine;
using System.Collections;

public class SceneNameLabel : MonoBehaviour
{
	void Start ()
	{
		guiText.text = Application.loadedLevelName;
	}
}
