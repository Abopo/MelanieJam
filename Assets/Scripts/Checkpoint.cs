using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public bool activated;

    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start() {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            if (!activated) {
                _audioSource.Play();
            }

            GameManager.instance.PlayerTouchedCheckpoint(this);
            other.GetComponentInParent<PlayerController>().HealthMeter.ResetHealth();
        }
    }
}
