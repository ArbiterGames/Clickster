using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;

public class MainMenu : MonoBehaviour {
	
	private static int padding = 10;
	private static int buttonHeight = 80;
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
		
		if ( Arbiter.IsAuthenticated ) {
			if ( Arbiter.IsVerified ) {
				// TODO: Show the wallet button and the play for cash button
			} else {
				// TODO: Call Arbiter.Verify()
			}
		} else {
			
			// Arbiter Login Methods
			if ( GUI.Button(new Rect(padding, Screen.height - padding * 2 - buttonHeight * 2, buttonWidth, buttonHeight), "Login with Device", buttonStyle) ) {
				Arbiter.LoginAsAnonymous( LoginSuccessHandler, LoginErrorHandler );
			}
			
			
			if ( GUI.Button(new Rect(padding, Screen.height - padding * 3 - buttonHeight * 3, buttonWidth, buttonHeight), "Login with Game Center", buttonStyle) ) {
				Action<bool> processAuth = ( success ) => {
					if( success ) {
						Arbiter.LoginWithGameCenter( LoginSuccessHandler, LoginErrorHandler );
					} else {
						Debug.LogError( "Could not authenticate to Game Center! Make Sure the user has not disabled Game Center on their device, or have them create an Arbiter Account." );
					}
				};
				Social.localUser.Authenticate( processAuth );
			}
		}
		
		if ( GUI.Button(new Rect(padding, Screen.height - buttonHeight - padding, buttonWidth, buttonHeight), "Play Game", buttonStyle) ) {
			Application.LoadLevel("ClickArena");
		}
	}
	
	void LoginSuccessHandler() {
		Debug.Log ("Successfully logged in to Arbiter");
	}
	
	void LoginErrorHandler( List <string>errors ) {
		errors.ForEach( error => Debug.Log( error ));
	}
}
