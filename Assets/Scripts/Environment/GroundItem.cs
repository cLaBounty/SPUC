using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public ItemObject item;
    public int amount = 1;
    
    float hoverRate = 0.5f;
    float highestOffset = 0.6f;
    float startingZ;
    float time = 0;

    void Start() {
        startingZ = transform.position.y;
        time = Random.Range(0f, 1f);
    }

    void Update() {
        time += hoverRate * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, startingZ + Mathf.Lerp(0, highestOffset, Mathf.Cos(time * Mathf.PI) * 0.5f + 0.5f), transform.position.z);
    }

}