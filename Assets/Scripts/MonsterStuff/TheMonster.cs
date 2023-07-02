using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MONSTER_STATE { IDLE, WALKING, CHARGING, SEARCHING, NUM_STATES };

public class TheMonster : CollisionObject {


    MONSTER_STATE _curState;

    [SerializeField]
    int chargeThreshold;
    int _echoHits = 0;
    float _echoForgetTime = 0.1f;
    float _echoforgetTimer = 0f;

    public float chargeSpeed;
    Vector3 _lastPlayerPos;
    [SerializeField]
    float _chargeDelay;
    [SerializeField]
    float _chargeCooldown;
    float _chargeTimer;
    bool _isCharging;
    [SerializeField]
    Collider _chargeCollider;

    float _walkDelay = 5f;
    float _delayTimer = 0f;

    NavMeshAgent _agent;
    GameObject[] _pathNodes;
    int _nodeIndex;
    int _pathDir = 1;

    float _baseAccel;
    float _runAccel;
    bool _retracedPath; // Done after being hit

    MonsterSounds _sounds;
    PlayerController _playerController;
    GameObject _mainCamera;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        _agent = GetComponent<NavMeshAgent>();
        _baseAccel = _agent.acceleration;
        _retracedPath = true;
        //_pathNodes = GameObject.FindGameObjectsWithTag("MonsterPathNode");

        _chargeCollider.enabled = false;

        _curState = MONSTER_STATE.IDLE;

        _sounds = GetComponent<MonsterSounds>();
        _playerController = FindObjectOfType<PlayerController>();
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //WalkToNextNode();
    }

    // Update is called once per frame
    void Update() {
        switch (_curState) {
            case MONSTER_STATE.IDLE:
                IdleUpdate();
                break;
            case MONSTER_STATE.WALKING:
                WalkingUpdate();
                break;
            case MONSTER_STATE.CHARGING:
                ChargingUpdate();
                break;
            case MONSTER_STATE.SEARCHING:
                break;
        }

        ForgetEchoHits();
    }

    void ForgetEchoHits() {
        if (_echoHits > 0) {
            _echoforgetTimer += Time.deltaTime;
            if (_echoforgetTimer > _echoForgetTime) {
                _echoHits--;
                _echoforgetTimer = 0f;
            }
        }
    }

    void WalkToNextNode() {
        _curState = MONSTER_STATE.WALKING;

        // Make sure we're not stopped
        _agent.isStopped = false;

        if (_agent.acceleration != _baseAccel) {
            _agent.acceleration = _baseAccel;
        }

        // Progress down the path
        _nodeIndex += _pathDir;
        if (_nodeIndex >= _pathNodes.Length) {
            _pathDir = -1;
            _nodeIndex--;
        } else if (_nodeIndex <= 0) {
            _pathDir = 1;
            _nodeIndex = 0;
        }

        _agent.SetDestination(_pathNodes[_nodeIndex].transform.position);
    }

    void IdleUpdate() {
        // If we have a path
        if (_pathNodes != null && _pathNodes.Length > 0) {
            // wait a small delay before walking
            _delayTimer += Time.deltaTime;
            if (_delayTimer > _walkDelay) {
                WalkToNextNode();

                // Play a sound
                _sounds.ReturnToPath();

                _delayTimer = 0;
            }
        }
    }

    void WalkingUpdate() {
        if (!_agent.pathPending) {
            if (_agent.remainingDistance <= _agent.stoppingDistance) {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f) {
                    // Done
                    WalkToNextNode();
                    _retracedPath = true;
                }
            }
        }

        _chargeTimer += Time.deltaTime;
    }

    void ChargingUpdate() {
        Debug.DrawLine(transform.position, _lastPlayerPos);

        if (_isCharging) {
            // Face player position
            //transform.LookAt(_lastPlayerPos);

            // Run forward until we hit something
            transform.Translate(Vector3.forward * chargeSpeed * Time.deltaTime);
        }
    }

    void SearchingUpdate() {

    }

    protected override void OnParticleCollision(GameObject other) {
        base.OnParticleCollision(other);

        if (_curState != MONSTER_STATE.CHARGING && _chargeTimer > _chargeCooldown) {
            // Keep track of how many particles hit us
            _echoHits++;

            // Charge at player position if too many
            if (_echoHits >= chargeThreshold) {
                StartChargeSequence();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "MonsterPathNode") {
            // Go to the next node
            //WalkToNextNode();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(_curState == MONSTER_STATE.CHARGING && _isCharging) {
            // If we're charging and hit a collision object
            if(collision.gameObject.layer == LayerMask.NameToLayer("CollisionObject")) {
                ChargeCollision(collision.gameObject);
            }
        }
        if(collision.gameObject.tag == "Damage") {
            if (_retracedPath) {
                // Stop current pathing objective, and go back to previous one?
                RunFromDamage();
            }
        }
    }
    
    void StartChargeSequence() {
        _echoHits = 0;

        _curState = MONSTER_STATE.CHARGING;

        // Stop pathing agent
        _agent.isStopped = true;

        // Get player position
        _lastPlayerPos = _playerController.transform.position;

        // Start charging after a delay
        StartCoroutine(ChargeStartup());
    }

    IEnumerator ChargeStartup() {
        // Play some sort of sound
        _sounds.ChargeSetup();

        _delayTimer = 0f;
        while(_delayTimer < _chargeDelay) {
            _delayTimer += Time.deltaTime;

            // Face player position
            transform.LookAt(_lastPlayerPos);

            yield return null;
        }

        // Charge
        ChargeAtPlayer();
    }
    void ChargeAtPlayer() {
        _isCharging = true;

        _chargeCollider.enabled = true;

        // Play some sounds?
        _sounds.ChargeStart();
    }

    void ChargeCollision(GameObject collision) {
        // Stop the charge
        EndCharge();

        // Play sound
        _sounds.PlayChargeHit(collision);

        // Shake Camera
        _mainCamera.GetComponent<ShakeableTransform>().StartShake(0.5f);
    }

    void EndCharge() {
        _isCharging = false;
        _chargeCollider.enabled = false;
        _curState = MONSTER_STATE.IDLE;
    }

    void RunFromDamage() {
        // Make sure we're not charging
        _isCharging = false;

        // Reverse the path
        _pathDir = -_pathDir;
        _nodeIndex += _pathDir;
        WalkToNextNode();

        // Up our acceleration to get away from the danger
        _agent.acceleration = 50;

        // Ignore danger until we've reversed the path enough
        _retracedPath = false;

        // Play sound
        _sounds.HurtClip();
    }

    public void ChangePath(MonsterPath path) {
        if (_pathNodes != path.pathNodes) {
            // Get the path objects from the parent
            _pathNodes = path.pathNodes;

            _nodeIndex = 0;
            WalkToNextNode();
        }
    }
}
