using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


// Source: https://www.youtube.com/watch?v=_QajrabyTJc
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] CharacterController controller;
	[Header("Player Stats")]
	[SerializeField] float speed = 12f;
	[SerializeField] float baseSpeed = 12f;
	[SerializeField] float sprintSpeed;
	[SerializeField] float gravity = -9.81f * 2;
	[SerializeField] float jumpHeight = 3f;
	[SerializeField] float spritMultiplier = 1.75f;
	private bool isSprinting = true;
	Vector3 velocity;
	Vector3 previousPosition;

	// Ground Check
	public Transform groundCheck;
	public LayerMask groundMask;
	private float groundDistance = 0.4f;
	private bool isGrounded;

	// Crouching
	private bool isCrouching = false;
	[SerializeField] float crouchMultiplier = 0.75f;
	[SerializeField] float crouchSpeed;

    // Update is called once per frame
    void Update()
    {
		if (PauseMenu.GameIsPaused) return;
		
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		if(isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

        float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");


		if (isSprinting && !isCrouching)
		{
			speed = baseSpeed;
			isSprinting = false;
		} 
		else if (!Input.GetKey(KeyCode.LeftShift) && !isCrouching)
		{
			sprintSpeed = baseSpeed * spritMultiplier;
			speed = sprintSpeed;
			isSprinting = true;
		}

		Vector3 move = transform.right * x + transform.forward * z;

		controller.Move(move * speed * Time.deltaTime);

		if(Input.GetButtonDown("Jump") && isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}
		
		if(Input.GetKeyDown(KeyCode.LeftControl) & !isCrouching)
		{
			crouchSpeed = baseSpeed * crouchMultiplier;
			controller.height *= 0.75f;
			speed = crouchSpeed;
			isCrouching = true;
		}
		else if (Input.GetKeyDown(KeyCode.LeftControl) & isCrouching)
		{
			controller.height /= 0.75f;
			speed = baseSpeed;
			isCrouching = false;
		}

		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}

