using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilDestroy : MonoBehaviour
{
    [SerializeField] float destroyTimer;
    private void Start() {
        StartCoroutine(DestroyTimer());
    }

    IEnumerator DestroyTimer(){
        yield return new WaitForSeconds(destroyTimer);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other) {
        Destroy(gameObject);
    }
}
