using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PICKUPS { SHIELD, MONEY, SHINY_ROCK, NUM_PICKUPS };

public class Pickup : InteractableObject {

    public PICKUPS pickupType;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }

    protected override void OnInteract() {
        base.OnInteract();

        // Give the player an item
        switch (pickupType) {
            case PICKUPS.SHIELD:
                GetShield();
                break;
            case PICKUPS.MONEY:
                GetMoney();
                break;
            case PICKUPS.SHINY_ROCK:
                PickupRock();
                break;
        }
    }

    void GetShield() {
        // spawn a shield
        Object shieldObj = Resources.Load("Prefabs/Shield");
        Instantiate(shieldObj);

        // Show tutorial
        GameManager.instance.ShowTextPopup("LMB to rotate shield.");

        Destroy(gameObject);
    }

    void GetMoney() {
        _playerController.GetCash();
        _audioSource.Play();

        StartCoroutine(DestroyAfter());
    }

    void PickupRock() {
        _playerController.shinyRocks += 1;
     
        Destroy(gameObject);
    }

    IEnumerator DestroyAfter() {
        while(_audioSource.isPlaying) {
            yield return null;
        }

        Destroy(gameObject);
    }
}
