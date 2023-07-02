using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float moveSpeed;
    Rigidbody _rigidbody;

    AudioSource _audioSource;
    Collider _myCollider;

    // Start is called before the first frame update
    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = moveSpeed * transform.forward;

        _audioSource = GetComponent<AudioSource>();
        _myCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnCollisionEnter(Collision collision) {
        GetComponent<AudioSource>().Play();
        _myCollider.enabled = false;
        StartCoroutine(WaitForFinish());
    }

    IEnumerator WaitForFinish() {
        while(_audioSource.isPlaying) {
            yield return null;
        }

        Destroy(gameObject);
    }
}
