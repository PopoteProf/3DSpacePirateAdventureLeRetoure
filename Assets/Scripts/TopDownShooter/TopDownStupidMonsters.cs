using System;
using TMPro;
using UnityEditorInternal;
using UnityEngine;


public class TopDownStupidMonsters : MonoBehaviour
{

    [SerializeField] protected Animator _animator;
    [Space(10)]
    [SerializeField] protected float _aggroDistance = 15;
    [SerializeField] protected float _attackDistance = 1.5f;
    [SerializeField] protected float _moveSpeed = 3f;
    [SerializeField] protected float _attackDelay = 2;
    [SerializeField] protected TopDownShooterControler _player;
    [SerializeField] protected float _deathDespawnDealy = 10;
    

    [Space(10), Header("HP")]
    [SerializeField] protected int _maxHP=3;

    protected int _hp;

    protected float _attackTimer= 0;
    protected bool _isDead;

    protected bool _canAttack { get => _attackTimer > _attackDelay; }

    protected virtual void Start() {
        _hp = _maxHP;
    }
    
    void Update() {
       ManagedMovement();
    }

    protected virtual void ManagedMovement() {
       
    }

    private void OnDrawGizmos() {
        Gizmos.color= Color.red;
        Gizmos.DrawWireSphere(transform.position , _aggroDistance);
        Gizmos.DrawWireSphere(transform.position , _attackDistance);
    }

    public void TakeHit() {
        _hp--;
        if (_hp <= 0) {
            SetDeath();
        }
        else {
            if (_animator)_animator.SetTrigger("Hit");
        }
        
    }
    public virtual void SetDeath() {
        _isDead = true;
        Destroy(gameObject , _deathDespawnDealy);
        if (_animator)_animator.SetTrigger("Dead");
    }

    public void SetAttackDamage() {
        if (_player)_player.TakeDamage();
    }

    public void SetPlayer(TopDownShooterControler player) {
        _player = player;
    }
}