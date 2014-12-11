using UnityEngine;
using System.Collections;
using System.Linq;

public class ClickArenaManager : MonoBehaviour {
	
	private static int padding = 10;
	private static int buttonHeight = 40;
	private static int buttonWidth = Screen.width - padding * 2;
	
	public int score = 0;
	public bool gameOver = false;
	private float timeRemaining = 10.0f;
	float sec = 0.0f;
	
	
	void Update () 
	{   
		if ( timeRemaining > 0 ) {
			RunTimer();
		} else {
			gameOver = true;
		}
	}
	
	
	void OnGUI() {
		
		GUIStyle buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = 32;
		
		GUIStyle labelStyle = new GUIStyle("label");
		labelStyle.fontSize = 18;
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.normal.textColor = Color.black;
		labelStyle.alignment = TextAnchor.MiddleCenter;
		
		GUIStyle bunnyDialogue = new GUIStyle("label");
		bunnyDialogue.fontSize = 18;
		bunnyDialogue.fontStyle = FontStyle.Bold;
		bunnyDialogue.alignment = TextAnchor.MiddleCenter;
		
		GUIStyle titleStyle = new GUIStyle("label");
		titleStyle.fontSize = 32;
		titleStyle.fontStyle = FontStyle.BoldAndItalic;
		titleStyle.normal.textColor = Color.red;
		titleStyle.alignment = TextAnchor.MiddleCenter;
		
		GUI.Label(new Rect(padding, padding, buttonWidth, 100), "Punch the Bunny!", titleStyle);
		
		if ( gameOver ) {
			if(GUI.Button(new Rect(padding, Screen.height - 60, buttonWidth, buttonHeight), "Back", buttonStyle)) {
				Application.LoadLevel("MainMenu");
			}
			
			GUI.Label(new Rect(padding, 120, buttonWidth, 100), "" + score, titleStyle);
			GUI.Label(new Rect(padding, 180, buttonWidth, 100), "Please let me rest :(", bunnyDialogue);
		} else {
			GUI.Label(new Rect(padding, 140, buttonWidth, labelStyle.fontSize + 10), "Score:" + score, labelStyle);
			GUI.Label(new Rect(padding, 140 + labelStyle.fontSize, buttonWidth, labelStyle.fontSize + 10), "Time remaining:" + timeRemaining, labelStyle);
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
		if ( gameOver == false ) {
			score++;
		}
	}
}
