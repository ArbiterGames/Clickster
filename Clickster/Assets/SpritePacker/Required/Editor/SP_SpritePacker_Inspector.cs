using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(SP_SpritePacker))]
public class SP_SpritePacker_Inspector : SP_Inspector<SP_SpritePacker>
{
	private SP_Source activeSource;
	
	[MenuItem("Assets/Create/Sprite Packer")]
	public static void Create()
	{
		var gameObject = new GameObject("Sprite Packer"); gameObject.AddComponent<SP_SpritePacker>();
		var dir        = AssetDatabase.GenerateUniqueAssetPath(SP_Helper.GetContextDirectory() + "/" + gameObject.name + ".prefab");
		var prefab     = PrefabUtility.CreatePrefab(dir, gameObject); SP_Helper.SafeDestroy(gameObject);
		
		Selection.activeGameObject = prefab;
		EditorGUIUtility.PingObject(prefab);
	}
	
	protected override void OnInspector()
	{
		SP_Inspector.Separator();
		
		DrawSources();
		
		SP_Inspector.Separator(Target.Sources.Count > 0);
		
		DrawOptions();
		
		SP_Inspector.Separator();
		
		DrawDefaults();
		
		SP_Inspector.Separator();
		
		DrawDragAndDropButton();
		
		DrawRebuildButton();
		
		SP_Inspector.Separator();
	}
	
	private void DrawSources()
	{
		var hoveredSource = Event.current.type == EventType.Repaint ? default(SP_Source) : activeSource;
		
		if (Target.Sources.Count > 0)
		{
			for (var i = 0; i < Target.Sources.Count; i++)
			{
				var source = Target.Sources[i];
				
				if (source != null)
				{
					SP_Inspector.Separator(i > 0);
					
					var rect = EditorGUILayout.BeginVertical(source == activeSource ? SP_Inspector.LightButton : SP_Inspector.DarkButton);
					{
						var identifier = source.Identifier;
						var path       = AssetDatabase.GUIDToAssetPath(identifier);
						var texture2D  = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
						
						EditorGUI.BeginChangeCheck();
						{
							EditorGUILayout.BeginVertical(texture2D == null ? SP_Inspector.Error : SP_Inspector.None);
							{
								path = AssetDatabase.GetAssetPath(EditorGUILayout.ObjectField(texture2D, typeof(Texture2D), true));
							}
							EditorGUILayout.EndVertical();
							
							source.Identifier = AssetDatabase.AssetPathToGUID(path);
							
							if (source == activeSource)
							{
								source.Trim       = EditorGUILayout.Toggle("Trim", source.Trim);
								source.KeepPivot  = EditorGUILayout.Toggle("Keep Pivot", source.KeepPivot);
								source.BorderSize = EditorGUILayout.IntField("Border Size", source.BorderSize);
								source.BorderType = (SP_BorderType)EditorGUILayout.EnumPopup("Border Type", source.BorderType);
								
								CheckBorders(source);
							}
						}
						if (EditorGUI.EndChangeCheck() == true && Target.AutoRebuild == true)
						{
							Target.Rebuild();
							
							SP_Helper.SetDirty(Target);
						}
					}
					EditorGUILayout.EndVertical();
					
					if (rect.Contains(Event.current.mousePosition) == true)
					{
						hoveredSource = activeSource = source;
					}
				}
			}
		}
		
		if (hoveredSource == null)
		{
			//activeSource = null;
		}
	}
	
	private void DrawDragAndDropButton()
	{
		var rect = SP_Inspector.Reserve(16.0f);
		
		if (GUI.Button(rect, "Drag and Drop") == true)
		{
			SP_DragAndDrop.Open();
		}
		
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
								
								Target.AddSource(objectIdentifier);
								
								rebuild = true;
							}
						}
						
						if (rebuild == true && Target.AutoRebuild == true)
						{
							Target.Rebuild();
						}
						
						SP_Helper.SetDirty(Target);
					}
				}
				break;
			}
		}
		
		SP_Inspector.DrawDragAndDropZone(rect);
	}
	
	private void DrawRebuildButton()
	{
		if (GUILayout.Button("Rebuild") == true)
		{
			Target.Rebuild();
			
			SP_Helper.SetDirty(Target);
		}
	}
	
	private void DrawOptions()
	{
		EditorGUI.BeginChangeCheck();
		{
			Target.Algorithm       = (SP_Algorithm)EditorGUILayout.EnumPopup("Alogirthm", Target.Algorithm);
			Target.MaxSize         = EnumValuesPopup<SP_PowerOfTwo>("Max Size", Target.MaxSize);
			Target.ForceSquare     = EditorGUILayout.Toggle("Force Square", Target.ForceSquare);
			Target.SuffixAtlasName = EditorGUILayout.Toggle("Suffix Atlas Name", Target.SuffixAtlasName);
			Target.AutoRebuild     = EditorGUILayout.Toggle("Auto Rebuild", Target.AutoRebuild);
		}
		if (EditorGUI.EndChangeCheck() == true && Target.AutoRebuild == true)
		{
			Target.Rebuild();
			
			SP_Helper.SetDirty(Target);
		}
	}
	
	private void DrawDefaults()
	{
		Target.DefaultTrim       = EditorGUILayout.Toggle("Default Trim", Target.DefaultTrim);
		Target.DefaultKeepPivot  = EditorGUILayout.Toggle("Default Keep Pivot", Target.DefaultKeepPivot);
		Target.DefaultBorderSize = EditorGUILayout.IntField("Default Border Size", Target.DefaultBorderSize);
		Target.DefaultBorderType = (SP_BorderType)EditorGUILayout.EnumPopup("Default Border Type", Target.DefaultBorderType);
	}
	
	private T EnumValuesPopup<T>(string handle, T e)
	{
		var values = (int[])System.Enum.GetValues(typeof(T));
		var names  = System.Array.ConvertAll<int, string>(values, v => v.ToString());
		
		return (T)(object)EditorGUILayout.IntPopup(handle, (int)(object)e, names, values);
	}
	
	private void CheckBorders(SP_Source source)
	{
		if (source != null && source.Trim == true && string.IsNullOrEmpty(source.Identifier) == false)
		{
			var path = AssetDatabase.GUIDToAssetPath(source.Identifier);
			
			if (string.IsNullOrEmpty(path) == false)
			{
				var sprites = SP_Helper.LoadAllObjectsAtPath<Sprite>(path);
				
				for (var i = 0; i < sprites.Length; i++)
				{
					if (sprites[i].border != Vector4.zero)
					{
						EditorGUILayout.HelpBox("This sprite has a border, so it cannot be trimmed.", MessageType.Warning); return;
					}
				}
			}
		}
	}
}