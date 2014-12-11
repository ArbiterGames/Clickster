using UnityEngine;
using System.Collections;

public class ClickTrigger : MonoBehaviour {
	
	public GameObject arenaManagerGO;
	private ClickArenaManager arenaManager;
	
	void Start() {
		arenaManager = arenaManagerGO.GetComponent<ClickArenaManager>();
	}
	
	void OnMouseDown() {
		arenaManager.AddToScore();
	}
}
