using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndTrigger : MonoBehaviour {

    public float breakForce;
    public float breakRadius;

    [SerializeField]
    GameObject _backWall;

    [SerializeField]
    Light _outsideLight;

    Rigidbody[] _crackRocks;

    Transform _camera;

    // Start is called before the first frame update
    void Start() {
        _crackRocks = transform.parent.GetComponentsInChildren<Rigidbody>();
        _camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _outsideLight = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update() {
        // For testing
        if(Input.GetKeyDown(KeyCode.K)) {
            GameEnd();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Monster") {
            // End the game
            GameEnd();
        }
    }

    void GameEnd() {
        // Slow down time
        Time.timeScale = 0.9f;

        // Move camera to good position
        _camera.GetComponent<SlideTo>().LerpToPos(new Vector3(-18, 50, 169), false, true);

        _backWall.SetActive(false);

        // Have the wall break open
        foreach (Rigidbody body in _crackRocks) {
            body.isKinematic = false;
            body.AddExplosionForce(breakForce, transform.position, breakRadius);
        }

        // Lerp the camera render color and the outside light
        StartCoroutine(LightingLerp());

        // Eventually transition to credits or something

        // For now just popping up a menu
        GameManager.instance.OnGameEnd.Invoke();
    }

    IEnumerator LightingLerp() {
        // Set camera render color to black
        Camera mainCamera = _camera.GetComponent<Camera>();
        mainCamera.backgroundColor = Color.black;

        // Set up the lerp timers
        float lerpTime = 3f;
        float lerpTimer = 0f;
        float colorValue = 0f;

        while (lerpTimer < lerpTime) {
            lerpTimer += Time.deltaTime;
            colorValue = Mathf.Lerp(0, 1, lerpTimer / lerpTime);
            mainCamera.backgroundColor = new Color(colorValue, colorValue, colorValue);
            _outsideLight.intensity = Mathf.Lerp(0, 5, lerpTimer / lerpTime);

            yield return null;
        }

        mainCamera.backgroundColor = new Color(1, 1, 1);
        _outsideLight.intensity = 5;

        yield return new WaitForSeconds(1f);

        // Go to credits
        GameManager.instance.EndGame();
    }
}
