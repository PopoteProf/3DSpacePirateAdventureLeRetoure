using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TopDownStupidMonsters : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    [SerializeField] private float _aggroDistance = 15;
    [SerializeField] private float _attackDistance = 1.5f;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _attackDelay = 2;
    [SerializeField] private TopDownShooterControler _player;
    [SerializeField] private float _deathDespawnDealy = 10;
    private CharacterController _characterController;

    private float _attackTimer= 0;
    private bool _isDead;

    private bool _canAttack { get => _attackTimer > _attackDelay; }

    void Start() {
        _characterController = GetComponent<CharacterController>();
    }

   
    void Update() {
        if (_isDead) return;
        if( Vector3.Distance(_player.transform.position, transform.position)<=_aggroDistance) {
            _characterController.Move((_player.transform.position - transform.position).normalized * _moveSpeed*Time.deltaTime);
            if (_characterController.velocity.magnitude > 0.5f)
            {
                transform.forward = _characterController.velocity;
            }
        }

        if (Vector3.Distance(_player.transform.position, transform.position) <= _attackDistance && _canAttack) {
            if (_animator)_animator.SetTrigger("Attack");
            _attackTimer = 0;
        }

        if (!_canAttack) {
            _attackTimer += Time.deltaTime;
        }
        if (_animator)_animator.SetFloat("Speed", _characterController.velocity.magnitude);
    }

    private void OnDrawGizmos() {
        Gizmos.color= Color.red;
        Gizmos.DrawWireSphere(transform.position , _aggroDistance);
        Gizmos.DrawWireSphere(transform.position , _attackDistance);
    }

    public void SetDeath() {
        _characterController.enabled = false;
        _isDead = true;
        Destroy(gameObject , _deathDespawnDealy);
        if (_animator)_animator.SetTrigger("Dead");
    }

    public void SetPlayer(TopDownShooterControler player) {
        _player = player;
    }
}
