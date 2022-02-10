using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source: https://www.youtube.com/watch?v=_QajrabyTJc
public class MouseLook : MonoBehaviour
{

	public float mouseSensitivity = 300f;
	public Transform playerBody;
    public Camera cam;
	float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam.fieldOfView = 60.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		xRotation -=mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
		playerBody.Rotate(Vector3.up * mouseX);

		//FOV increase for sprinting
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
			if (cam.fieldOfView != 70.0f)
			{
				cam.fieldOfView += 1.0f;
			}
        }
        else
        {
            if (cam.fieldOfView != 60.0f)
			{
				cam.fieldOfView -= 1.0f;
			}
        }

        // if (Input.GetKey(KeyCode.LeftControl)) 
        // {

        // }
    }
}
