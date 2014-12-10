using UnityEngine;
using System.Collections;
using System.Linq;

public class ClickArena : MonoBehaviour {
	
	static int padding = 10;
	static int buttonHeight = 100;
	static int boxWidth = Screen.width - padding * 2;
	static int boxHeight = buttonHeight * 2 + padding * 3;
	static int boxY = (Screen.height - boxHeight) / 2;
	static int buttonWidth = boxWidth - padding * 2;
	
	int score = 0;
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
		GUIStyle boxStyle = new GUIStyle("box");
		boxStyle.fontSize = 38;
		GUIStyle labelStyle = new GUIStyle("label");
		labelStyle.fontSize = 18;
		labelStyle.alignment = TextAnchor.MiddleCenter;
		
		GUI.Box(new Rect(padding, boxY, boxWidth, boxHeight), "The Game", boxStyle);
		GUI.Label(new Rect(0, boxY - padding - labelStyle.fontSize, buttonWidth, buttonHeight), "Score:" + score, labelStyle);
		GUI.Label(new Rect(0, boxY - padding - labelStyle.fontSize * 2, buttonWidth, buttonHeight), "Time remaining:" + timeRemaining, labelStyle);
		
		if ( timeRemaining > 0 ) {
			if(GUI.Button(new Rect(padding * 2, buttonHeight + boxY + padding, buttonWidth, buttonHeight), "click!!!", buttonStyle)) {
				score++;
			}
		}
		
		if ( timeRemaining == 0 ) {
			if(GUI.Button(new Rect(padding * 2, buttonHeight * 2 + boxY + padding, buttonWidth, buttonHeight), "Back", buttonStyle)) {
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
}
