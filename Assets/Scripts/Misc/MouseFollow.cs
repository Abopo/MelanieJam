using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour {

    Camera _camera;
    Vector3 _mousePosition;

    // Start is called before the first frame update
    void Start() {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update() {
        _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.y = 0;
        transform.position = _mousePosition;
    }
}
