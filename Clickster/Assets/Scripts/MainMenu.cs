using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;

public class MainMenu : MonoBehaviour {
	
	// uiEnabled handles blocking user input during network requests
	////////////////////////////////////////////////////////////////////////
	bool uiEnabled = true;
	
	// The GameState object stores whether or not the user is playing a Cash Challenge
	// or just a practice game. The ClickArenaManager references the game state and does
	// things like update the UI and report to Arbiter if the user is playing a Cash Challenge
	////////////////////////////////////////////////////////////////////////////////////////////////
	static GameState gameState;
	
	// Filters can be passed into the Cash Challenge request to segment different modes or levels or you game
	Dictionary<string,string> filters;
	
	
	void Awake() {
		
		if ( gameState == null ) {
			GameObject go = new GameObject( "GameState" );
			go.AddComponent<GameState>();
			gameState = go.GetComponent<GameState>();
		}
		
		gameState.practiceMode = true;
		filters = new Dictionary<string,string>();
		filters.Add("level", "2");
		
		// Make sure we have the latest Cash Challenge 
		// whenever a logged in user hits the main menu
		////////////////////////////////////////////////
		if ( Arbiter.IsAuthenticated ) {
			Arbiter.RequestCashChallenge( filters, OnCashChallengeCreated, OnCashChallengeError );
		}
	}
	
	void OnGUI() {
	
		
		// Define text and button formatting
		////////////////////////////////////////
		int padding = 10;
		int buttonHeight = 80;
		int textHeight = 40;
		int buttonWidth = Screen.width - padding * 2;
		GUIStyle titleStyle = new GUIStyle("label");
		titleStyle.fontSize = 48;
		titleStyle.fontStyle = FontStyle.BoldAndItalic;
		titleStyle.normal.textColor = Color.red;
		titleStyle.alignment = TextAnchor.MiddleCenter;
		GUIStyle buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = 32;
		buttonStyle.fontStyle = FontStyle.Bold;
		GUIStyle defaultTextStyle = new GUIStyle("label");
		defaultTextStyle.fontSize = 32;
		defaultTextStyle.normal.textColor = Color.black;
		defaultTextStyle.fontStyle = FontStyle.Bold;
		defaultTextStyle.alignment = TextAnchor.MiddleCenter;
		
		
		// Render title and practice button
		////////////////////////////////////
		GUI.Label(new Rect(padding, padding, buttonWidth, 80), "Clickster", titleStyle);
		
		if ( GUI.Button(new Rect(padding, 400, buttonWidth, buttonHeight), "Practice", buttonStyle) ) {
			gameState.challenge = null;
			Application.LoadLevel("ClickArena");
		}
		
		
		// Arbiter Logic
		////////////////////////////////////
		if ( uiEnabled ) {
		
			// If logged into Arbiter, then create and display a Cash Challenge
			if ( Arbiter.IsAuthenticated ) {
			
				// Opens the Wallet Dashboard for the user to purchase and cashout their credits
				/////////////////////////////////////////////////////////////////////////////////
				if ( GUI.Button(new Rect(padding, Screen.height - padding * 2 - buttonHeight * 2, buttonWidth, buttonHeight), "View Arbiter Wallet", buttonStyle) ) {
					Arbiter.DisplayWalletDashboard( OnWalletClosed );
				}
				
				// Logout button destroys the current session. If the player authenticated with  
				// their device (vs using Game Center), they will lose access to their wallet
				/////////////////////////////////////////////////////////////////////////////////
				if ( GUI.Button(new Rect(padding, Screen.height - padding - buttonHeight, buttonWidth, buttonHeight), "Logout", buttonStyle) ) {
					uiEnabled = false;
					Arbiter.Logout( LogoutSuccessHandler, DefaultErrorHandler );
				}
				
				// Cash Challenge Details and Entry Button
				////////////////////////////////////////////
				if ( gameState.challenge != null ) {
					GUI.Label(new Rect(padding, 400 + buttonHeight * 2, buttonWidth, buttonHeight), "Cash Challenge", titleStyle);
					GUI.Label(new Rect(padding, 400 + buttonHeight * 3, buttonWidth, buttonHeight), "Score to beat: " + gameState.challenge.ScoreToBeat, defaultTextStyle);
					GUI.Label(new Rect(padding, 400 + buttonHeight * 3 + textHeight, buttonWidth, buttonHeight), "Entry fee: " + gameState.challenge.EntryFee, defaultTextStyle);
					GUI.Label(new Rect(padding,  400 + buttonHeight * 4, buttonWidth, buttonHeight), "Prize: " + gameState.challenge.Prize, defaultTextStyle);
					if ( GUI.Button(new Rect(padding, 400 + buttonHeight * 5, buttonWidth, buttonHeight), "Enter Cash Challenge", buttonStyle) ) {
						Arbiter.AcceptCashChallenge( gameState.challenge.Id, OnCashChallengeAccepted, OnCashChallengeError );
					}
				}
				
			// If not logged into Arbiter, then display the login options
			} else {
				
				// Authenticates with Token stored on the device
				////////////////////////////////////////////////
				if ( GUI.Button(new Rect(padding, Screen.height - padding - buttonHeight, buttonWidth, buttonHeight), "Login with Device", buttonStyle) ) {
					uiEnabled = false;
					Arbiter.LoginWithDeviceId( LoginSuccessHandler, DefaultErrorHandler );
				}
				
				// Uses Unity's social plugin to authenticate with Apple Game Center
				// and then uses that Game Center account to authenticate with Aribter
				//////////////////////////////////////////////////////////////////////
				if ( GUI.Button(new Rect(padding, Screen.height - padding * 2 - buttonHeight * 2, buttonWidth, buttonHeight), "Login with Game Center", buttonStyle) ) {
					uiEnabled = false;
					Action<bool> processAuth = ( success ) => {
						if( success ) {
							Arbiter.LoginWithGameCenter( LoginSuccessHandler, DefaultErrorHandler );
						} else {
							Debug.LogError( "Could not authenticate to Game Center! Make Sure the user has not disabled Game Center on their device, or have them create an Arbiter Account." );
						}
					};
					Social.localUser.Authenticate( processAuth );
				}
			}
		}
	}
	
	
	// Arbiter Callback Handlers
	////////////////////////////
	
	
	// Once a user successfully logs in, make sure they have agreed to the Arbiter ToS and allowed 
	// Location Services so that Arbiter can verify they are in a state that Arbiter operates within
	///////////////////////////////////////////////////////////////////////////////////////////////
	void LoginSuccessHandler() {
		if ( Arbiter.IsVerified == false ) {
			Arbiter.VerifyUser( VerificationHandler, DefaultErrorHandler );	
		} else {
			uiEnabled = true;
			if ( gameState.challenge == null ) {
				Arbiter.RequestCashChallenge( filters, OnCashChallengeCreated, OnCashChallengeError );
			}
		}
	}
	
