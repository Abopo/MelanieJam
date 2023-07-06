using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Monster") {
            // End the game
            GameEnd();
        }
    }

    void GameEnd() {
        // Slow down time

        // Have the wall break open

        // Eventually transition to credits or something

        // For now just popping up a menu
        GameManager.instance.OnGameEnd.Invoke();
    }
}
