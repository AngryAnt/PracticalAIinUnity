using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class GraphTool : EditorWindow, IGraph
{
	const float kGestureDistanceSQ = 40.0f * 40.0f, kGestureAccuracy = 0.9f;


	List<Node> nodes = new List<Node> (), removeNodes = new List<Node> ();
	Vector2 m_DragStart;


	[MenuItem ("Window/Graph Tool")]
	static void Launch ()
	{
		GetWindow<GraphTool> ().title = "Graph";
	}


	public bool Vertical
	{
		get; set;
	}


	public Vector2 Offset
	{
		get; set;
	}


	public void Remove (Node target)
	{
		removeNodes.Add (target);
	}


	void DoRemove ()
	{
		foreach (Node target in removeNodes)
		{
			nodes.Remove (target);
			foreach (Node node in nodes)
			{
				 node.Remove (target);
			}
		}

		removeNodes.Clear ();
	}


	Styles m_Styles;
	Styles Styles
	{
		get
		{
			if (m_Styles == null)
			{
				m_Styles = new Styles ();
			}

			return m_Styles;
		}
	}


	void OnGUI ()
	{
		EditorGUILayout.Space ();
		GUILayout.Label (title, Styles.m_TitleStyle);

		if (nodes.Count < 1)
		{
			GUILayout.FlexibleSpace ();
			GUILayout.Label ("Double-click anywhere to get started.", EditorStyles.wordWrappedMiniLabel);
		}

		// Render all connections first //

		if (Event.current.type == EventType.repaint)
		{
			foreach (Node node in nodes)
			{
				foreach (Node target in node.Targets)
				{
					Node.DrawConnection (node.Position + Offset, target.Position + Offset, Vertical);
				}
			}
		}

		GUI.changed = false;

		foreach (Node node in nodes)
		// Handle all nodes
		{
			node.OnGUI (Styles.m_NodeStyle);
		}

		DoRemove ();

		wantsMouseMove = Node.Selection != null;
			// If we have a selection, we're doing an operation which requires an update each mouse move

		switch (Event.current.type)
		// Handle events not handled by the notes
		{
			case EventType.mouseUp:
			// If we had a mouse up event, clear our selection
				Node.Selection = null;
				Event.current.Use ();
			break;
			case EventType.mouseDown:
				m_DragStart = Event.current.mousePosition;

				if (Event.current.clickCount == 2)
				// If we double-click, create a new node there
				{
					Node.Selection = new Node (Names.Give () ?? "Node " + nodes.Count, Event.current.mousePosition - Offset, this);
					nodes.Add (Node.Selection);
					Event.current.Use ();
				}
			break;
			case EventType.mouseDrag:
				if (Event.current.button == 0)
				// Dragging with the left mouse button updates the offset - thus moving the canvas
				{
					Offset += Event.current.delta;
					Event.current.Use ();
				}
				else if (Event.current.button == 1)
				// Dragging horizontally or vertically with the right mouse adjusts the graph orientation
				{
					Vector2 vector = Event.current.mousePosition - m_DragStart;

					if (vector.sqrMagnitude > kGestureDistanceSQ)
					{
						float dot = Mathf.Abs (Vector2.Dot (Vector2.up, vector.normalized));

						if (!Vertical && dot > kGestureAccuracy)
						{
							Vertical = true;
							Event.current.Use ();
						}
						else if (Vertical && 1.0f - dot > kGestureAccuracy)
						{
							Vertical = false;
							Event.current.Use ();
						}
					}
				}
			break;
		}

		if (GUI.changed)
		// Repaint if we changed anything
		{
			Repaint ();
		}
	}
}


class Styles
{
	public GUIStyle
		m_NodeStyle,
		m_TitleStyle;


	public Styles ()
	{
		m_NodeStyle = GUI.skin.FindStyle ("NodeStyle") ?? new GUIStyle (GUI.skin.FindStyle ("flow node 4") ?? GUI.skin.box)
		{
			name = "NodeStyle",
			fontSize = 11,
			alignment = TextAnchor.MiddleCenter
		};

		m_TitleStyle = GUI.skin.FindStyle ("TitleStyle") ?? new GUIStyle (GUI.skin.label)
		{
			name = "TitleStyle",
			fontSize = 20,
			alignment = TextAnchor.MiddleCenter
		};
	}
}
