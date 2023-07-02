using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    bool _alive;

    public GameResources gameResources;

    PlayerController _player;

    Checkpoint[] _allCheckpoints;

    [SerializeField]
    TextMeshProUGUI _popupText;

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
        _player = FindObjectOfType<PlayerController>();
        _allCheckpoints = FindObjectsOfType<Checkpoint>();

        HealthMeter.onPlayerDeath.AddListener(OnPlayerDeath);

        // Spawn the player at the active checkpoint
        SpawnPlayerAtCheckpoint();
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
        // Reset position to active checkpoint
        SpawnPlayerAtCheckpoint();

        // TODO: Reset states of interactable objects and stuff

    }

    public void ShowTextPopup(string text) {
        _popupText.text = text;

        StartCoroutine(HideTextLater());
    }

    IEnumerator HideTextLater() {
        yield return new WaitForSeconds(5);

        _popupText.text = "";
    }
}
