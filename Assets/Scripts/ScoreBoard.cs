﻿using UnityEngine;
using System.Collections;

public class ScoreBoard : MonoBehaviour {

	public int score;
	// Use this for initialization
	void Start () 
	{
		score = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	void OnGUI()
	{
		GUILayout.Label ("Score:" + score);
	}
}
