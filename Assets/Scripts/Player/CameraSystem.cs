using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    Dictionary<string, Camera> cameras = new Dictionary<string, Camera>();

    void Awake()
    {
        Camera[] camArr = GameObject.FindObjectsOfType<Camera>();
        
        for (int i = 0; i < camArr.Length; i++)
        {
            addCamera(camArr[i].gameObject.name, camArr[i]);
        }
    }

    public void addCamera(string key, Camera cam)
    {
        cameras.Add(key, cam);
    }

    public Camera getCamera(string key)
    {
        return cameras[key];
    }

    public Camera getMainCamera() {
        return cameras["Main Camera"];
    }
}