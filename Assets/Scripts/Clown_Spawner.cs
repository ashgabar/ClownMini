using UnityEngine;
using System.Collections;

public class Clown_Spawner : MonoBehaviour 
{

	public GameObject clowns;
	public int numClowns;
	public GameObject[] spawns;



	/***********
	 * Start function, called when the script is loaded just before any update functions.
	 * 
	 * Used to instantiate the clowns and update the gameManager.numClowns.
	 * 
	 * Written by Chris
	 **********/
	void Start () 
	{
		spawns = GameObject.FindGameObjectsWithTag("Respawn");
		if (spawns.Length == 0) 
		{
			Debug.Log ("no objects with respawn tag");
		}

		for(int i =0; i < numClowns; i ++)
		{
			Instantiate (clowns, spawns[i].transform.position, Quaternion.identity);
		}
		GameManager.gameManager.numClowns = numClowns;
	}
	
}
