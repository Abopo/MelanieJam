using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendNPC : NPC {

    bool _gaveShield;
    Object _shieldObject;

    protected override void Start() {
        base.Start();

        _gaveShield = false;
        _shieldObject = Resources.Load("Prefabs/Shield");
    }

    protected override void OnInteract() {
        base.OnInteract();

        if (_gaveShield) {
            SayDialogue(16, 17);
        } else {
            SayDialogue(0, 14);
        }
    }

    protected override void OnDialogueComplete() {
        base.OnDialogueComplete();

        if (!_gaveShield) {
            Debug.Log("Spawn shield.");

            // Spawn a shield for the player
            Instantiate(_shieldObject);

            GameManager.instance.ShowTextPopup("Got Shield\nLMB to rotate   |   Q to toggle", 10);

            GetComponent<AudioSource>().Play();

            _gaveShield = true;
        }
    }
}
