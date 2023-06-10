using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float life = 5f;
    public float speed = 0.1f;

    private void Start() {
        StartCoroutine(Lifetime());    
    }

    private void Update() {
        transform.Translate(Vector3.forward * speed);
    }

    private IEnumerator Lifetime() {
        yield return new WaitForSeconds(life);
        Destroy(gameObject);
    }

}
