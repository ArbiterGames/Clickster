using UnityEngine;
using System.Collections;
using System.Linq;

public class ClickArenaManager : MonoBehaviour {
	
	static int buttonHeight = 40;
	static int padding = 10;
	static int buttonWidth = 200;
	static int xPos = (Screen.width - buttonWidth - padding) / 2;
	
	public int score = 0;
	float sec = 0.0f;
	float timeRemaining = 10.0f;
	
	
	void Update () 
	{   
		if ( timeRemaining > 0 ) {
			RunTimer();
		}
	}
	
	
	void OnGUI() {
		
		GUIStyle buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = 32;
		GUIStyle labelStyle = new GUIStyle("label");
		labelStyle.fontSize = 18;
		labelStyle.alignment = TextAnchor.MiddleCenter;
		
		GUIStyle titleStyle = new GUIStyle("label");
		titleStyle.fontSize = 32;
		titleStyle.fontStyle = FontStyle.BoldAndItalic;
		titleStyle.normal.textColor = Color.red;
		titleStyle.alignment = TextAnchor.MiddleCenter;
		
		GUI.Label(new Rect(xPos, padding, buttonWidth, 100), "Punch the Bunny!", titleStyle);
		
		GUI.Label(new Rect(xPos, 140, buttonWidth, labelStyle.fontSize + 10), "Score:" + score, labelStyle);
		GUI.Label(new Rect(xPos, 140 + labelStyle.fontSize, buttonWidth, labelStyle.fontSize + 10), "Time remaining:" + timeRemaining, labelStyle);
		
		if ( timeRemaining == 0 ) {
			if(GUI.Button(new Rect(padding * 2, 600, buttonWidth, buttonHeight), "Back", buttonStyle)) {
				Application.LoadLevel("MainMenu");
			}
		}
	}
	
	void RunTimer() {
		sec += Time.deltaTime;
		if (sec >= 1) {
			timeRemaining -= Mathf.Floor(sec);
			sec = 0;	
		}
	}
	
	public void AddToScore() {
		if ( timeRemaining > 0 ) {
			score++;
		}
	}
}
