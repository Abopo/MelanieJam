using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : OnOffObject {

    public float upYPos;
    public float downYPos;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public override void TurnOn() {
        base.TurnOn();

        // Move up to blocking position
        transform.position = new Vector3(transform.position.x, upYPos, transform.position.z);
    }

    public override void TurnOff() {
        base.TurnOff();

        // Move down below the floor
        transform.position = new Vector3(transform.position.x, downYPos, transform.position.z);
    }
}
