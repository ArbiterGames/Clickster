using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ClickArenaManager : MonoBehaviour {
	
	public bool gameOver = false;
	
	int score = 0;
	bool scoreReported = false;
	bool outcomeReturned = false;
	float timeRemaining = 10.0f;
	float sec = 0.0f;
	
	static GameState gameState;
	
	void Awake() {
		GameObject go = GameObject.Find("GameState");
		gameState = go.GetComponent<GameState>();
	}
	
	// Runs down the timer then reports the score to 
	// Arbiter if this is not a practice round
	////////////////////////////////////////////////
	void Update() {   
		if ( timeRemaining > 0 ) {
			RunTimer();
		} else {
			gameOver = true;
			if ( gameState.practiceMode == false && scoreReported == false ) {
				scoreReported = true;
				Arbiter.ReportScoreForChallenge( gameState.challenge.Id, score.ToString(), OnReportScoreSuccess, OnReportScoreError );
			}
		}
	}
	
	
	void OnGUI() {
	
		// Define text and button formatting
		////////////////////////////////////////
		int padding = 10;
		int buttonHeight = 80;
		int buttonWidth = Screen.width - padding * 2;
		GUIStyle buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = 32;
		GUIStyle labelStyle = new GUIStyle("label");
		labelStyle.fontSize = 18;
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.normal.textColor = Color.black;
		labelStyle.alignment = TextAnchor.MiddleCenter;
		GUIStyle titleStyle = new GUIStyle("label");
		titleStyle.fontSize = 48;
		titleStyle.fontStyle = FontStyle.BoldAndItalic;
		titleStyle.normal.textColor = Color.red;
		titleStyle.alignment = TextAnchor.MiddleCenter;
		
		
		// Update the heading depending upon whether this is practice or for cash
		//////////////////////////////////////////////////////////////////////////
		if ( gameState.practiceMode ) {
			GUI.Label(new Rect(padding, padding, buttonWidth, 100), "Punch the Bunny!", titleStyle);
		} else {
			GUI.Label(new Rect(padding, padding, buttonWidth, 100), "Punch the Bunny " + gameState.challenge.ScoreToBeat + " times to win!", titleStyle);
		}
		
		
		// Update the UI based on whether or not the user is out of time
		////////////////////////////////////////////////////////////////
		if ( gameOver ) {
			GUI.Label(new Rect(padding, 120, buttonWidth, 100), "" + score, titleStyle);
			if(GUI.Button(new Rect(padding, Screen.height - buttonHeight - padding, buttonWidth, buttonHeight), "Back", buttonStyle)) {
				Application.LoadLevel("MainMenu");
			}
		} else {
			GUI.Label(new Rect(padding, 140, buttonWidth, labelStyle.fontSize + 10), "Score:" + score, labelStyle);
			GUI.Label(new Rect(padding, 140 + labelStyle.fontSize, buttonWidth, labelStyle.fontSize + 10), "Time remaining:" + timeRemaining, labelStyle);
		}
		
		// Update the UI once Arbiter has returned the outcome of the Cash Challenge
		////////////////////////////////////////////////////////////////////////////////
		if ( gameState.practiceMode == false && outcomeReturned ) {
			bool isWinner = IsWinner();
			if ( isWinner ) {
				GUI.Label(new Rect(padding, 180, buttonWidth, labelStyle.fontSize + 10), "You Won " + gameState.challenge.Prize + " credits!", labelStyle);
			} else {
				GUI.Label(new Rect(padding, 180, buttonWidth, labelStyle.fontSize + 10), "You Lost", labelStyle);
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
	
	bool IsWinner() {
		if ( gameState.challenge.Status == Arbiter.ScoreChallenge.StatusType.Closed ) {
			if ( gameState.challenge.Winner != null && gameState.challenge.Winner.Id == Arbiter.UserId ) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}
	
	void OnReportScoreSuccess( Arbiter.ScoreChallenge challenge ) {
		gameState.challenge = challenge;
		outcomeReturned = true;
	}
	
	void OnReportScoreError( List<string> errors, List<string> messages ) {
		errors.ForEach( error => Debug.Log( error ));
		messages.ForEach( error => Debug.Log( error ));
	}
	
	// Used by the ClickTrigger Game Object
	// to update the score on each click
	////////////////////////////////////////
	public void AddToScore() {
		if ( gameOver == false ) {
			score++;
		}
	}		
}
