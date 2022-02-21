using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source: https://www.youtube.com/watch?v=_QajrabyTJc
public class MouseLook : MonoBehaviour
{
	[SerializeField] Transform playerBody;
    [SerializeField] Camera cam;
	[SerializeField] float mouseSensitivity = 300.0f;
	[SerializeField] float rateOfIncrease = 60.0f;
	private float maxFOV = 70f;
	private float minFOV = 60f;
	private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam.fieldOfView = 60.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -=mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);

			transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
			playerBody.Rotate(Vector3.up * mouseX);
		}
        
		//FOV increase for sprinting
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
			if (cam.fieldOfView < maxFOV)
			{
				cam.fieldOfView += rateOfIncrease * Time.deltaTime;
			}
        }
        else
        {
            if (cam.fieldOfView > minFOV)
			{
				cam.fieldOfView -= rateOfIncrease * Time.deltaTime;
			}
        }

		if (cam.fieldOfView < minFOV) {
            cam.fieldOfView = minFOV;
        } else if (cam.fieldOfView > maxFOV) {
            cam.fieldOfView = maxFOV;
        }
    }
}
