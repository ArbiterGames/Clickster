using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {
	
	private static int padding = 10;
	private static int buttonHeight = 100;
	private static int boxWidth = Screen.width - padding * 2;
	private static int boxHeight = buttonHeight * 6 + padding * 5;
	private static int boxY = (Screen.height - boxHeight) / 2;
	private static int buttonWidth = boxWidth - padding * 2;
	
	void OnGUI() {
		
		GUIStyle buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = 32;
		GUIStyle boxStyle = new GUIStyle("box");
		boxStyle.fontSize = 38;
		
		GUI.Box(new Rect(padding, boxY, boxWidth, boxHeight), "Main Menu", boxStyle);
		
		
		if(GUI.Button(new Rect(padding * 2, buttonHeight * 2 + padding + boxY, buttonWidth, buttonHeight), "Play Game", buttonStyle)) {
			Application.LoadLevel("ClickArena");
		}
	}
}
