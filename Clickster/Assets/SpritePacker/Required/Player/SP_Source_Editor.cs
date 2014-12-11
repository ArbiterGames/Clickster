#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public partial class SP_Source
{
	public List<SP_Rect> Compile()
	{
		var rects     = new List<SP_Rect>();
		var path      = AssetDatabase.GUIDToAssetPath(identifier);
		var texture2D = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
		
		if (texture2D != null)
		{
			var importer = FixSettings(texture2D, path);
			
			if (importer != null)
			{
				var pixels      = new SP_Pixels(texture2D);
				var sprites     = SP_Helper.LoadAllObjectsAtPath<Sprite>(path);
				var spritesheet = importer.spritesheet;
				
				if (sprites.Length > 0)
				{
					for (var i = 0; i < sprites.Length; i++)
					{
						var sprite    = sprites[i];
						var subPixels = pixels.GetSubset((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height);
						var newRect   = new SP_Rect(); rects.Add(newRect);
						
						newRect.Identifier = identifier;
						newRect.Index      = i;
						newRect.Name       = sprite.name;
						newRect.BorderType = borderType;
						newRect.Pixels     = subPixels;
						newRect.KeepPivot  = keepPivot;
						newRect.Border     = sprite.border;
						
						// The sprites order may differ from the spiresheets
						if (spritesheet != null && spritesheet.Length > 0)
						{
							for (var j = spritesheet.Length - 1; j >= 0; j--)
							{
								var smd = spritesheet[j];
								
								if (sprite.name == smd.name)
								{
									newRect.Index = j; break;
								}
							}
						}
						
						TrimRect(newRect, GetPivot(sprite));
					}
				}
				else
				{
					var newRect = new SP_Rect(); rects.Add(newRect);
					
					newRect.Identifier = identifier;
					newRect.Index      = -1;
					newRect.Name       = texture2D.name;
					newRect.BorderType = borderType;
					newRect.Pixels     = pixels;
					newRect.KeepPivot  = keepPivot;
					
					TrimRect(newRect, new Vector2(0.5f, 0.5f));
				}
			}
		}
		
		return rects;
	}
	
	private Vector2 GetPivot(Sprite sprite)
	{
		var pivot = default(Vector2);
		
		pivot.x = SP_Helper.Divide(-sprite.bounds.min.x, sprite.bounds.size.x);
		pivot.y = SP_Helper.Divide(-sprite.bounds.min.y, sprite.bounds.size.y);
		
		return pivot;
	}
	
	private void TrimRect(SP_Rect rect, Vector2 pivot)
	{
		if (trim == true && rect.Border == Vector4.zero)
		{
			var sourceRect  = new Rect(0.0f, 0.0f, rect.Pixels.Width, rect.Pixels.Height);
			var trimmedRect = default(Rect);
			var pivotX      = pivot.x * sourceRect.width;
			var pivotY      = pivot.y * sourceRect.height;
			
			rect.Pixels = rect.Pixels.GetTrimmed(ref trimmedRect);
			
			pivotX = SP_Helper.Divide(pivotX - trimmedRect.xMin, trimmedRect.width );
			pivotY = SP_Helper.Divide(pivotY - trimmedRect.yMin, trimmedRect.height);
			
			rect.Pivot = new Vector2(pivotX, pivotY);
		}
		else
		{
			rect.Pivot = pivot;
		}
		
		rect.Width  = Mathf.FloorToInt(rect.Pixels.Width)  + borderSize * 2;
		rect.Height = Mathf.FloorToInt(rect.Pixels.Height) + borderSize * 2;
	}
	
	private TextureImporter FixSettings(Texture2D texture2D, string path)
	{
		var importer = SP_Helper.GetAssetImporter<TextureImporter>(path);
		
		if (importer != null)
		{
			if (importer.isReadable != true || importer.textureFormat != TextureImporterFormat.AutomaticTruecolor)
			{
				importer.isReadable    = true;
				importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
				
				SP_Helper.ReimportAsset(importer.assetPath);
			}
		}
		
		return importer;
	}
}
#endif