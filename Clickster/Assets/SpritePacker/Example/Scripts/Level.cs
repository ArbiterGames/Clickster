using UnityEngine;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
	public int          Size               = 20;
	public int          Height             = 20;
	public int          Depth              = 20;
	public List<Sprite> SkySprites         = new List<Sprite>();
	public List<Sprite> GroundSprites      = new List<Sprite>();
	public List<Sprite> UndergroundSprites = new List<Sprite>();
	
	[ContextMenu("Regenerate")]
	private void Regenerate()
	{
		Clear();
		
		for (var i = -Size; i <= Size; i++)
		{
			if (SkySprites.Count > 0)
			{
				for (var j = 0; j < Height; j++)
				{
					var sky   = new GameObject("Sky");
					var renderer = sky.AddComponent<SpriteRenderer>();
					
					sky.transform.parent        = transform;
					sky.transform.localPosition = new Vector3(i, j + 1, 0.0f);
					
					renderer.sprite = SkySprites[Random.Range(0, SkySprites.Count)];
				}
			}
			
			if (SkySprites.Count > 0)
			{
				var ground   = new GameObject("Ground");
				var renderer = ground.AddComponent<SpriteRenderer>();
				
				ground.transform.parent        = transform;
				ground.transform.localPosition = new Vector3(i, 0.0f, 0.0f);
				
				renderer.sprite = GroundSprites[Random.Range(0, GroundSprites.Count)];
			}
			
			if (UndergroundSprites.Count > 0)
			{
				for (var j = 0; j < Depth; j++)
				{
					var underground = new GameObject("Underground");
					var renderer    = underground.AddComponent<SpriteRenderer>();
					
					underground.transform.parent        = transform;
					underground.transform.localPosition = new Vector3(i, -j - 1, 0.0f);
					
					renderer.sprite = UndergroundSprites[Random.Range(0, UndergroundSprites.Count)];
				}
			}
		}
	}
	
	private void Clear()
	{
		for (var i = transform.childCount - 1; i >= 0; i--)
		{
			SP_Helper.SafeDestroy(transform.GetChild(i).gameObject);
		}
	}
}