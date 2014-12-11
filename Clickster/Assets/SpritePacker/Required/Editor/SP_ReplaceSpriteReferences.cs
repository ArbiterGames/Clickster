using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class SP_ReplaceSpriteReferences
{
	private static List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
	
	private static int totalReplaceCount;
	
	[MenuItem("Assets/Replace Sprite References", true)]
	public static bool ReplaceSpriteReferencesValidate()
	{
		foreach (var selectedObject in Selection.objects)
		{
			if (selectedObject.GetType() == typeof(Sprite))
			{
				return true;
			}
		}
		
		return false;
	}
	
	[MenuItem("Assets/Replace Sprite References", false, 20)]
	public static void ReplaceSpriteReferences()
	{
		totalReplaceCount = 0;
		
		FindSpriteRenderers();
		
		var spritePackers = SP_Helper.LoadAllPrefabComponents<SP_SpritePacker>();
		
		foreach (var selectedObject in Selection.objects)
		{
			if (selectedObject.GetType() == typeof(Sprite))
			{
				var path       = AssetDatabase.GetAssetPath(selectedObject);
				var identifier = AssetDatabase.AssetPathToGUID(path);
				
				if (string.IsNullOrEmpty(identifier) == false)
				{
					foreach (var spritePacker in spritePackers)
					{
						// Did we find the sprite packer that generated this texture?
						if (spritePacker.Identifier == identifier)
						{
							var result = spritePacker.Results.Find(r => r.Sprite == selectedObject);
							
							if (result != null)
							{
								var sprites = SP_Helper.LoadAllObjectsAtGUID<Sprite>(result.Identifier);
								
								if (sprites.Length > 0 && result.Index < sprites.Length)
								{
									ReplaceSpriteReference(sprites[result.Index], (Sprite)selectedObject);
								}
								else
								{
									Debug.LogWarning(selectedObject.name + " doesn't have a source sprite, so it can't be replaced");
								}
							}
							
							break;
						}
					}
				}
			}
		}
		
		if (totalReplaceCount == 0)
		{
			Debug.Log("Failed to find any sprite references to replace");
		}
	}
	
	private static void ReplaceSpriteReference(Sprite oldSprite, Sprite newSprite)
	{
		var replaceCount = 0;
		
		if (oldSprite != null && newSprite != null && newSprite.name.StartsWith(oldSprite.name) == true)
		{
			// Replace all
			foreach (var spriteRenderer in spriteRenderers)
			{
				if (spriteRenderer.sprite == oldSprite)
				{
					spriteRenderer.sprite = newSprite; totalReplaceCount += 1; replaceCount += 1;
				}
			}
		}
		
		if (replaceCount > 0)
		{
			Debug.Log(oldSprite.name + " was replced with " + newSprite.name + " in " + replaceCount + " SpriteRenderer" + (replaceCount > 1 ? "s" : ""));
		}
	}
	
	private static void FindSpriteRenderers()
	{
		spriteRenderers.Clear();
		
		spriteRenderers.AddRange(SP_Helper.LoadAllPrefabComponents<SpriteRenderer>());
		
		spriteRenderers.AddRange(SP_Helper.FindAll<SpriteRenderer>());
	}
}