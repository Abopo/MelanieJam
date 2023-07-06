using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : InteractableObject {

    [SerializeField]
    SuperTextMesh _dialogueText;

    [SerializeField]
    string[] _dialogues;

    int _curIndex;
    int _endIndex;
    bool _isSpeaking;

    Coroutine _nextLineCoroutine;

    protected override void Start() {
        base.Start();

        _playerController = FindObjectOfType<PlayerController>();
        _dialogueText.onCompleteEvent.AddListener(OnFinishedReading);
    }

    protected override void Update() {
        base.Update();

        if (_isSpeaking) {
            if (!_dialogueText.reading) {
                // Wait for input from player
                if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) {
                    //NextLine();
                }
            }
        }
    }

    public void SayDialogue(int startIndex, int endIndex) {
        _curIndex = startIndex;
        _endIndex = endIndex;
        _isSpeaking = true;

        _dialogueText.gameObject.SetActive(true);
        _dialogueText.text = _dialogues[_curIndex++];

        _interactPrompt.SetActive(false);

        // Take control away from player
        _playerController.hasControl = false;

        // Make sure to stop the next line coroutine in case it was part way through
        if (_nextLineCoroutine != null) {
            StopCoroutine(_nextLineCoroutine);
        }
    }

    protected virtual void OnFinishedReading() {
        // Just automatically go to the next line after a small delay.
        _nextLineCoroutine = StartCoroutine(NextLineLater());
    }
    IEnumerator NextLineLater() {
        yield return new WaitForSeconds(1.2f);

        NextLine();
    }

    protected void NextLine() {
        if(_curIndex <= _endIndex) {
            _dialogueText.text = _dialogues[_curIndex++];
        } else {
            EndDialogue();
        }
    }

    protected void EndDialogue() {
        // End speaking
        _isSpeaking = false;
        _dialogueText.gameObject.SetActive(false);

        OnDialogueComplete();
    }

    protected virtual void OnDialogueComplete() {
        _interactPrompt.SetActive(true);

        // Give control back to player
        _playerController.hasControl = true;
    }
}
