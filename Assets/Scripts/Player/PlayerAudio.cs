using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {

    [SerializeField]
    AudioClip[] _footstepClips;
    int _footstepIndex;

    [SerializeField]
    float _stepDist = 2f;
    Vector3 _lastStepPos;
    float _distFromLastStep;

    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start() {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        // TODO: Don't play from hit push?
        Footsteps();
    }

    void Footsteps() {
        _distFromLastStep += Vector3.Distance(_lastStepPos, transform.position);
        // Save the step position
        _lastStepPos = transform.position;

        if (_distFromLastStep > _stepDist) {
            _distFromLastStep = 0;

            // Play the next footstep sound
            _audioSource.clip = _footstepClips[_footstepIndex++];
            _audioSource.Play();
            if (_footstepIndex >= _footstepClips.Length) {
                _footstepIndex = 0;
            }

        }
    }
}
