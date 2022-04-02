using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform camera;

    private void LateUpdate() {
        if (camera == null) { camera = GameObject.FindObjectOfType<CameraSystem>().getMainCamera().transform; }
        transform.LookAt(transform.position + camera.forward);
    }
}
