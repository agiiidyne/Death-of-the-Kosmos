using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    // player controller
	public Transform player;
	
	// boat specs
	public float speed = 20f;
	public float speedRate = 20f;
	public bool boatActive;
	bool isInTransition;
	
	// boat interactions
	public Transform seatPoint;
	public Vector3 sittingOffset;
	public Transform exitPoint;
	public float transitionSpeed = 0.2f;
	
	void Update()
	{	
		// boat interactions
		if(boatActive && isInTransition)
		{
			Exit();
		}
		else if(!boatActive && isInTransition)
		{
			Enter();
		}
		
		if(Input.GetKeyDown(KeyCode.Q))
		{
			isInTransition = true;
		}
		
		// boat movement
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
	}
	
	void Enter()
	{
		//disable components
		player.GetComponentInChildren<CapsuleCollider>().enabled = false;
		player.GetComponentInChildren<Rigidbody>().useGravity = false;
		
		// move player to seat
		player.position = Vector3.Lerp(player.position, seatPoint.position + sittingOffset, transitionSpeed);
		player.rotation = Quaternion.Slerp(player.rotation, seatPoint.rotation, transitionSpeed);
		
		// animation into sitting will go here
		
		// checking reset
		if(player.position == seatPoint.position + sittingOffset)
		{
			isInTransition = false;
			boatActive = true;
		}
	}
	
	void Exit()
	{
		// move to exit point
		player.position = Vector3.Lerp(player.position, exitPoint.position, transitionSpeed);
		
		// animation to idle will go here
		
		// checking reset
		if(player.position == exitPoint.position)
		{
			isInTransition = false;
			boatActive = false;
		}
		
		// enable components
		player.GetComponentInChildren<CapsuleCollider>().enabled = true;
		player.GetComponentInChildren<Rigidbody>().useGravity = true;
	}
}
