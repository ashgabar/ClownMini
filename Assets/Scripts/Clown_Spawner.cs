using UnityEngine;
using System.Collections;

public class Clown_Spawner : MonoBehaviour 
{

	public GameObject clown;
	public int numClowns;
	public GameObject[] spawns;

	public int coinFlip;



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

		int myspawns = 1;

		for(int i =0; i < numClowns; i ++)
		{


			GameObject myclone = (GameObject)Instantiate (clown, spawns[i].transform.position, Quaternion.identity);
			myclone.SetActive (false);
			coinFlip = Random.Range (0,2);
			Debug.Log ("coin Flip " + coinFlip);

			if(coinFlip == 0)
			{
				myclone.SetActive(false);
			}

			else if(coinFlip == 1 && myspawns <= GameManager.gameManager.spawnPerLevel)
			{
				myclone.SetActive (true);
				Debug.Log("myspawn" + myspawns);
				Debug.Log ("spawn per level" + GameManager.gameManager.spawnPerLevel);
				myspawns ++;
			}

		}




		GameManager.gameManager.numClowns = numClowns;
	}
	
}
