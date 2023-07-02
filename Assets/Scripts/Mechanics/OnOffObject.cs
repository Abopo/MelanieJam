using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffObject : MonoBehaviour {

    public bool isOn;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public virtual void TurnOn() {
        isOn = true;
    }

    public virtual void TurnOff() {
        isOn = false;
    }

    public void SetState(bool isOn) {
        if (isOn) {
            TurnOn();
        } else {
            TurnOff();
        }
    }
}
