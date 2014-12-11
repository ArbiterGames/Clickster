using UnityEngine;

[System.Serializable]
public class SP_Result
{
	[SerializeField]
	private string identifier;
	
	[SerializeField]
	private string name;
	
	[SerializeField]
	private int index;
	
	[SerializeField]
	private Sprite sprite;
	
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
	
	public Sprite Sprite
	{
		set
		{
			sprite = value;
		}
		
		get
		{
			return sprite;
		}
	}
}