using UnityEngine;

[System.Serializable]
public partial class SP_Source
{
	[SerializeField]
	private string identifier;
	
	[SerializeField]
	private bool trim = true;
	
	[SerializeField]
	private bool keepPivot = true;
	
	[SerializeField]
	private int borderSize = 1;
	
	[SerializeField]
	private SP_BorderType borderType = SP_BorderType.Transparent;
	
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
	
	public bool Trim
	{
		set
		{
			trim = value;
		}
		
		get
		{
			return trim;
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
	
	public int BorderSize
	{
		set
		{
			borderSize = Mathf.Max(value, 0);
		}
		
		get
		{
			return borderSize;
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
}