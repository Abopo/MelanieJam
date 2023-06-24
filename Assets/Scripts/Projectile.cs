using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float moveSpeed;
    Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = new Vector3(moveSpeed, 0, 0);
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }
}
