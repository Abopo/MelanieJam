using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    GameObject _optionsMenu;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartGame() {
        // TODO: Load some story intro scene

        SceneManager.LoadScene("MainCaveScene");
    }

    public void OpenOptions() {
        _optionsMenu.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
