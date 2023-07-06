using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour {

    [SerializeField]
    protected GameObject _interactPrompt;

    bool _playerIsNear;

    protected AudioSource audioSource;

    protected PlayerController _playerController;

    // Start is called before the first frame update
    protected virtual void Start() {
        audioSource = GetComponent<AudioSource>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    protected virtual void Update() {
        if (_playerIsNear && _playerController.hasControl) {
            if (Input.GetKeyDown(KeyCode.E)) {
                OnInteract();
            }
        }
    }

    protected virtual void OnInteract() {

    }

    private void OnTriggerEnter(Collider other) {
        TriggerEnterChecks(other);
    }

    protected virtual void TriggerEnterChecks(Collider other) {
        if (other.tag == "Player") {
            _playerIsNear = true;

            // Show an interact prompt
            _interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            _playerIsNear = false;
            _interactPrompt.SetActive(false);
        }
    }

    private void OnDestroy() {
        _interactPrompt.SetActive(false);
        Destroy(_interactPrompt.gameObject);
    }

    protected virtual void Reset() {

    }
}
