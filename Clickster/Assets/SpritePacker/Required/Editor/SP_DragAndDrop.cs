using UnityEngine;
using UnityEditor;

public class SP_DragAndDrop : SP_Window<SP_DragAndDrop>
{
	public static void Open()
	{
		EditorWindow.GetWindow(typeof(SP_DragAndDrop)).title = "Drag and Drop";
	}
	
	public override void OnInspector()
	{
		var spritePackers = SP_Helper.LoadAllPrefabComponents<SP_SpritePacker>();
		
		for (var i = 0; i < spritePackers.Count; i++)
		{
			SP_Inspector.Separator(i > 0);
			
			DrawDragAndDropArea(spritePackers[i]);
		}
	}
	
	private void DrawDragAndDropArea(SP_SpritePacker spritePacker)
	{
		var style = new GUIStyle(EditorStyles.boldLabel); style.alignment = TextAnchor.MiddleCenter; style.fontSize = 18; style.normal.textColor = Color.gray;
		var rect  = SP_Inspector.Reserve(50.0f);
		
		if (GUI.Button(rect, spritePacker.name) == true)
		{
			Selection.activeGameObject = spritePacker.gameObject;
			EditorGUIUtility.PingObject(spritePacker.gameObject);
		}
		
		SP_Inspector.DrawDragAndDropZone(rect);
		
		if (rect.Contains(Event.current.mousePosition) == true)
		{
			switch (Event.current.type)
			{
				case EventType.DragUpdated:
				{
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
				}
				break;
				case EventType.DragPerform:
				{
					if (Event.current.type == EventType.DragPerform)
					{
						DragAndDrop.AcceptDrag();
						
						var rebuild = false;
						
						foreach (var objectReference in DragAndDrop.objectReferences)
						{
							if (objectReference is Texture2D)
							{
								var objectIdentifier = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(objectReference));
								
								spritePacker.AddSource(objectIdentifier);
								
								rebuild = true;
							}
						}
						
						if (rebuild == true && spritePacker.AutoRebuild == true)
						{
							spritePacker.Rebuild();
						}
						
						SP_Helper.SetDirty(spritePacker);
					}
				}
				break;
			}
		}
	}
}