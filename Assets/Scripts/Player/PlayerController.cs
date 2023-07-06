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

    // Shop stuff
    public int cash;
    public int shinyRocks = 0;
    public bool hasFloodlight;
    [SerializeField]
    GameObject _floodlight;
    public bool hasSlingshot;
    [SerializeField]
    GameObject _slingObject;
    [SerializeField]
    float _slingCooldown;
    float _slingCooldownTimer = 0f;

    Vector3 _velocity;
    Rigidbody _rigidBody;
    Collider _myCollider;

    PlayerAudio _audio;
    Camera _camera;

    // UI
    HealthMeter _healthMeter;
    DizzyMeter _dizzyMeter;
    CashCounter _cashCounter;

    public HealthMeter HealthMeter { get => _healthMeter; }

    private void Awake() {
    }
    // Start is called before the first frame update
    void Start() {
        _rigidBody = GetComponent<Rigidbody>();

        _ecolocation = GetComponentInChildren<Echolocation>();
        _audio = GetComponentInChildren<PlayerAudio>();
        _healthMeter = GetComponentInChildren<HealthMeter>();
        _dizzyMeter = GetComponentInChildren<DizzyMeter>();
        _cashCounter = GetComponentInChildren<CashCounter>(true);

        _myCollider = GetComponentInChildren<Collider>();

        _camera = GetComponentInChildren<Camera>();
        HealthMeter.onPlayerDeath.AddListener(OnPlayerDeath);

        hasControl = true;
        _bufferTimer = _mouseBuffer;
        _lastMousePos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update() {
        if(hasSlingshot) {
            _slingCooldownTimer += Time.deltaTime;
        }

        if (hasControl) {
            // Items
            if (shinyRocks > 0) {
                if (Input.GetKeyDown(KeyCode.R)) {
                    DropShinyRock();
                }
            }
            if (hasFloodlight) {
                if (Input.GetKeyDown(KeyCode.F)) {
                    ToggleFloodlight();
                }
            }
            if (hasSlingshot) {
                if (Input.GetMouseButtonDown(1)) {
                    FireSlingshot();
                }
            }
        }

        _velocity = Vector3.zero;

    }

    private void FixedUpdate() {
        if (hasControl) {
            CheckInput();

            Movement();

            _bufferTimer += Time.deltaTime;
        } else if (_damaged) {
            _damageTimer += Time.deltaTime;
            if (_damageTimer > _damageTime) {
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
                    // Get pushed away from the object
                    DamagePush(collision.transform);

                    // Take a damage
                    TakeDamage();
                }
            }
        }
    }

    void TakeDamage() {
        // Take one damage
        _healthMeter.TakeDamage();

        // Briefly lose control
        DamageStun();

        _audio.PlayHurtClip();
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
        _damaged = false;
        
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

    public void GetCash() {
        cash++;

        _cashCounter.DisplayCashCount();

        // Play a sound

    }

    void DropShinyRock() {
        if(shinyRocks > 0) {
            Vector3 spawnPos = new Vector3(transform.position.x, 0, transform.position.z);
            Instantiate(Resources.Load("Prefabs/LevelPieces/ShinyRock"), spawnPos, Quaternion.identity);
            shinyRocks--;
        }
    }

    void ToggleFloodlight() {
        if(hasFloodlight) {
            if(_floodlight.activeSelf) {
                _camera.backgroundColor = Color.black;
                _floodlight.SetActive(false);
            } else {
                _camera.backgroundColor = Color.white;
                _floodlight.SetActive(true);
            }
        }
    }

    void FireSlingshot() {
        if (_slingCooldownTimer > _slingCooldown) {
            GameObject _shotObject = Instantiate(_slingObject, transform.position, Quaternion.identity);
            _shotObject.GetComponent<Projectile>().moveSpeed = 10;

            // Shoot towards the target reticle (mouse pos)
            Vector3 targetPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            targetPos.y = 0;
            _shotObject.transform.LookAt(targetPos);

            Physics.IgnoreCollision(_shotObject.GetComponent<Collider>(), _myCollider);

            // Play sound
            _audio.PlaySlingshotClip();

            _slingCooldownTimer = 0;
        }
    }
}
