using UnityEngine;
using System.Collections;

public class ClownMoveTwo : MonoBehaviour {
	public float speed = 1.0f;
	public Vector3 upPos;
	public Vector3 downPos;
	public float upTime;
	public float downTime;
	public float currentTime;
	public float randomStartTime;
	public float randomDownTime;
	public enum Status {UP,DOWN,MOVING};
	public Status myStatus;

	// Use this for initialization
	void Start () {
		upPos = new Vector3 (transform.position.x, transform.position.y + 6, transform.position.z);
		downPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		
		currentTime = Time.time;
		if(randomStartTime == 0)
			randomStartTime = Random.Range (0.1f, .5f);
		if(randomDownTime == 0)
			randomDownTime = Random.Range (0.5f, 2.0f);
		
		upTime += randomStartTime;
		downTime += randomDownTime;
		
		myStatus = Status.DOWN;
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;
		
		if (currentTime > upTime && myStatus == Status.UP)
			UpDown ();
		else if (currentTime > downTime && myStatus == Status.DOWN)
			UpDown ();
	}

	void UpDown(){
		switch(myStatus)
		{
			case Status.UP:
				StartCoroutine(MoveObject(this.transform, downPos));
				currentTime = 0;
				break;
				
			case Status.DOWN:
				StartCoroutine(MoveObject(this.transform, upPos));
				currentTime = 0;
				break;
				
			case Status.MOVING:
				currentTime = 0;
				break;
		}
	}
	
	IEnumerator MoveObject(Transform objectToMove, Vector3 moveTo)
	{
		this.transform.collider.enabled = false;
		transform.renderer.material.color = Color.white;
		myStatus = Status.MOVING;
		
		while(objectToMove.position != moveTo)
		{
			float step = speed * Time.deltaTime;
			objectToMove.position = Vector3.MoveTowards(objectToMove.position, moveTo, step);
			yield return null;
		}
		
		if(objectToMove.position == upPos)
		{
			myStatus = Status.UP;
			this.transform.collider.enabled = true;
		}
		else if(objectToMove.position == downPos)
		{
			myStatus = Status.DOWN;
		}
		
		currentTime = 0;
	}
	
	void OnMouseDown()
	{
		transform.renderer.material.color = Color.red;
		GameManager.gameManager.score += 1;
		GameManager.gameManager.hits += 1;
		this.transform.collider.enabled = false;
		Debug.Log ("I was Hit");
	}
}
