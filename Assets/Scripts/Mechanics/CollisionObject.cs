using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OBJECT_TYPE { NONE, DANGER, INTERACTABLE, NUM_TYPES };

public class CollisionObject : MonoBehaviour {

    [SerializeField]
    public OBJECT_TYPE objectType;

    MeshRenderer _myRenderer;

    ParticleSystem _colliderSystem;
    GameObject _tempEcolocationHit;

    [SerializeField]
    bool keepMesh;

    // Start is called before the first frame update
    protected virtual void Start() {
        _myRenderer = GetComponentInChildren<MeshRenderer>();

        if(_myRenderer != null && !keepMesh) {
            _myRenderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnParticleCollision(GameObject other) {
        _colliderSystem = other.GetComponent<ParticleSystem>();
        List<ParticleCollisionEvent> particleCollisionEvents = new List<ParticleCollisionEvent>();
        ParticlePhysicsExtensions.GetCollisionEvents(_colliderSystem, this.gameObject, particleCollisionEvents);

        foreach(ParticleCollisionEvent pce in particleCollisionEvents) {
            // Some intersections are wrong for some reason, so skip those
            if (pce.intersection != Vector3.zero) {
                // Spawn an ecolocation hit object
                _tempEcolocationHit = Instantiate(GameManager.instance.gameResources.ecolocationHit) as GameObject;
                
                // Set it's position to the particle collision
                _tempEcolocationHit.transform.position = pce.intersection;

                // Set it's color based on our type
                switch (objectType) {
                    case OBJECT_TYPE.NONE:
                        // Leave it white?
                        break;
                    case OBJECT_TYPE.DANGER:
                        // Change to red
                        _tempEcolocationHit.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.88f, 0.08f, 0.08f);
                        break;
                    case OBJECT_TYPE.INTERACTABLE:
                        // Change to green
                        _tempEcolocationHit.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.05f, 0.88f, 0.05f);
                        break;
                }
            }

        }
    }
}
