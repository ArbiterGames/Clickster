using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
[AddComponentMenu("")]
public partial class SP_SpritePacker : MonoBehaviour
{
	#pragma warning disable 414
	[SerializeField]
	private List<SP_Source> sources = new List<SP_Source>();
	
	#pragma warning disable 414
	[SerializeField]
	private List<SP_Result> results = new List<SP_Result>();
	
	[SerializeField]
	private SP_Algorithm algorithm = SP_Algorithm.MaxRects;
	
	[SerializeField]
	private SP_PowerOfTwo maxSize = SP_PowerOfTwo.Power12;
	
	[SerializeField]
	private bool forceSquare = false;
	
	[SerializeField]
	private bool suffixAtlasName = true;
	
	[SerializeField]
	private bool autoRebuild = true;
	
	[SerializeField]
	private bool defaultTrim = true;
	
	[SerializeField]
	private bool defaultKeepPivot = true;
	
	[SerializeField]
	private int defaultBorderSize = 1;
	
	[SerializeField]
	private SP_BorderType defaultBorderType = SP_BorderType.Clamp;
	
	[SerializeField]
	private string identifier;
	
	public SP_Algorithm Algorithm
	{
		set
		{
			algorithm = value;
		}
		
		get
		{
			return algorithm;
		}
	}
	
	public SP_PowerOfTwo MaxSize
	{
		set
		{
			maxSize = value;
		}
		
		get
		{
			return maxSize;
		}
	}
	
	public bool ForceSquare
	{
		set
		{
			forceSquare = value;
		}
		
		get
		{
			return forceSquare;
		}
	}
	
	public bool SuffixAtlasName
	{
		set
		{
			suffixAtlasName = value;
		}
		
		get
		{
			return suffixAtlasName;
		}
	}
	
	public bool AutoRebuild
	{
		set
		{
			autoRebuild = value;
		}
		
		get
		{
			return autoRebuild;
		}
	}
	
	public bool DefaultTrim
	{
		set
		{
			defaultTrim = value;
		}
		
		get
		{
			return defaultTrim;
		}
	}
	
	public bool DefaultKeepPivot
	{
		set
		{
			defaultKeepPivot = value;
		}
		
		get
		{
			return defaultKeepPivot;
		}
	}
	
	public int DefaultBorderSize
	{
		set
		{
			defaultBorderSize = Mathf.Max(value, 0);
		}
		
		get
		{
			return defaultBorderSize;
		}
	}
	
	public SP_BorderType DefaultBorderType
	{
		set
		{
			defaultBorderType = value;
		}
		
		get
		{
			return defaultBorderType;
		}
	}
	
	protected virtual void Start()
	{
		Debug.LogError("Don't add sprite packers to your scene!");
		
		SP_Helper.SafeDestroy(gameObject);
	}
}