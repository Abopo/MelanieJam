using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractableObject {

    [SerializeField]
    OnOffObject[] linkedObjects;

    public bool isOn;

    [SerializeField]
    Transform _leverObject;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }

    protected override void OnInteract() {
        // Gonna try not letting the lever flip back
        if (isOn) {
            base.OnInteract();
            Flip();
            // Play a sound
            _audioSource.Play();
        }
    }

    void Flip() {
        // Flip
        isOn = !isOn;

        // Flip all linked objects
        foreach (OnOffObject ooo in linkedObjects) {
            if (ooo.isOn != isOn) {
                ooo.SetState(isOn);
            }
        }

        // Move lever to correct position
        UpdateLeverPosition();
    }

    void UpdateLeverPosition() {
        if(isOn) {
            _leverObject.localRotation = Quaternion.Euler(-50, 0, 0);
        } else {
            _leverObject.localRotation = Quaternion.Euler(50, 0, 0);
        }
    }

    protected override void TriggerEnterChecks(Collider other) {
        // Gonna try not letting the lever flip back
        if (isOn) {
            base.TriggerEnterChecks(other);

            if (other.tag == "Damage") {
                OnInteract();
            }
        }
    }
}
