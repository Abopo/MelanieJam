using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitReaction : MonoBehaviour {

    [SerializeField]
    string _collideWithTag;

    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start() {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == _collideWithTag) {
            React();
        }
    }

    protected virtual void React() {
        _audioSource.Play();
    }
}
