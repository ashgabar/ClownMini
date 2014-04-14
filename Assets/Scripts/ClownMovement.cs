using UnityEngine;
using System.Collections;

public class ClownMovement : MonoBehaviour 
{
	public Vector3 upPos;
	public Vector3 downPos;
	public float upTime;
	public float downTime;
	public float currentTime;
	public float randomStartTime;
	public float randomDownTime;
	public enum Status {UP,DOWN,MOVING};
	public Status myStatus;

	public float speed = 100.0f;

	void Start () 
	{


		upPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		downPos = new Vector3 (transform.position.x, transform.position.y -6, transform.position.z);

		currentTime = Time.time;
		if(randomStartTime == 0)
			randomStartTime = Random.Range (0.1f, 0.5f);
		if(randomDownTime == 0)
			randomDownTime = Random.Range (0.5f,2.0f);

		upTime += randomStartTime;
		downTime += randomDownTime;

		myStatus = Status.UP;

	}
	
	// Update is called once per frame
	void Update () 
	{	
		currentTime += Time.deltaTime;

		if (currentTime > upTime && myStatus == Status.UP)
						updown ();
		else if (currentTime > downTime && myStatus == Status.DOWN)
						updown ();

	}

	IEnumerator MoveObject(Transform objectToMove,Vector3 position,float time)
	{
		float t = 0f; // timer var used in while loop not reseting the incoming param.
		Vector3 originalPosition = objectToMove.position;
		myStatus = Status.MOVING;
		this.transform.collider.enabled = false;
		transform.renderer.material.color = Color.white;
		while (t < time) 
		{
			//float step = speed * Time.deltaTime;
			//objectToMove.position = Vector3.MoveTowards(originalPosition, position, step);
			objectToMove.position = Vector3.Lerp (originalPosition,position,t);
			t += Time.deltaTime / time;
			yield return null;
		}

		if (position == upPos)
		{
			myStatus = Status.UP;
			this.transform.collider.enabled = true;
		}
		else if (position == downPos)
		{
			myStatus = Status.DOWN;

		}

	}


	void OnMouseDown()
	{
		transform.renderer.material.color = Color.red;
		GameManager.gameManager.score += 1;
		GameManager.gameManager.hits += 1;
		this.transform.collider.enabled = false;
		Debug.Log ("I was Hit");
	}


	void updown()
	{

		switch (myStatus) 
		{
		case Status.UP:
			StartCoroutine(MoveObject (this.transform,downPos,2f));
			currentTime = 0;
			break;
			
		case Status.DOWN:
			StartCoroutine(MoveObject(this.transform,upPos,2f));
			currentTime = 0;
			break;
			
		case Status.MOVING:
			currentTime = 0;
			break;
			
		default:
			break;

		}
	}

}
