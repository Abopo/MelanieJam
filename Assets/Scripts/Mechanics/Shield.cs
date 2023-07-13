using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    PlayerController _player;

    Vector3 _rotate = new Vector3(0f, 90f, 0f);

    [SerializeField]
    GameObject _objects;

    // Start is called before the first frame update
    void Start() {
        _player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update() {
        // Always be at player position
        transform.position = _player.transform.position;

        if (_player.hasControl) {
            if (Input.GetMouseButtonDown(0)) {
                // Change position by rotating 90 degrees
                transform.Rotate(_rotate);
            }
            if (Input.GetKeyDown(KeyCode.Q)) {
                // Toggle shield on/off
                if(_objects.activeSelf) {
                    _objects.SetActive(false);
                } else {
                    _objects.SetActive(true);
                }
            }
        }
    }
}
