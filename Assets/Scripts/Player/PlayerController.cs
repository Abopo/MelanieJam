using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    bool _hasControl;

    [SerializeField]
    float _moveSpeed;

    [SerializeField]
    float mouseSensitivity;
    Vector3 _lastMousePos;
    float mouseDist;

    [SerializeField]
    ParticleSystem _ecolocation;

    [SerializeField]
    float damagePushForce;
    float _damageTime = 0.2f;
    float _damageTimer = 0f;

    Vector3 _velocity;
    Rigidbody _rigidBody;

    HealthMeter _healthMeter;
    DizzyMeter _dizzyMeter;

    // Start is called before the first frame update
    void Start() {
        _rigidBody = GetComponent<Rigidbody>();

        _healthMeter = GetComponentInChildren<HealthMeter>();
        _dizzyMeter = GetComponentInChildren<DizzyMeter>();
    }

    // Update is called once per frame
    void Update() {
    }

    private void FixedUpdate() {
        _velocity = Vector3.zero;

        if (_hasControl) {
            CheckInput();

            Movement();
        } else {
            _damageTimer += Time.deltaTime;
            if(_damageTimer > _damageTime) {
                _hasControl = true;
            }
        }
    }

    void CheckInput() {
        if (Input.GetKey(KeyCode.W)) {
            _velocity.z += _moveSpeed;
        }
        if (Input.GetKey(KeyCode.S)) {
            _velocity.z -= _moveSpeed;
        }
        if (Input.GetKey(KeyCode.D)) {
            _velocity.x += _moveSpeed;
        }
        if (Input.GetKey(KeyCode.A)) {
            _velocity.x -= _moveSpeed;
        }

        // Mouse
        if(!_dizzyMeter.isDizzy) {
            mouseDist = Vector3.Distance(Input.mousePosition, _lastMousePos);
            if (mouseDist >= mouseSensitivity) {
                if (!_ecolocation.isEmitting) {
                    _ecolocation.Play();
                }
                _dizzyMeter.IncreaseMeter();
            } else if(_ecolocation.isPlaying){
                _ecolocation.Stop();
            }
            _lastMousePos = Input.mousePosition;
        } else if (_ecolocation.isPlaying) {
            _ecolocation.Stop();
        }
    }

    void Movement() {
        _rigidBody.velocity = _velocity;
    }

    private void OnCollisionEnter(Collision collision) {
        if(_hasControl && collision.gameObject.layer == LayerMask.NameToLayer("CollisionObject")) {
            if(collision.gameObject.GetComponent<CollisionObject>().objectType == OBJECT_TYPE.DANGER) {
                // Take one damage
                _healthMeter.TakeDamage();

                // Get pushed away from the object
                DamagePush(collision.transform);

                // Briefly lose control
                LoseControl();
            }
        }
    }

    void DamagePush(Transform source) {
        Vector3 dir = transform.position - source.position;
        dir.y = 0;
        dir.Normalize();

        _rigidBody.AddForce(dir * damagePushForce, ForceMode.VelocityChange);
    }

    void LoseControl() {
        _hasControl = false;
        _damageTimer = 0;
    }
}
