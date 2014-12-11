using UnityEngine;
using System.Collections.Generic;

public abstract class SP_Bin
{
	protected int width;
	protected int height;
	
	public int Width
	{
		get
		{
			return width;
		}
	}
	
	public int Height
	{
		get
		{
			return height;
		}
	}
	
	public SP_Bin(int newWidth, int newHeight)
	{
		width  = newWidth;
		height = newHeight;
	}
	
	public abstract bool Pack(List<SP_Rect> rects);
}