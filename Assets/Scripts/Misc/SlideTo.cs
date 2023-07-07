using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlideTo : MonoBehaviour {

    // Lerping
    protected bool _lerping;
    Vector3 _startPos;
    Vector3 _endPos;
    float _distance;
    float _startTime;
    public float _lerpSpeed = 10f;
    float _interpolation;
    bool _local;
    bool _unscaledTime;

    public UnityEvent SlideFinished;


    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (_lerping) {
            // Distance moved equals elapsed time times speed..
            float distCovered;

            if (_unscaledTime) {
                distCovered = (Time.unscaledTime - _startTime) * _lerpSpeed;
            } else {
                distCovered = (Time.time - _startTime) * _lerpSpeed;
            }

            // Fraction of journey completed equals current distance divided by total distance.
            _interpolation = distCovered / _distance;

            if (_local) {
                transform.localPosition = Vector3.Lerp(_startPos, _endPos, _interpolation);
            } else {
                transform.position = Vector3.Lerp(_startPos, _endPos, _interpolation);
            }

            if (_interpolation >= 1f) {
                _lerping = false;

                SlideFinished.Invoke();
            }
        }
    }

    public void LerpToPos(Vector3 pos) {
        _lerping = true;
        _startPos = transform.position;
        _endPos = pos;
        _local = false;
        
        _distance = Vector3.Distance(_startPos, _endPos);
        if (_distance <= 0) {
            // We're already at the position so just cancel
            _lerping = false;
            SlideFinished.Invoke();
        }

        _unscaledTime = false;
        _startTime = Time.time;
        _interpolation = 0f;
    }
    public void LerpToPos(Vector3 pos, bool local) {
        _lerping = true;
        if (local) {
            _startPos = transform.localPosition;
            _local = true;
        } else {
            _startPos = transform.position;
            _local = false;
        }
        _endPos = pos;
        
        _distance = Vector3.Distance(_startPos, _endPos);
        if(_distance <= 0) {
            // We're already at the position so just cancel
            _lerping = false;
            SlideFinished.Invoke();
        }

        _unscaledTime = false;
        _startTime = Time.time;
        _interpolation = 0f;
    }
    public void LerpToPos(Vector3 pos, bool local, bool unscaledTime) {
        _lerping = true;
        if (local) {
            _startPos = transform.localPosition;
            _local = true;
        } else {
            _startPos = transform.position;
            _local = false;
        }
        _endPos = pos;

        _distance = Vector3.Distance(_startPos, _endPos);
        if (_distance <= 0) {
            // We're already at the position so just cancel
            _lerping = false;
            SlideFinished.Invoke();
        }

        if (unscaledTime) {
            _startTime = Time.unscaledTime;
            _unscaledTime = true;
        } else {
            _startTime = Time.time;
            _unscaledTime = false;
        }
        _interpolation = 0f;
    }

    public void LerpToPos(Vector3 pos, float lerpSpeed) {
        _lerping = true;
        _startPos = transform.position;
        _endPos = pos;
        _local = false;
        
        _distance = Vector3.Distance(_startPos, _endPos);
        if (_distance <= 0) {
            // We're already at the position so just cancel
            _lerping = false;
            SlideFinished.Invoke();
        }

        _lerpSpeed = lerpSpeed;
        _unscaledTime = false;
        _startTime = Time.time;
        _interpolation = 0f;
    }
}
