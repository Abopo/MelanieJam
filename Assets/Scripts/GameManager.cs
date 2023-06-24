using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    bool _alive;

    public GameResources gameResources;

    private void Awake() {
        SingletonCheck();
    }
    void SingletonCheck() {
        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        if (obj != null && obj != gameObject) {
            _alive = false;
            DestroyImmediate(gameObject);
        } else {
            instance = this;
            _alive = true;
        }
    }

    // Start is called before the first frame update
    void Start() {
        gameResources = gameObject.GetComponent<GameResources>();
    }

    // Update is called once per frame
    void Update() {
        
    }
}
