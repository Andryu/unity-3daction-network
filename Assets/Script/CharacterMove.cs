﻿using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour {

	const float GravityPower = 9.8f;
	const float StoppingDistance = 0.6f;
	Vector3 velocity = Vector3.zero;

	// Cache
	CharacterController characterController;
	
	public bool arrived = false;

	bool forceRotate = false;
	Vector3 forceRotateDirection;

	public Vector3 destination;

	public float walkSpeed = 6.0f;

	public float rotationSpeed = 360.0f;



	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
		destination = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (characterController.isGrounded){
			Vector3 destinationXZ = destination;

			// same chacter pos.y == destination.y
			destinationXZ.y = transform.position.y;

			// xz
			// Normalize
			Vector3 direction = (destinationXZ - transform.position).normalized;
			float distance = Vector3.Distance(transform.position, destinationXZ);

			Vector3 currentSpeed = velocity;
			if (arrived || distance < StoppingDistance )
				arrived = true;


			if (arrived) 
				velocity = Vector3.zero;
			else
				velocity = direction * walkSpeed;

			velocity = Vector3.Lerp(currentSpeed, velocity, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
			velocity.y = 0;

			if(!forceRotate) {
				if(velocity.magnitude > 0.1f && !arrived) {
					Quaternion characterTargetRotation = Quaternion.LookRotation(direction);
					transform.rotation = Quaternion.RotateTowards(
						transform.rotation, characterTargetRotation, rotationSpeed * Time.deltaTime);
				}
			}
		    else{
				// Force 
				Quaternion characterTargetRotation = Quaternion.LookRotation(forceRotateDirection);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, characterTargetRotation, rotationSpeed * Time.deltaTime);
			}

		}
       	// gravity
		velocity += Vector3.down * GravityPower * Time.deltaTime;

		// is Ground
		Vector3 snapGround = Vector3.zero;
		if (characterController.isGrounded) 
			snapGround = Vector3.down;

		characterController.Move(velocity * Time.deltaTime+snapGround);

		if(characterController.velocity.magnitude < 0.1f)
			arrived = true;

		// ForceRotate cold
		if(forceRotate && Vector3.Dot (transform.forward, forceRotateDirection) > 0.99f)
			forceRotate = false;
	}

	public void SetDestination(Vector3 destination)
	{
		arrived = false;
		this.destination = destination;
	}

	public void SetDirection(Vector3 direction)
	{
		forceRotateDirection = direction;
		forceRotateDirection.y = 0;
		forceRotateDirection.Normalize();
		forceRotate = true;
	}

	public void StopMove()
	{
		destination = transform.position;
	}

	public bool Arrived()
	{
		return arrived;
	}
}
