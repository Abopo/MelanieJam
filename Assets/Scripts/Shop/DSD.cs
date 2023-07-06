using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSD : NPC {

    [SerializeField]
    GameObject _shop;

    [SerializeField]
    bool _firstTime = true;
    bool _playerHadCash;
    bool _openShop;

    protected override void OnInteract() {
        base.OnInteract();

        if (_firstTime) {
            SayDialogue(0, 4);
        } else {
            if (_playerHadCash) {
                SayDialogue(10, 10);
                _openShop = true;
            } else {
                if (_playerController.cash > 0) {
                    _playerHadCash = true;
                    SayDialogue(9, 10);
                    _openShop = true;
                } else {
                    SayDialogue(7, 8);
                }
            }
        }

    }

    void OpenShop() {
        _shop.SetActive(true);
        _playerController.hasControl = false;
    }

    protected override void OnDialogueComplete() {
        base.OnDialogueComplete();

        if (_firstTime) {
            _firstTime = false;
            // Check if the player has cash yet
            if (_playerController.cash > 0) {
                _playerHadCash = true;
                SayDialogue(5, 6);
                _openShop = true;
            } else {
                SayDialogue(7, 8);
            }
        } else {
            if (_openShop) {
                OpenShop();
                _openShop = false;
            }
        }
    }

    public void CloseShop() {
        _shop.SetActive(false);
        _playerController.hasControl = true;
        EndDialogue();
    }

    public void ShinyRockDialogue() {
        SayDialogue(12, 13);
    }

    public void FloodlightDialogue() {
        if (_playerController.hasFloodlight) {
            SayDialogue(20, 20);
        } else {
            SayDialogue(14, 16);
        }
    }

    public void SlingshotDialogue() {
        if (_playerController.hasSlingshot) {
            SayDialogue(23, 23);
        } else {
            SayDialogue(17, 17);
        }
    }
}
