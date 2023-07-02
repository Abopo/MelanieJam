using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopup : MonoBehaviour {

    [SerializeField]
    string textToShow;

    bool _shown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (!_shown && other.tag == "Player") {
            GameManager.instance.ShowTextPopup(textToShow);
            _shown = true;
        }
    }
}
