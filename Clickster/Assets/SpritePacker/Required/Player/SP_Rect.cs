using UnityEngine;

public class SP_Rect
{
	private string        identifier;
	private int           index;
	private string        name;
	private int           width;
	private int           height;
	private int           x;
	private int           y;
	private bool          keepPivot;
	private Vector2       pivot;
	private Vector4       border;
	private SP_Pixels     pixels;
	private SP_BorderType borderType;
	
	public string Identifier
	{
		set
		{
			identifier = value;
		}
		
		get
		{
			return identifier;
		}
	}
	
	public int Index
	{
		set
		{
			index = value;
		}
		
		get
		{
			return index;
		}
	}
	
	public string Name
	{
		set
		{
			name = value;
		}
		
		get
		{
			return name;
		}
	}
	
	public int Width
	{
		set
		{
			width = value;
		}
		
		get
		{
			return width;
		}
	}
	
	public int Height
	{
		set
		{
			height = value;
		}
		
		get
		{
			return height;
		}
	}
	
	public int X
	{
		set
		{
			x = value;
		}
		
		get
		{
			return x;
		}
	}
	
	public int Y
	{
		set
		{
			y = value;
		}
		
		get
		{
			return y;
		}
	}
	
	public bool KeepPivot
	{
		set
		{
			keepPivot = value;
		}
		
		get
		{
			return keepPivot;
		}
	}
	
	public Vector2 Pivot
	{
		set
		{
			pivot = value;
		}
		
		get
		{
			return pivot;
		}
	}
	
	public Vector4 Border
	{
		set
		{
			border = value;
		}
		
		get
		{
			return border;
		}
	}
	
	public int R
	{
		get
		{
			return x + width;
		}
	}
	
	public int T
	{
		get
		{
			return y + height;
		}
	}
	
	public int BorderX
	{
		get
		{
			return (width  - pixels.Width) / 2;
		}
	}
	
	public int BorderY
	{
		get
		{
			return (height  - pixels.Height) / 2;
		}
	}
	
	public SP_Pixels Pixels
	{
		set
		{
			pixels = value;
		}
		
		get
		{
			return pixels;
		}
	}
	
	public SP_BorderType BorderType
	{
		set
		{
			borderType = value;
		}
		
		get
		{
			return borderType;
		}
	}
	
	public SP_Pixels ExpandedPixels
	{
		get
		{
			var l        = BorderX;
			var b        = BorderY;
			var expanded = new SP_Pixels(width, height);
			
			for (var x = 0; x < width; x++)
			{
				for (var y = 0; y < height; y++)
				{
					var z = x - l;
					var w = y - b;
					var c = default(Color32);
					
					switch (borderType)
					{
						case SP_BorderType.Transparent: c = pixels.GetPixelTransparent(z, w); break;
						case SP_BorderType.Clamp:       c = pixels.GetPixelClamp(z, w);       break;
						case SP_BorderType.Repeat:      c = pixels.GetPixelRepeat(z, w);      break;
					}
					
					expanded.SetPixel(x, y, c);
				}
			}
			
			return expanded;
		}
	}
}