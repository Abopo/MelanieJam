using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSounds : MonoBehaviour {

    [SerializeField]
    AudioSource _mainAudio;
    [SerializeField]
    AudioSource _extraAudio;
    [SerializeField]
    AudioSource _extraAudio2;

    [SerializeField]
    AudioClip[] _footstepClips;
    int _footstepIndex;

    [SerializeField]
    AudioClip[] _chargeClips;

    [SerializeField]
    AudioClip _hurtClip;

    float _stepDist = 3f;
    Vector3 _lastPos;
    float _distFromLastStep;

    // Start is called before the first frame update
    void Start() {
        _lastPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        // Play footstep sounds based on the monster's movement
        Footsteps();

        // Testing
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            ChargeSetup();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            ChargeStart();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            ReturnToPath();
        }
    }

    void Footsteps() {
        _distFromLastStep += Vector3.Distance(_lastPos, transform.position);
        // Save the step position
        _lastPos = transform.position;

        // Check if we've walked far enough for a step (and we're not in the middle of another step)
        if (_distFromLastStep > _stepDist) {
            // Play the next footstep sound
            if(!_extraAudio.isPlaying) {
                _extraAudio.clip = _footstepClips[_footstepIndex++];
                _extraAudio.Play();
            } else if (!_extraAudio2.isPlaying) {
                _extraAudio2.clip = _footstepClips[_footstepIndex++];
                _extraAudio2.Play();
            } else {
                _extraAudio.clip = _footstepClips[_footstepIndex++];
                _extraAudio.Play();
            }

            if (_footstepIndex >= _footstepClips.Length) {
                _footstepIndex = 0;
            }
            _distFromLastStep = 0;
        }
    }

    public void ChargeSetup() {
        // Make footsteps play faster
        //_stepDist = 2f;

        // Play first charge clip
        _mainAudio.clip = _chargeClips[0];
        _mainAudio.Play();
    }

    public void ChargeStart() {
        // Play next charge clip
        _mainAudio.clip = _chargeClips[1];
        _mainAudio.Play();
    }

    public void PlayChargeHit(GameObject hitObject) {
        // Play the main hit clip
        _mainAudio.clip = _chargeClips[3];
        _mainAudio.Play();

        // Also play an extra one based on the object hit
        switch (hitObject.tag) {
            case "Metal":
                _extraAudio.clip = _chargeClips[4];
                _extraAudio.Play();
                break;
            default:
                //_extraAudio.clip = _chargeHitClips[1];
                break;
        }
    }

    public void PlayFinalChargeHit() {
        // Play the main hit clip
        _mainAudio.clip = _chargeClips[3];
        _mainAudio.Play();

        // also something else???
    }

    public void ReturnToPath() {
        _mainAudio.clip = _chargeClips[2];
        _mainAudio.Play();

        // Return footsteps to normal
        _stepDist = 3f;
    }

    public void HurtClip() {
        _mainAudio.clip = _hurtClip;
        _mainAudio.Play();
    }
}
