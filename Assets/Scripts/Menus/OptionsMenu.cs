using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    Slider _volumeSlider;
    Toggle _colorBlindToggle;

    [SerializeField]
    GameObject _warningText;

    // Start is called before the first frame update
    void Start() {
        _volumeSlider = GetComponentInChildren<Slider>();
        _colorBlindToggle = GetComponentInChildren<Toggle>();

        _volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1);
        AudioListener.volume = _volumeSlider.value;

        _colorBlindToggle.isOn = PlayerPrefs.GetInt("Colorblind", 0) == 0 ? false : true;

        _warningText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustVolume() {
        AudioListener.volume = _volumeSlider.value;
        PlayerPrefs.SetFloat("MasterVolume", _volumeSlider.value);

        if(_volumeSlider.value <= 0.5f) {
            _warningText.SetActive(true);
        } else {
            _warningText.SetActive(false);
        }
    }

    public void ToggleColorBlind() {
        if (_colorBlindToggle.isOn) {
            PlayerPrefs.SetInt("Colorblind", 1);
        } else {
            PlayerPrefs.SetInt("Colorblind", 0);
        }
    }

    public void CloseMenu() {
        gameObject.SetActive(false);
    }
}
