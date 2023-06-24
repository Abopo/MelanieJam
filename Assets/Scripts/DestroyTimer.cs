using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour {

    public float timeToDestroy;
    float _timer;

    bool _isDestroyed;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (!_isDestroyed) {
            _timer += Time.deltaTime;
            if (_timer > timeToDestroy) {
                _isDestroyed = true;
                Destroy(gameObject);
            }
        }
    }
}
