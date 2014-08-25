using UnityEngine;
#if UNITY_EDITOR
	using UnityEditor;
	using UnityAssets;
#endif
using System.Collections;


public class ShowWindowOnStart : MonoBehaviour
{
	public enum MaximizeBehaviour
	{
		Unchanged,
		Maximize,
		Normalize
	};


	public string typeName, title;
	public MaximizeBehaviour maximizeBehaviour = MaximizeBehaviour.Unchanged;

	
#if UNITY_EDITOR
	void Start ()
	{
		EditorWindow window = Utility.FindInstance<EditorWindow> (typeName);

		if (window == null)
		{
			Debug.LogError ("Could not find EditorWindow instance of type: " + typeName);
		}
		else
		{
			if (!string.IsNullOrEmpty (title))
			{
				window.title = title;
			}

			switch (maximizeBehaviour)
			{
				case MaximizeBehaviour.Maximize:
					window.maximized = true;
				break;
				case MaximizeBehaviour.Normalize:
					window.maximized = false;
				break;
			}

			window.Show ();
			window.Focus ();
		}
	}
#endif
}
