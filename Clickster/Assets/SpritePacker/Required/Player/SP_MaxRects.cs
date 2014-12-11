using UnityEngine;
using System.Collections.Generic;

public class SP_MaxRects : SP_Bin
{
	private int               x;
	private int               y;
	private List<SP_MaxRects> children = new List<SP_MaxRects>();
	
	private int R
	{
		get
		{
			return x + width;
		}
	}
	
	private int T
	{
		get
		{
			return y + height;
		}
	}
	
	public SP_MaxRects(int newWidth, int newHeight) : base(newWidth, newHeight)
	{
		children.Add(new SP_MaxRects(0, 0, newWidth, newHeight));
	}
	
	private SP_MaxRects(int newX, int newY, int newWidth, int newHeight) : base(newWidth, newHeight)
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
				
				if (gapX > 0) children.Add(new SP_MaxRects(child.x + rect.Width, child.y              , gapX       , child.height));
				if (gapY > 0) children.Add(new SP_MaxRects(child.x             , child.y + rect.Height, child.width, gapY        ));
				
				Subdivide(rect);
				
				return true;
			}
		}
		
		return false;
	}
	
	private static List<SP_MaxRects> newChildrenA = new List<SP_MaxRects>();
	private static List<SP_MaxRects> newChildrenB = new List<SP_MaxRects>();
	
	private void Subdivide(SP_Rect rect)
	{
		newChildrenA.Clear();
		newChildrenB.Clear();
		
		foreach (var child in children)
		{
			if (rect.X >= child.R || rect.Y >= child.T || rect.R <  child.x || rect.T <  child.y)
			{
				newChildrenA.Add(child); continue;
			}
			
			var gapL = rect.X  - child.x;
			var gapB = rect.Y  - child.y;
			var gapR = child.R - rect.R;
			var gapT = child.T - rect.T;
			
			if (gapL > 0) newChildrenB.Add(new SP_MaxRects(child.x       , child.y       , gapL       , child.height));
			if (gapB > 0) newChildrenB.Add(new SP_MaxRects(child.x       , child.y       , child.width, gapB        ));
			if (gapR > 0) newChildrenB.Add(new SP_MaxRects(child.R - gapR, child.y       , gapR       , child.height));
			if (gapT > 0) newChildrenB.Add(new SP_MaxRects(child.x       , child.T - gapT, child.width, gapT        ));
		}
		
		children.Clear();
		children.AddRange(newChildrenA);
		children.AddRange(newChildrenB);
		
		for (var i = children.Count - 1; i >= 0; i--)
		{
			var child = children[i];
			
			foreach (var childB in children)
			{
				if (child != childB)
				{
					if (child.x >= childB.x && child.y >= childB.y && child.R <= childB.R && child.T <= childB.T)
					{
						children.Remove(child); break;
					}
				}
			}
		}
	}
}