using UnityEngine;
using UnityEditor;
using System.IO;


[CustomEditor (typeof (Names))]
public class Names : Editor, IRunnable
{
	public void Run ()
	{
		CurrentCollection = this;
	}


	[MenuItem ("Assets/Create/Collection of names")]
	static void Create ()
	{
		string activePath = AssetDatabase.GetAssetPath (Selection.activeObject);
		activePath = string.IsNullOrEmpty (activePath) ? "Assets/" : activePath;

		AssetDatabase.CreateAsset (
			ScriptableObject.CreateInstance<Names> (),
			AssetDatabase.GenerateUniqueAssetPath (Path.GetDirectoryName (activePath) + "/Names.asset")
		);
	}


	public static Names CurrentCollection
	{ get; set; }


	public static string Give ()
	{
		return CurrentCollection == null ? null : CurrentCollection.Pop ();
	}


	[SerializeField]
	string[] m_Names;
	int m_LastIndex = -1;


	public string Peek ()
	{
		return (m_Names.Length < 1 || m_LastIndex < 0) ? null : m_Names[m_LastIndex % m_Names.Length];
	}


	public string Pop ()
	{
		++m_LastIndex;
		return Peek ();
	}


	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();
		EditorGUILayout.Space ();

		if (target != null && GUILayout.Button ("Load", EditorStyles.miniButton))
		{
			Names.CurrentCollection = (Names)target;
		}
	}
}
