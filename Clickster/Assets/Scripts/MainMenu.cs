using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {
	
	private static int padding = 10;
	private static int buttonHeight = 100;
	private static int buttonWidth = Screen.width - padding * 2;
	
	void OnGUI() {
		
		GUIStyle buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = 32;
		
		
		if(GUI.Button(new Rect(padding, buttonHeight * 2 + padding, buttonWidth, buttonHeight), "Play Game", buttonStyle)) {
			Application.LoadLevel("ClickArena");
		}
	}
}
