using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    [SerializeField]
    GameObject _optionsMenu;

    [SerializeField]
    GameObject _quitMenu;

    bool _hasFocus = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
    }

    public void ActivateMenu() {
        if(!_hasFocus) {
            OpenMenu();
        } else {
            CloseMenu();
        }
    }

    public void OpenMenu() {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        _hasFocus = true;
    }

    public void CloseMenu() {
        if (_hasFocus) {
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);
            _hasFocus = false;
        }
    }

    public void OpenOptions() {
        if (_hasFocus) {
            _optionsMenu.SetActive(true);
            _hasFocus = false;
        }
    }

    public void CloseOptions() {
        _hasFocus = true;
    }

    public void QuitGame() {
        if (_hasFocus) {
            // First open warning menu
            _quitMenu.SetActive(true);
            _hasFocus = false;
        }
    }

    public void ReallyQuit() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Title");
    }

    public void CancelQuit() {
        // Close warning menu
        _quitMenu.SetActive(false);
    }
}
