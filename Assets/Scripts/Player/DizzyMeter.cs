using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DizzyMeter : MonoBehaviour {

    [SerializeField]
    GameObject meterObject;
    [SerializeField]
    RectTransform _meterFront;

    float _baseMeterLength;
    [SerializeField]
    float _curMeterLength;

    [SerializeField]
    float _dizzyTime;
    [SerializeField]
    float _dizzyTimer;

    float _bufferTime = 1f;
    float _bufferTimer;

    public bool isDizzy;

    bool _increasingMeter;

    // Start is called before the first frame update
    void Start() {
        _baseMeterLength = _meterFront.sizeDelta.x;
        _curMeterLength = 0;
        UpdateMeter();
    }

    // Update is called once per frame
    void Update() {
        if(!_increasingMeter && _dizzyTimer > 0) {
            _bufferTimer += Time.deltaTime;
            if (_bufferTimer > _bufferTime) {
                if (isDizzy) {
                    DecreaseMeter(Time.deltaTime/2f);
                } else {
                    DecreaseMeter(Time.deltaTime/2f);
                }
            }
        }
    }

    private void LateUpdate() {
        _increasingMeter = false;
    }

    public void IncreaseMeter() {
        _increasingMeter = true;
        _bufferTimer = 0f;

        if (!meterObject.activeSelf) {
            meterObject.SetActive(true);
        }
        _dizzyTimer += Time.fixedDeltaTime;
        _curMeterLength = Mathf.Lerp(0, _baseMeterLength, _dizzyTimer / _dizzyTime);
        UpdateMeter();
    }

    void DecreaseMeter(float time) {
        _dizzyTimer -= time;
        if(_dizzyTimer <= 0) {
            _dizzyTimer = 0;
        }
        _curMeterLength = Mathf.Lerp(0, _baseMeterLength, _dizzyTimer / _dizzyTime);
        UpdateMeter();
    }

    void UpdateMeter() {
        _meterFront.sizeDelta = new Vector2(_curMeterLength, _meterFront.rect.height);

        if(_curMeterLength >= _baseMeterLength) {
            // Get dizzy
            isDizzy = true;
        }
        if (_curMeterLength <= 0) {
            // Hide the meters
            meterObject.SetActive(false);
            // Undizzy
            isDizzy = false;
        }
    }

    public void ResetMeter() {
        _dizzyTimer = 0f;
        UpdateMeter();
    }
}
