using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock : MonoBehaviour {

    [SerializeField]
    float _soundThreshold;

    Vector3 _lastPos;
    AudioSource _audioSource;
    float _dist;

    // Start is called before the first frame update
    void Start() {
        // Play for one frame
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update() {
    }

    private void FixedUpdate() {
        _dist = Vector3.Distance(transform.position, _lastPos);
        if (_dist > _soundThreshold) {
            if(_audioSource.volume < 1) {
                _audioSource.volume = 1;
            }
            _audioSource.UnPause();
        } else {
            _audioSource.Pause();
        }

        _lastPos = transform.position;
    }
}
