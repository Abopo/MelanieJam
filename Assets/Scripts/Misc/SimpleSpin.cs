using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpin : MonoBehaviour {

    [SerializeField]
    float spinSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }
}
