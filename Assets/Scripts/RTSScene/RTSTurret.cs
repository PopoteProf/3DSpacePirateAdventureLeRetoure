using UnityEngine;

public class RTSTurret : RTSIldeTarget
{
    [SerializeField] private RTSDetectionZone _rtsDetectionZone;
    [Tooltip("nombre d'attaque en une second")]
    [SerializeField] private float _fireRate = 1;
    [SerializeField] private float _fireRayLifeTime;
    [SerializeField] private int _damage = 2;
    [SerializeField] private LineRenderer _fireLineRender;
    [SerializeField] private Transform _firePoint;

    private IDamagable _currentTarget;

    private float _attackTimer;
    private Vector3 _targetLastPos;

    private bool _isAttacking {
        get => _attackTimer != 0;
    }

    private void Update() {
        if (!_isAttacking) {
            _currentTarget = _rtsDetectionZone.GetFirstTarget();
            if (IsValideTarget()) StartAttack();
        }
        else {
            ManageFire();
        }
        
        

    }

    private void StartAttack() {
        _fireLineRender.enabled =true;
        _targetLastPos = _currentTarget.GetCurrentPosition();
        SetFireLineRenderer(true);
        _currentTarget.TakeDamage(_damage);
        _attackTimer = 1 / _fireRate;

    }

    private void ManageFire() {
        if (IsValideTarget()) _targetLastPos = _currentTarget.GetCurrentPosition();
        
        Vector3 dir = _targetLastPos - transform.position;
        dir.y = 0;
        transform.forward = dir;

        _attackTimer -= Time.deltaTime;
        SetFireLineRenderer(_attackTimer > (1/_fireRate)-_fireRayLifeTime);

        if (_attackTimer <= 0) {
            _attackTimer = 0;
        }
        
    }

    private void SetFireLineRenderer(bool value) {
        _fireLineRender.enabled = value;
        
        if (!value) return;
        
        _fireLineRender.SetPositions( new []{_firePoint.position, _targetLastPos});

    }

    private bool IsValideTarget() {
        if (_currentTarget == null) return false;
        if (_currentTarget.IsDead()) return false;
        return true;
    }
}