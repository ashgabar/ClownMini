using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager gameManager;

	public float hits;
	public float score;
	public float numClowns;
	public float gameTime;
	public int currentState;
	public string wonLost;

	//defines for game states
	//will these be needed outside the gameManager class? they could be defined in a header file
	const int MENUSTATE = 0;
	const int GAMESTATE = 1;
	const int GAMEWON = 2;
	const int GAMELOST = 3;

	/***********
	 * Awake function, called when the script is loaded
	 * 
	 * Used to initialize the gameManager and other game objects.
	 * 
	 * Written by Chris
	 **********/
	void Awake()
	{
		if (gameManager == null)
		{
			DontDestroyOnLoad (gameObject);
			gameManager = this;
		}
		else if (gameManager != this) 
		{
			Destroy (gameObject);
		}

		currentState = GAMESTATE; //this should be changed to MENUSTATE when the menu is implemented
		hits = 0;
		score = 0;
		numClowns = 0;
		gameTime = Time.time;
	}

	/***********
	 * Update function, called every frame.
	 * 
	 * Used to update the game time.
	 * 
	 * Written by Chris and Callum
	 **********/
	void Update()
	{
		//gameTime += Time.deltaTime;

		/*if (gameTime >= 30.0) {
			currentState = GAMEWON;
		}*/

		switch (currentState) 
		{
			case MENUSTATE:
				//things?
				break;

			case GAMESTATE:
				//things?
				gameTime += Time.deltaTime;

				if (gameTime >= 30.0) 
				{
					if(score <= 15)
					{
						currentState = GAMEWON;
						wonLost = "You lost!";
					}
					else
					{
						currentState = GAMELOST;
						wonLost = "You won!";
					}
				}
				break;

			case GAMEWON:
				//display stats?
				//reset stats and time to 0?
				//option to move on to next level
				break;

			case GAMELOST:
				//display stats?
				//reset stats and time to 0?
				//option to move on to retry level
				break;
		}

	}

	/***********
	 * OnGUI function, renders the UI and handles UI events.
	 * 
	 * Used to display game time and score related numbers.
	 * 
	 * Written by Chris
	 **********/
	void OnGUI()
	{
		GUI.Label (new Rect (10, 10, 100, 40), "Score: " + score);
		GUI.Label (new Rect (10, 50, 100, 40), "Hits: " + hits);
		GUI.Label (new Rect (10, 90, 100, 40), "Clowns: " + numClowns);
		GUI.Label (new Rect (10, 130, 100, 40), "Time: " + gameTime);
		GUI.Label (new Rect (10, 170, 100, 40), "GameState: " + currentState);
		GUI.Label (new Rect (500, 200, 100, 40), wonLost);
	}
}
