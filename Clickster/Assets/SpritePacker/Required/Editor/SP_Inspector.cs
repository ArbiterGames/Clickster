using UnityEngine;
using UnityEditor;

public static class SP_Inspector
{
	private static Texture2D checker;
	private static GUIStyle  none;
	private static GUIStyle  error;
	private static GUIStyle  darkButton;
	private static GUIStyle  lightButton;
	
	public static Texture2D Checker
	{
		get
		{
			if (checker == null)
			{
				checker = SP_Helper.CreateTempTexture(20, 20, "iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAQElEQVQ4EWNkYGBoAGJiQAMxipiIUUSKmlEDSQkt7GpHwxB7uJAiykiC4gZi1I5GCjGhhF/NaBjiDx9iZKkehgA9MgGnI6/4wwAAAABJRU5ErkJggg==");
			}
			
			return checker;
		}
	}
	
	public static GUIStyle None
	{
		get
		{
			if (none == null)
			{
				none = new GUIStyle();
			}
			
			return none;
		}
	}
	
	public static GUIStyle Error
	{
		get
		{
			if (error == null)
			{
				error                   = new GUIStyle();
				error.border            = new RectOffset(3, 3, 3, 3);
				error.normal            = new GUIStyleState();
				error.normal.background = SP_Helper.CreateTempTexture(12, 12, "iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAALElEQVQIHWP4z8CgC8SHgfg/lNZlQBIACYIlGEEMBjTABOQfQRM7AlKGYSYAoOwcvDRV9/MAAAAASUVORK5CYII=");
			}
			
			return error;
		}
	}
	
	public static GUIStyle DarkButton
	{
		get
		{
			if (darkButton == null)
			{
				darkButton                   = new GUIStyle();
				darkButton.border            = new RectOffset(3, 3, 3, 3);
				darkButton.margin            = new RectOffset(0, 0, 0, 0);
				darkButton.padding           = new RectOffset(2, 2, 2, 2);
				darkButton.alignment         = TextAnchor.MiddleCenter;
				darkButton.normal            = new GUIStyleState();
				darkButton.normal.background = SP_Helper.CreateTempTexture(8, 8, "iVBORw0KGgoAAAANSUhEUgAAAAgAAAAICAYAAADED76LAAAAdElEQVQYGX3PwQ1AMBiG4eLSUw/EDkxjBzswhCGEQazAJA56ceT9pL2Q+JOnaf9+bVpjjLGYceAKfOjZhMmEGgN2qAr02BTQyRZxk+lTCo0pg8N7Uwn1nAK/pYAepOveldPwGUOFBitOqEp0WLSI39RNn2/e2dAYftTMrW8AAAAASUVORK5CYII=");
				darkButton.active            = new GUIStyleState();
				darkButton.active.background = SP_Helper.CreateTempTexture(8, 8, "iVBORw0KGgoAAAANSUhEUgAAAAgAAAAICAYAAADED76LAAAAnElEQVQYGWNQUFDg4OHhWcjExPSRgYHhPwgD2Z9AYiA5RiBjwf///zX9/Py61NTU3gIVMNy6dUt406ZNZUCF5xlBOsPDw5NgkiAFIABStHLlynlM//7940OXBCkAiYHkmEAcfABoA9MnkHHoim7fvi0EkmPm5ubWACrwFxUVPScsLPwdpPDatWuiW7duLWVjY9vLgOTNT0A5DG8CAPN+PbDeZ/24AAAAAElFTkSuQmCC");
			}
			
			return darkButton;
		}
	}
	
	public static GUIStyle LightButton
	{
		get
		{
			if (lightButton == null)
			{
				lightButton                   = new GUIStyle();
				lightButton.border            = new RectOffset(3, 3, 3, 3);
				lightButton.margin            = new RectOffset(0, 0, 0, 0);
				lightButton.padding           = new RectOffset(2, 0, 2, 2);
				lightButton.alignment         = TextAnchor.MiddleCenter;
				lightButton.normal            = new GUIStyleState();
				lightButton.normal.background = SP_Helper.CreateTempTexture(8, 8, "iVBORw0KGgoAAAANSUhEUgAAAAgAAAAICAYAAADED76LAAAAnElEQVQYGWNQUFDg4OHhWcjExPSRgYHhPwgD2Z9AYiA5RiBjwf///zX9/Py61NTU3gIVMNy6dUt406ZNZUCF5xlBOsPDw5NgkiAFIABStHLlynlM//7940OXBCkAiYHkmEAcfABoA9MnkHHoim7fvi0EkmPm5ubWACrwFxUVPScsLPwdpPDatWuiW7duLWVjY9vLgOTNT0A5DG8CAPN+PbDeZ/24AAAAAElFTkSuQmCC");
				lightButton.active            = new GUIStyleState();
				lightButton.active.background = SP_Helper.CreateTempTexture(8, 8, "iVBORw0KGgoAAAANSUhEUgAAAAgAAAAICAYAAADED76LAAAAmUlEQVQYGWMwMDDgEBYWXsjMzPyRgYHhPwgD2Z9AYiA5RiBjwb9//zRLSkq6rKys3gIVMBw7dky4p6enDKjwPCNIZ1NTUxJMEqQABECK6urq5jH9/fuXD10SpAAkBpJjAnHwASaQg0DGoSs6ceKEEEiOWUhISAOowF9eXv6cjIzMd5DCgwcPivb395dycXHtZUDy5iegHIY3AbzJPlhNnSj4AAAAAElFTkSuQmCC");
			}
			
			return lightButton;
		}
	}
	
	public static void Separator(bool draw = true)
	{
		EditorGUILayout.Separator();
	}
	
	public static Rect Reserve(float height = 5.0f)
	{
		var rect = EditorGUILayout.BeginVertical();
		{
			EditorGUILayout.LabelField("", GUILayout.Height(height));
		}
		EditorGUILayout.EndVertical();
		
		return rect;
	}
	
	public static void DrawDragAndDropZone(Rect rect)
	{
		DrawTiledTexture(new Rect(rect.xMin    , rect.yMin    , rect.width, 2 ), Checker);
		DrawTiledTexture(new Rect(rect.xMin    , rect.yMax - 2, rect.width, 2 ), Checker);
		DrawTiledTexture(new Rect(rect.xMin    , rect.yMin    , 2, rect.height), Checker);
		DrawTiledTexture(new Rect(rect.xMax - 2, rect.yMin    , 2, rect.height), Checker);
	}
	
	public static void DrawTiledTexture(Rect rect, Texture texture)
	{
		var coords = new Rect(0.0f, 0.0f, rect.width / (float)texture.width, rect.height / (float)texture.height);
		
		GUI.DrawTextureWithTexCoords(rect, texture, coords, true);
	}
}

public abstract class SP_Inspector<T> : Editor
	where T : MonoBehaviour
{
	protected T Target;
	
	public override void OnInspectorGUI()
	{
		Target = target as T;
		
		OnInspector();
	}
	
	protected virtual void OnSelectionChange()
	{
		Repaint();
	}
	
	protected virtual void OnFocus()
	{
		Repaint();
	}
	
	protected virtual void OnLostFocus()
	{
		Repaint();
	}
	
	protected virtual void OnHierarchyChange()
	{
		Repaint();
	}
	
	protected virtual void OnProjectChange()
	{
		Repaint();
	}
	
	protected abstract void OnInspector();
}