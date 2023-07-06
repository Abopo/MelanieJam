using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotProjectile : Projectile {

    ParticleSystem _particles;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        _particles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    protected override void OnCollisionEnter(Collision collision) {
        if (_particles != null) {
            _particles.Play();
        }
        
        base.OnCollisionEnter(collision);
    }

    protected override IEnumerator WaitForFinish() {
        while (_particles.isPlaying) {
            yield return null;
        }

        Destroy(gameObject);
    }
}
