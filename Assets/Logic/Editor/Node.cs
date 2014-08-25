using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;


public interface IGraph
{
	bool Vertical {get;}
	Vector2 Offset {get;}
	void Remove (Node target);
}


public class Node
{
	const float kNodeSize = 100.0f, kNodeProportions = 1.5f, kConnectionWidth = 8.0f;

	static Node selection = null;
	static bool connecting = false;

	Vector2 position;
	Rect nodeRect;
	string name;
	List<Node> targets = new List<Node> ();
	IGraph m_Parent;


	public Node (string name, Vector2 position, IGraph parent)
	{
		this.name = name;
		Position = position;
		m_Parent = parent;
	}


	public static Node Selection
	{
		get
		{
			return selection;
		}
		set
		{
			selection = value;
			if (selection == null)
			{
				connecting = false;
			}
		}
	}


	public Vector2 Position
	{
		get
		{
			return position;
		}
		set
		{
			position = value;

			nodeRect = new Rect (
				position.x - kNodeSize * 0.5f,
				position.y - (kNodeSize / kNodeProportions) * 0.5f,
				kNodeSize,
				kNodeSize / kNodeProportions
			);
		}
	}


	public ReadOnlyCollection<Node> Targets
	{
		get
		{
			return targets.AsReadOnly ();
		}
	}


	public void ConnectTo (Node target)
	{
		if (targets.Contains (target))
		{
			return;
		}

		targets.Add (target);
	}


	public void Remove (Node target)
	{
		targets.Remove (target);
	}


	public void OnGUI (GUIStyle style)
	{
		Rect rect = new Rect (nodeRect.x + m_Parent.Offset.x, nodeRect.y + m_Parent.Offset.y, nodeRect.width, nodeRect.height);

		switch (Event.current.type)
		{
			case EventType.mouseDown:
				if (rect.Contains (Event.current.mousePosition))
				// Select this node if we clicked it
				{
					selection = this;

					if (Event.current.button != 0)
					{
						Event.current.clickCount = 0;
					}

					if (selection == this && Event.current.button == 1)
					{
						m_Parent.Remove (this);
						selection = null;
					}
					else if (Event.current.clickCount == 2)
					// If we double-clicked it, enter connect mode
					{
						connecting = true;
					}

					Event.current.Use ();
				}
			break;
			case EventType.mouseUp:
				// If we released the mouse button...
				if (selection == null)
				// ... with no active selection, ignore the event
				{
					break;
				}
				else if (selection == this)
				// ... while this node was active selection...
				{
					if (!connecting)
					// ... and we were not in connect mode, clear the selection
					{
						Selection = null;
						Event.current.Use ();
					}
				}
				else if (connecting && rect.Contains (Event.current.mousePosition))
				// ... over this component while in connect mode, connect selection to this node and clear selection
				{
					selection.ConnectTo (this);
					Selection = null;
					Event.current.Use ();
				}
			break;
			case EventType.mouseDrag:
				if (selection == this)
				// If doing a mouse drag with this component selected...
				{
					if (connecting)
					// ... and in connect mode, just use the event as we'll be painting the new connection
					{
						Event.current.Use ();
					}
					else
					// ... and not in connect mode, drag the component
					{
						Position += Event.current.delta;
						Event.current.Use ();
					}
				}
			break;
			case EventType.repaint:
				style.Draw (rect, new GUIContent (name), false, false, false, false);
					// The component box

				if (selection == this && connecting)
				// The new connection
				{
					GUI.color = Color.red;
					DrawConnection (position + m_Parent.Offset, Event.current.mousePosition, m_Parent.Vertical, false, true);
					GUI.color = Color.white;
				}
			break;
		}
	}


	public static void DrawConnection (Vector2 from, Vector2 to, bool vertical, bool absoluteFrom = false, bool absoluteTo = false)
	// Render a node connection between the two given points
	{
		if (vertical)
		{
			bool top = from.y > to.y;

			Handles.DrawBezier(
				new Vector3 (from.x, from.y + (absoluteFrom ? 0.0f : ((top ? -kNodeSize : kNodeSize) / kNodeProportions) * 0.5f), 0.0f),
				new Vector3 (to.x, to.y + (absoluteTo ? 0.0f : ((top ? kNodeSize : -kNodeSize) / kNodeProportions) * 0.5f), 0.0f),
				new Vector3 (from.x, from.y, 0.0f) + Vector3.up * (kNodeSize / kNodeProportions) * (top ? -1.0f : 1.0f),
				new Vector3 (to.x, to.y, 0.0f) + Vector3.up * (kNodeSize / kNodeProportions) * (top ? 1.0f : -1.0f),
				GUI.color,
				null,
				kConnectionWidth
			);
		}
		else
		{
			bool left = from.x > to.x;

			Handles.DrawBezier(
				new Vector3 (from.x + (left ? -kNodeSize : kNodeSize) * 0.5f, from.y, 0.0f),
				new Vector3 (to.x + (left ? kNodeSize : -kNodeSize) * 0.5f, to.y, 0.0f),
				new Vector3 (from.x, from.y, 0.0f) + Vector3.right * kNodeSize * (left ? -1.0f : 1.0f),
				new Vector3 (to.x, to.y, 0.0f) + Vector3.right * kNodeSize * (left ? 1.0f : -1.0f),
				GUI.color,
				null,
				kConnectionWidth
			);
		}
	}
}
