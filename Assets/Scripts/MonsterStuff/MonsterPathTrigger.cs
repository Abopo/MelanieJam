using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPathTrigger : MonoBehaviour {

    [SerializeField]
    MonsterPath _pathToTrigger;

    [SerializeField]
    MONSTER_STATE _state;

    TheMonster _monster;

    // Start is called before the first frame update
    void Start() {
        _monster = FindObjectOfType<TheMonster>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            // Update the monster's path to ours.
            _monster.ChangePath(_pathToTrigger, _state);
        }
    }
}
