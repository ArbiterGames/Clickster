using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {
	
	private static int padding = 10;
	private static int buttonHeight = 40;
	private static int buttonWidth = Screen.width - padding * 2;
	
	void OnGUI() {
		GUIStyle titleStyle = new GUIStyle("label");
		titleStyle.fontSize = 32;
		titleStyle.fontStyle = FontStyle.BoldAndItalic;
		titleStyle.normal.textColor = Color.black;
		titleStyle.alignment = TextAnchor.MiddleCenter;
		
		GUIStyle buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = 32;
		
		GUI.Label(new Rect(padding, padding, buttonWidth, 80), "Clickster", titleStyle);
		
		if(GUI.Button(new Rect(padding, Screen.height - 60, buttonWidth, buttonHeight), "Play Game", buttonStyle)) {
			Application.LoadLevel("ClickArena");
		}
	}
}
