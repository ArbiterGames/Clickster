using UnityEngine;
using UnityEditor;

public abstract class SP_Window<T> : EditorWindow
	where T : SP_Window<T>
{
	[SerializeField]
	private Vector2 scrollPos;
	
	[SerializeField]
	private float height = 100;
	
	public void OnGUI()
	{
		var oldHeight = height;
		
		scrollPos = GUI.BeginScrollView(new Rect (0, 0, position.width, position.height), scrollPos, new Rect (0, 0, 0, height));
		
		OnInspector();
		
		var r = EditorGUILayout.BeginVertical(); EditorGUILayout.LabelField("", GUILayout.Height(0.0f)); EditorGUILayout.EndVertical();;
		
		if (Event.current.type == EventType.Repaint)
		{
			height = r.y;
		}
		
		GUI.EndScrollView();
		
		if (GUI.changed == true || oldHeight != height)
		{
			Repaint();
		}
	}
	
	public void OnSelectionChange()
	{
		Repaint();
	}
	
	public void OnFocus()
	{
		Repaint();
	}
	
	public void OnLostFocus()
	{
		Repaint();
	}
	
	public void OnHierarchyChange()
	{
		Repaint();
	}
	
	public void OnProjectChange()
	{
		Repaint();
	}
	
	public abstract void OnInspector();
}