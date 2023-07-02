using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeableTransform : MonoBehaviour {

    [SerializeField]
    float _frequency;
    [SerializeField]
    Vector2 _maxShake;
    Vector2 _curMaxShake;

    public bool unscaledTime;
    public bool fadeout;

    float _shakeTime;
    float _shakeTimer = 0f;
    bool _shaking;

    float _fadeTime = 0.01f;
    float _fadeTimer = 0f;
    float _fadeRateX;
    float _fadeRateY;
    bool _fading;

    Vector3 _basePos;

    float _seed;

    private void Awake() {
        _seed = Random.value;

        _basePos = transform.localPosition;
    }
    // Start is called before the first frame update
    void Start() {
        _curMaxShake = _maxShake;
    }

    // Update is called once per frame
    void Update() {
        if (_shaking) {
            if (unscaledTime) {
                transform.localPosition = _basePos + new Vector3(
                    _curMaxShake.x * Mathf.PerlinNoise(_seed, Time.unscaledTime * _frequency) * 2,
                    0,
                    _curMaxShake.y * Mathf.PerlinNoise(_seed + 1, Time.unscaledTime * _frequency) * 2);

                _shakeTimer += Time.unscaledDeltaTime;
            } else {
                transform.localPosition = _basePos + new Vector3(
                    _curMaxShake.x * Mathf.PerlinNoise(_seed, Time.time * _frequency) * 2,
                    0,
                    _curMaxShake.y * Mathf.PerlinNoise(_seed + 1, Time.time * _frequency) * 2);

                _shakeTimer += Time.deltaTime;
            }
            if(_shakeTimer >= _shakeTime && !_fading) {
                if (fadeout) {
                    StartFadeOut();
                } else {
                    StopShaking();
                }
            }
        }

        if (_fading) {
            if (unscaledTime) {
                _fadeTimer += Time.unscaledDeltaTime;
            } else {
                _fadeTimer += Time.deltaTime;
            }

            // Fade out
            if(_fadeTimer >= _fadeTime) {
                FadeOut();

                _fadeTimer = 0f;
            }
        }
    }

    public void ResetBasePosition() {
        _basePos = transform.localPosition;
    }

    public void StartShake(float shakeTime) {
        //_basePos = transform.localPosition;
       
        _shakeTime = shakeTime;
        _shakeTimer = 0f;
        _shaking = true;
        _curMaxShake = _maxShake;
    }
    public void StartShake(float shakeTime, float frequency) {
        //_basePos = transform.localPosition;
       
        _shakeTime = shakeTime;
        _shakeTimer = 0f;
        _shaking = true;

        _frequency = frequency;
        _curMaxShake = _maxShake;
    }
    public void StartShake(float shakeTime, float frequency, Vector2 maxShake) {
        //_basePos = transform.localPosition;
        
        _shakeTime = shakeTime;
        _shakeTimer = 0f;
        _shaking = true;

        _frequency = frequency;
        _maxShake = maxShake;
        _curMaxShake = _maxShake;
    }

    public void StopShaking() {
        _shaking = false;
        _fading = false;
        transform.localPosition = _basePos;
    }

    public void StartFadeOut() {
        _fading = true;
        _fadeTimer = 0f;

        _fadeRateX = _maxShake.x / 100;
        _fadeRateY = _maxShake.y / 100;
    }
    
    void FadeOut() {
        // Reduce by fadeRates
        _curMaxShake.x = _curMaxShake.x - _fadeRateX;
        _curMaxShake.y = _curMaxShake.y - _fadeRateY;

        // Make sure we don't go past 0
        if (_curMaxShake.x < 0) {
            _curMaxShake.x = 0;
        }
        if (_curMaxShake.y < 0) {
            _curMaxShake.y = 0;
        }

        // Once all shaking has been reduced to 0
        if (_curMaxShake.x <= 0 && _curMaxShake.y <= 0) {
            StopShaking();
        }
    }
}
