using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echolocation : MonoBehaviour {


    [SerializeField]
    ParticleSystem _ecolocation;

    AudioSource _audioSource;
    float _baseVolume;

    public bool _isPlaying = false;

    Coroutine fadeIn;
    Coroutine fadeOut;

    bool _pausing = false;
    float _pauseDelay = 0.2f;
    float _pauseTimer = 0f;

    float _fadeTime = 0.5f;
    float _fadeTimer = 0f;

    // Start is called before the first frame update
    void Start() {
        _audioSource = GetComponent<AudioSource>();
        _baseVolume = _audioSource.volume;
        _audioSource.Play();
        _audioSource.Pause();
    }

    // Update is called once per frame
    void Update() {
        if (_pausing) {
            _pauseTimer += Time.deltaTime;
            if (_pauseTimer > _pauseDelay) {
                PauseSound();
            }
        }
    }

    public void PlayEffect() {
        if (!_isPlaying) {
            _isPlaying = true;

            _ecolocation.Play();

            _pausing = false;

            fadeIn = StartCoroutine(FadeSoundIn());
        }
    }

    public void StopEffect() {
        if (_isPlaying && !_pausing) {
            _ecolocation.Stop();
            _pauseTimer = 0f;
            _pausing = true;
        }
    }

    void PauseSound() {
        _isPlaying = false;
        fadeOut = StartCoroutine(FadeSoundOut());
        _pausing = false;
    }

    IEnumerator FadeSoundIn() {
        //Debug.Log("Fade sound in.");

        // Don't overlap fade in/out
        if (fadeOut != null) {
            StopCoroutine(fadeOut);
        }

        _audioSource.UnPause();
        _fadeTimer = 0f;
        float _curVolume = _audioSource.volume;

        while (_fadeTimer < _fadeTime) {
            _fadeTimer += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(_curVolume, _baseVolume, _fadeTimer / _fadeTime);
            yield return null;
        }

        _audioSource.volume = _baseVolume;
        //Debug.Log("Sound fully faded in.");
    }

    IEnumerator FadeSoundOut() {
        //Debug.Log("Fade sound out.");

        // Don't overlap fade in/out
        if (fadeIn != null) {
            StopCoroutine(fadeIn);
        }

        _fadeTimer = 0f;
        float _curVolume = _audioSource.volume;

        while (_fadeTimer < _fadeTime) {
            _fadeTimer += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(_curVolume, 0, _fadeTimer / _fadeTime);
            yield return null;
        }

        _audioSource.volume = 0;
        _audioSource.Pause();
        //Debug.Log("Sound fully faded out.");
    }
}
