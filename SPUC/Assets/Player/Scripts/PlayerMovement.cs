using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Source: https://www.youtube.com/watch?v=_QajrabyTJc
public class PlayerMovement : MonoBehaviour
{
	public CharacterController controller;
	public float speed = 12f;
	public float baseSpeed = 12f;
	public float sprintSpeed;
	public float gravity = -9.81f * 2;
	public float jumpHeight = 3f;

	public Transform groundCheck;
	public float groundDistance = 0.4f;
	public LayerMask groundMask;

	public float spritingMultiplier = 1.75f;

	Vector3 velocity;
	bool isGrounded;

    // Update is called once per frame
    void Update()
    {
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		if(isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

        float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		if(Input.GetKey(KeyCode.LeftShift))
		{
			sprintSpeed = baseSpeed * spritingMultiplier;
			speed = sprintSpeed;
		} else
		{
			speed = baseSpeed;
		}

		Vector3 move = transform.right * x + transform.forward * z;
		controller.Move(move * speed * Time.deltaTime);

		if(Input.GetButtonDown("Jump") && isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}

		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}
