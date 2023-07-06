using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetReticle : MonoBehaviour {

    Camera _camera;
    Vector3 _mousePosition;
    Vector3 _offscreenPos = new Vector3(1000, 1000, 1000);

    PlayerController _playerController;

    // Start is called before the first frame update
    void Start() {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update() {
        if (_playerController.hasSlingshot && _playerController.hasControl) {
            _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            _mousePosition.y = 2;
            transform.position = _mousePosition;
        } else {
            transform.position = _offscreenPos;
        }
    }
}
