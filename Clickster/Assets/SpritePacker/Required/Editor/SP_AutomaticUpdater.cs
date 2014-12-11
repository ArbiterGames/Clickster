using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SP_AutomaticUpdater : AssetPostprocessor
{
	private static List<SP_SpritePacker> dirtyPackers = new List<SP_SpritePacker>();
	
	public void OnPostprocessTexture(Texture2D texture)
	{
		var identifier = AssetDatabase.AssetPathToGUID(assetPath);
		
		if (string.IsNullOrEmpty(identifier) == false)
		{
			var spritePackers = SP_Helper.LoadAllPrefabComponents<SP_SpritePacker>();
			
			foreach (var spritePacker in spritePackers)
			{
				if (dirtyPackers.Contains(spritePacker) == false)
				{
					foreach (var source in spritePacker.Sources)
					{
						if (source != null && source.Identifier == identifier)
						{
							EditorApplication.delayCall += RebuildDirtyPackers;
							
							dirtyPackers.Add(spritePacker); break;
						}
					}
				}
			}
		}
	}
	
	private static void RebuildDirtyPackers()
	{
		foreach (var dirtyPacker in dirtyPackers)
		{
			if (dirtyPacker != null)
			{
				dirtyPacker.Rebuild();
			}
		}
		
		dirtyPackers.Clear();
	}
}