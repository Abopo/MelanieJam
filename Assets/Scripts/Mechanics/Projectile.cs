using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float moveSpeed;
    Rigidbody _rigidbody;

    protected AudioSource _audioSource;
    protected Collider _myCollider;

    // Start is called before the first frame update
    protected virtual void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = moveSpeed * transform.forward;

        _audioSource = GetComponent<AudioSource>();
        _myCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update() {
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        // Cancel all momentum
        _rigidbody.velocity = Vector3.zero;

        _myCollider.enabled = false;

        // Set clip based on what was hit
        if(collision.gameObject.layer == LayerMask.NameToLayer("Shield")) {
            _audioSource.clip = GameManager.instance.gameResources._arrowCollideShield;
        }

        _audioSource.Play();
        StartCoroutine(WaitForFinish());
    }

    protected virtual IEnumerator WaitForFinish() {
        while(_audioSource.isPlaying) {
            yield return null;
        }

        Destroy(gameObject);
    }
}
