using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashCounter : MonoBehaviour {

    [SerializeField]
    SuperTextMesh _amountText;

    bool _isDisplayed;
    float _displayTime = 3f;
    float _displayTimer = 0f;

    PlayerController _playerController;

    private void Awake() {
        _playerController = FindObjectOfType<PlayerController>();
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (_isDisplayed) {
            _displayTimer += Time.deltaTime;
            if (_displayTimer > _displayTime) {
                Hide();
            }
        }
    }

    public void DisplayCashCount() {
        _amountText.text = _playerController.cash.ToString();
        gameObject.SetActive(true);
        _isDisplayed = true;
        _displayTimer = 0f;
    }

    void Hide() {
        _isDisplayed = false;
        gameObject.SetActive(false);
    }
}
