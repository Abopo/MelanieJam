using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    [SerializeField]
    GameObject ammo;

    [SerializeField]
    float _shootTime;
    float _shootTimer;

    [SerializeField]
    float _shootSpeed;

    bool _isShooting;

    GameObject _shotObject;

    Collider _myCollider;

    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start() {
        _myCollider = GetComponentInChildren<Collider>();
        _audioSource = GetComponent<AudioSource>();

        _isShooting = true;
    }

    // Update is called once per frame
    void Update() {
        if (_isShooting) {
            _shootTimer += Time.deltaTime;
            if (_shootTimer > _shootTime) {
                Shoot();
            }
        }
    }

    void Shoot() {
        _shotObject = Instantiate(ammo, transform);
        _shotObject.transform.localPosition = Vector3.zero;
        _shotObject.GetComponent<Projectile>().moveSpeed = _shootSpeed;

        Physics.IgnoreCollision(_shotObject.GetComponent<Collider>(), _myCollider);

        _audioSource.Play();
        _shootTimer = 0f;
    }
}
