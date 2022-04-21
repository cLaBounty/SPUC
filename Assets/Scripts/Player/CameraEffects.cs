using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    // camera tilt left and right
    [SerializeField] Camera cam;
    [SerializeField] CharacterController controller;
    [Header("FOV")]
    [SerializeField] float rateOfIncrease = 45.0f;
    private float maxFOV = 70f;
	private float minFOV = 60f;

    void Update()
    {
        if (cam.fieldOfView < minFOV || cam.fieldOfView > maxFOV) return; // Fixes issue when Sniper is scoped

        // FOV increase for sprinting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            float newFOV = cam.fieldOfView - rateOfIncrease * Time.deltaTime;
            if (newFOV > minFOV) {
                cam.fieldOfView = newFOV;
            } else {
                cam.fieldOfView = minFOV;
            }
        }
        else
        {
            float newFOV = cam.fieldOfView + rateOfIncrease * Time.deltaTime;
            if (newFOV < maxFOV) {
                cam.fieldOfView = newFOV;
            } else {
                cam.fieldOfView = maxFOV;
            }
        }


    }
}
