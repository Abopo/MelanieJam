using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    [SerializeField]
    SuperTextMesh _currencyText;

    [SerializeField]
    Button _shinyRockButton;
    [SerializeField]
    int _shinyRockPrice;
    int _shinyRockStock = 5;
    [SerializeField]
    SuperTextMesh _shinyRockStockText;

    [SerializeField]
    Button _floodlightButton;
    [SerializeField]
    int _floodlightPrice;
    int _floodlightStock = 1;
    [SerializeField]
    SuperTextMesh _floodlightStockText;

    [SerializeField]
    Button _slingshotButton;
    [SerializeField]
    int _slingshotPrice;
    int _slingshotStock = 1;
    [SerializeField]
    SuperTextMesh _slingshotStockText;

    DSD _dsd;
    PlayerController _playerController;

    // Start is called before the first frame update
    void Start() {
        _dsd = FindObjectOfType<DSD>();
        _playerController = FindObjectOfType<PlayerController>();

        // Make sure we're off at game start
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        _currencyText.text = _playerController.cash.ToString();
    }

    public void TryBuyShinyRock() {
        if(_shinyRockStock > 0) {
            if (_playerController.cash >= _shinyRockPrice) {
                // Give the player a shiny rock
                _playerController.shinyRocks += 1;
                _playerController.cash -= 1;

                _shinyRockStock -= 1;
                _shinyRockStockText.text = _shinyRockStock.ToString();

                _dsd.SayDialogue(19, 19);

                if (_shinyRockStock <= 0) {
                    // Set to sold out
                    _shinyRockButton.interactable = false;
                }
            }
        }
    }

    public void TryBuyFloodlight() {
        if (_floodlightStock > 0) {
            if (_playerController.cash >= _floodlightPrice) {
                // Give the player a floodlight
                _playerController.hasFloodlight = true;
                _playerController.cash -= 1;

                _floodlightStock -= 1;
                _floodlightStockText.text = _floodlightStock.ToString();

                _dsd.SayDialogue(19, 20);

                if (_floodlightStock <= 0) {
                    // Set to sold out
                    _floodlightButton.interactable = false;
                }
            }
        }
    }

    public void TryBuySlingshot() {
        if (_slingshotStock > 0) {
            if (_playerController.cash >= _slingshotPrice) {
                // Give the player a slingshot
                _playerController.hasSlingshot = true;
                _playerController.cash -= 1;

                _slingshotStock -= 1;
                _slingshotStockText.text = _slingshotStock.ToString();

                _dsd.SayDialogue(22, 23);

                if (_slingshotStock <= 0) {
                    // Set to sold out
                    _slingshotButton.interactable = false;
                }
            }
        }
    }
}
