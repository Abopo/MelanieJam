using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu : MonoBehaviour {

    [SerializeField]
    SuperTextMesh _deathMessage;

    [SerializeField]
    string[] _messages;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMenu() {
        // Set a random death message
        int rand = Random.Range(0, _messages.Length);
        _deathMessage.text = _messages[rand];

        gameObject.SetActive(true);
    }

    public void CloseMenu() {
        gameObject.SetActive(false);
    }
}
