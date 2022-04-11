using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    // camera tilt left and right
    [SerializeField] Camera cam;
    [SerializeField] CharacterController controller;
    [Header("FOV")]
    [SerializeField] float rateOfIncrease = 60.0f;
    private float maxFOV = 70f;
	private float minFOV = 60f;

    void Update()
    {
        if (cam.fieldOfView < minFOV || cam.fieldOfView > maxFOV) return; // Fixes issue when Sniper is scoped

        // FOV increase for sprinting
        if (Input.GetKey(KeyCode.LeftShift) && PlayerStatus.isMoving) 
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
    }
}
