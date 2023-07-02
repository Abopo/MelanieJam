using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthMeter : MonoBehaviour {

    int _health = 3;

    [SerializeField]
    GameObject _meterObject;

    [SerializeField]
    Image[] _healthPoints;

    bool _displayed;
    float _displayTime = 1f;
    float _displayTimer = 0f;

    public static UnityEvent onPlayerDeath = new UnityEvent();

    // Start is called before the first frame update
    void Start() {
        HideHealth();
    }

    // Update is called once per frame
    void Update() {
        if(_displayed) {
            _displayTimer += Time.deltaTime;
            if(_displayTimer >= _displayTime) {
                HideHealth();
            }
        }
    }

    public void TakeDamage() {
        _health -= 1;

        if (_health <= 0) {
            _health = 0;
        }

        _healthPoints[_health].enabled = false;

        DisplayHealth();
    }

    public void GainHealth() {
        _health += 1;
        if(_health >= 3) {
            _health = 3;
        }

        _healthPoints[_health-1].enabled = true;

        DisplayHealth();
    }

    public void ResetHealth() {
        if(_health < 3) {
            DisplayHealth();
        }

        _health = 3;
        for (int i = 0; i < _healthPoints.Length; i++) {
            _healthPoints[i].enabled = true;
        }
    }

    void DisplayHealth() {
        _displayed = true;
        _displayTimer = 0f;

        _meterObject.SetActive(true);
    }

    void HideHealth() {
        _displayed = false;
        _meterObject.SetActive(false);
    }

    public bool DeathCheck() {
        if (_health <= 0) {
            _health = 0;

            // Die and restart
            Die();

            return true;
        } else {
            return false;
        }
    }

    void Die() {
        onPlayerDeath.Invoke();

        ResetHealth();
    }
}
