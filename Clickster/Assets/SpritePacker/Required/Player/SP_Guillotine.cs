using UnityEngine;
using System.Collections.Generic;

public class SP_Guillotine : SP_Bin
{
	private int                 x;
	private int                 y;
	private List<SP_Guillotine> children = new List<SP_Guillotine>();
	
	public SP_Guillotine(int newWidth, int newHeight) : base(newWidth, newHeight)
	{
		children.Add(new SP_Guillotine(0, 0, newWidth, newHeight));
	}
	
	private SP_Guillotine(int newX, int newY, int newWidth, int newHeight) : base(newWidth, newHeight)
	{
		x = newX;
		y = newY;
	}
	
	public override bool Pack(List<SP_Rect> rects)
	{
		rects.Sort((a, b) => Mathf.Max(b.Width, b.Height) - Mathf.Max(a.Width, a.Height));
		
		foreach (var rect in rects)
		{
			if (Pack(rect) == false)
			{
				return false;
			}
		}
		
		return true;
	}
	
	private bool Pack(SP_Rect rect)
	{
		foreach (var child in children)
		{
			var gapX = child.width  - rect.Width;
			var gapY = child.height - rect.Height;
			
			if (gapX >= 0 && gapY >= 0)
			{
				rect.X = child.x;
				rect.Y = child.y;
				
				children.Remove(child);
				
				if (gapX > 0) children.Add(new SP_Guillotine(child.x + rect.Width, child.y              , gapX       , rect.Height));
				if (gapY > 0) children.Add(new SP_Guillotine(child.x             , child.y + rect.Height, child.width, gapY       ));
				
				return true;
			}
		}
		
		return false;
	}
}