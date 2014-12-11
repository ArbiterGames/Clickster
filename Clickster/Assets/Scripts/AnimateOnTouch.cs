using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimateOnTouch : MonoBehaviour
{
	
	public float Interval = 0.25f;
	public int Index;
	public float Timer;
	public List<Sprite> SpriteFrames = new List<Sprite>();
	private SpriteRenderer spriteRenderer;
	private bool shouldAnimate = false;
	public GameObject arenaManagerGO;
	private ClickArenaManager arenaManager;
	
	protected virtual void Awake()
	{
		arenaManager = arenaManagerGO.GetComponent<ClickArenaManager>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	public void OnMouseDown() {
		if ( arenaManager.gameOver == false ) {
			shouldAnimate = true;
		}
	}
	
	protected virtual void Update()
	{
		if ( shouldAnimate ) {
			Timer -= Time.deltaTime;
			
			if (Timer <= 0.0f) {
				Timer = Interval;
				Index = (Index + 1) % SpriteFrames.Count;
				if ( Index == SpriteFrames.Count - 1 ) {
					shouldAnimate = false;
				} 
				spriteRenderer.sprite = SpriteFrames[Index];
			}
		} else if ( arenaManager.gameOver ) {
			spriteRenderer.sprite = SpriteFrames[3];
		}
	}
}
