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
	[SerializeField] string[] stepSounds;
	[SerializeField] float stepSoundsTime = 0.15f;

	// Sprinting
	private bool isSprinting = true;
	private Vector3 velocity;
	private Vector3 previousPosition;

	// Crouching
	private bool isCrouching = false;
	[SerializeField] float crouchMultiplier = 0.75f;
	[SerializeField] float crouchSpeed;

	// Ground Check
	public Transform groundCheck;
	public LayerMask groundMask;
	private float groundDistance = 0.4f;
	private bool isGrounded;
	bool movedLastFrame = false;

	int currentStepIndex = 0;

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

		if (!movedLastFrame && x + z >= 1 && isGrounded){
			movedLastFrame = true;
			StartCoroutine(PlaySound());
		}
		else if (x + z < 1 || !isGrounded) {
			movedLastFrame = false;
		}

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
			SFXManager.instance?.Play("Jump", 0.95f, 1.05f);
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

	IEnumerator PlaySound(){
		if (!isCrouching)
			SFXManager.instance.Play(stepSounds[currentStepIndex], 0.7f, 0.9f);
		else 
			SFXManager.instance.Play(stepSounds[currentStepIndex], 0.9f, 1.1f);
		
		currentStepIndex++;
		if (currentStepIndex >= stepSounds.Length) currentStepIndex = 0;

		yield return new WaitForSeconds(stepSoundsTime);

		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");
		if (isGrounded && x + z >= 1f) StartCoroutine(PlaySound());
	}
}