	// Logs the Verfication outcome
	/////////////////////////////////
	void VerificationHandler() {
		uiEnabled = true;
		if ( !Arbiter.LocationApproved ) {
			Debug.Log ("Issue in Verification.cs: The user's location is not approved for cash challenges in this game.");
		}  
		if ( !Arbiter.AgreedToTerms ) {
			Debug.Log ("Issue in Verification.cs: The user did not agree to the terms and conditions.");
		}
	}
	
	void LogoutSuccessHandler() {
		uiEnabled = true;
		gameState.challenge = null;
	}
	
	void OnWalletClosed() {
		Debug.Log("Wallet Closed");
	}
	
	// Once a Cash Challenge has been created, set that challenge to 
	// the GameState that gets passed along to the ClickArenaManager
	/////////////////////////////////////////////////////////////////
	void OnCashChallengeCreated( Arbiter.CashChallenge challenge ) {
		gameState.challenge = challenge;
	}
	
	// Once a Cash Challenge has been successfully 
	// accepted, send the user to the ClickArena!
	////////////////////////////////////////////// 
	void OnCashChallengeAccepted() {
		gameState.practiceMode = false;
		Application.LoadLevel("ClickArena");
	}
	
	// Logs errors to console
	////////////////////////////
	void DefaultErrorHandler( List<string> errors ) {
		uiEnabled = true;
		errors.ForEach( error => Debug.Log( error ));
	}
	
	// Returns both developer errors and messages
	// that can be based to the UI for the user
	////////////////////////////////////////////////
	void OnCashChallengeError( List<string> errors, List<string> messages ) {
		errors.ForEach( error => Debug.Log( error ));
		messages.ForEach( error => Debug.Log( error ));
	}
}
