using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

    [SerializeField]
    SuperTextMesh _storyText;

    [SerializeField]
    float scrollSpeed;
    bool _scrolling;

    // Start is called before the first frame update
    void Start() {
        _storyText.onCompleteEvent.AddListener(StartScroll);
    }

    // Update is called once per frame
    void Update() {
        if (_scrolling) {
            transform.Translate(0f, scrollSpeed * Time.deltaTime, 0);

            if(transform.localPosition.y >= 3600) {
                // TODO: Return to title screen
                SceneManager.LoadScene("MainCaveScene");
            }
        }
    }

    void StartScroll() {
        _scrolling = true;
    }
}
