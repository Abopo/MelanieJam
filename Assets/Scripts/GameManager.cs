using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    bool _alive;

    public GameResources gameResources;

    PlayerController _player;

    Checkpoint[] _allCheckpoints;

    [SerializeField]
    TextMeshProUGUI _popupText;

    [SerializeField]
    GameObject _deathPopup;

    [SerializeField]
    GameObject _gameEndPopup;

    public bool gameEnd;

    public UnityEvent OnPlayerRespawn;
    public UnityEvent OnGameEnd;

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
        if (_alive) {
            gameResources = gameObject.GetComponent<GameResources>();
            _player = FindObjectOfType<PlayerController>();
            _allCheckpoints = FindObjectsOfType<Checkpoint>();

            // Sort checkpoints
            Array.Sort(_allCheckpoints,
                delegate (Checkpoint x, Checkpoint y) { return x.index.CompareTo(y.index); });

            HealthMeter.onPlayerDeath.AddListener(OnPlayerDeath);

            OnGameEnd.AddListener(GameEndStuff);

            // Spawn the player at the active checkpoint
            SpawnPlayerAtCheckpoint();
        }
    }

    void SpawnPlayerAtCheckpoint() {
        foreach (Checkpoint check in _allCheckpoints) {
            if (check.activated == true) {
                _player.transform.position = new Vector3(check.transform.position.x, 0, check.transform.position.z);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        // Make sure we have the player
        if(_player == null) {
            _player = FindObjectOfType<PlayerController>();
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            // Teleport player to the next checkpoint in the list
            TeleportToNextCheckpoint();
        }
    }

    void TeleportToNextCheckpoint() {
        for (int i = 0; i < _allCheckpoints.Length; i++) {
            // Find the currently active checkpoint
            if (_allCheckpoints[i].activated == true) {
                // Turn it off
                _allCheckpoints[i].activated = false;

                // Turn on the next one
                if (i < _allCheckpoints.Length - 1) {
                    _allCheckpoints[i + 1].activated = true;
                } else {
                    _allCheckpoints[0].activated = true;
                }

                break;
            }
        }

        // spawn the player at the 'next' checkpoint
        SpawnPlayerAtCheckpoint();
    }

    public void PlayerTouchedCheckpoint(Checkpoint checkpoint) {
        foreach (Checkpoint check in _allCheckpoints) {
            if (check == checkpoint) {
                check.activated = true;
            } else {
                check.activated = false;
            }
        }
    }

    void OnPlayerDeath() {
        // Remove player control
        _player.hasControl = false;

        // Show death popup
        _deathPopup.SetActive(true);
    }

    public void RespawnPlayer() {
        // Give player control
        _player.hasControl = true;

        // Close death popup
        _deathPopup.SetActive(false);

        // Reset position to active checkpoint
        SpawnPlayerAtCheckpoint();

        // TODO: Reset states of interactable objects and stuff
        OnPlayerRespawn.Invoke();
    }

    public void ShowTextPopup(string text) {
        _popupText.text = text;

        StartCoroutine(HideTextLater());
    }

    IEnumerator HideTextLater() {
        yield return new WaitForSeconds(5);

        _popupText.text = "";
    }

    void GameEndStuff() {
        _player.hasControl = false;
        gameEnd = true;
        //_gameEndPopup.SetActive(true);
    }

    public void EndGame() {
        // TODO: return to title if there is one

        // For now, reload the scene?
        SceneManager.LoadScene("Credits");
    }
}
