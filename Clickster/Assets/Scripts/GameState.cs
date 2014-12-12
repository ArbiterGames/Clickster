﻿using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
	
	public static GameState instance;
	public Arbiter.ScoreChallenge challenge;
	public bool practiceMode;
	
	void Awake() {
		if ( !instance ) {
			instance = this;
			DontDestroyOnLoad( gameObject );
		} else {
			Destroy( gameObject );
		}
	}
	
}
