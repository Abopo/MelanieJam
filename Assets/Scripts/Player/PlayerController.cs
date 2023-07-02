using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public bool hasControl;

    [SerializeField]
    float _moveSpeed;

    [SerializeField]
    float mouseSensitivity;
    Vector3 _lastMousePos;
    float mouseDist;

    //[SerializeField]
    //ParticleSystem _ecolocation;

    Echolocation _ecolocation;
    [SerializeField]
    float _mouseBuffer = 0.2f;
    float _bufferTimer = 0f;

    [SerializeField]
    float damagePushForce;
    bool _damaged;
    float _damageTime = 0.2f;
    float _damageTimer = 0f;

    Vector3 _velocity;
    Rigidbody _rigidBody;

    HealthMeter _healthMeter;
    DizzyMeter _dizzyMeter;

    public HealthMeter HealthMeter { get => _healthMeter; }

    private void Awake() {
    }
    // Start is called before the first frame update
    void Start() {
        _rigidBody = GetComponent<Rigidbody>();

        _ecolocation = GetComponentInChildren<Echolocation>();
        _healthMeter = GetComponentInChildren<HealthMeter>();
        _dizzyMeter = GetComponentInChildren<DizzyMeter>();

        HealthMeter.onPlayerDeath.AddListener(OnPlayerDeath);

        hasControl = true;
        _bufferTimer = _mouseBuffer;
        _lastMousePos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update() {
    }

    private void FixedUpdate() {
        _velocity = Vector3.zero;

        if (hasControl) {
            CheckInput();

            Movement();

            _bufferTimer += Time.deltaTime;
        } else if(_damaged) {
            _damageTimer += Time.deltaTime;
            if(_damageTimer > _damageTime) {
                EndDamageStun();
            }
        }

        _lastMousePos = Input.mousePosition;
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
            if (mouseDist >= mouseSensitivity || Input.GetKey(KeyCode.Space)) {
                _ecolocation.PlayEffect();
                _dizzyMeter.IncreaseMeter();
                _bufferTimer = 0f;
            } else if(_bufferTimer >= _mouseBuffer) {
                _ecolocation.StopEffect();
            }
        } else {
            _ecolocation.StopEffect();
        }
    }

    void Movement() {
        _rigidBody.velocity = _velocity;
    }

    private void OnCollisionEnter(Collision collision) {
        if(hasControl) {
            if (collision.gameObject.GetComponent<CollisionObject>() != null) {
                if (collision.gameObject.GetComponent<CollisionObject>().objectType == OBJECT_TYPE.DANGER) {
                    // Take one damage
                    _healthMeter.TakeDamage();

                    // Get pushed away from the object
                    DamagePush(collision.transform);

                    // Briefly lose control
                    DamageStun();
                }
            }
        }
    }

    void DamagePush(Transform source) {
        Vector3 dir = transform.position - source.position;
        dir.y = 0;
        dir.Normalize();

        _rigidBody.AddForce(dir * damagePushForce, ForceMode.VelocityChange);
    }

    void DamageStun() {
        hasControl = false;
        _damaged = true;
        _damageTimer = 0;

        _ecolocation.StopEffect();
    }

    void EndDamageStun() {
        // First do a death check
        if(!_healthMeter.DeathCheck()) {
            // If we're not dead, get control back
            hasControl = true;
        } else {
            // Wait for death stuff to happen
        }
    }

    void OnPlayerDeath() {
        _rigidBody.velocity = Vector3.zero;
        
        _dizzyMeter.ResetMeter();
    }
}
