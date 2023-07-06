using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : OnOffObject {

    public float upYPos;
    public float downYPos;

    [SerializeField]
    Transform _movingPart;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public override void TurnOn() {
        base.TurnOn();

        // Move up to blocking position
        _movingPart.position = new Vector3(transform.position.x, upYPos, transform.position.z);

        // TODO: Play audio source if have
    }

    public override void TurnOff() {
        base.TurnOff();

        // Move down below the floor
        _movingPart.position = new Vector3(transform.position.x, downYPos, transform.position.z);
    }
}
