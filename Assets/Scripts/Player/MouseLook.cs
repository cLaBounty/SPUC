using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source: https://www.youtube.com/watch?v=_QajrabyTJc
public class MouseLook : MonoBehaviour
{
    public static float Sensitivity = 250f;
    public static float SensitivityMultiplier = 1f;

	[SerializeField] Transform playerBody;
    [SerializeField] Camera cam;

	private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam.fieldOfView = 60.0f;
    }
    
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X") * Sensitivity * SensitivityMultiplier * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * Sensitivity * SensitivityMultiplier * Time.deltaTime;

            xRotation -=mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);

			transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
			playerBody.Rotate(Vector3.up * mouseX);
		}
    }
}
