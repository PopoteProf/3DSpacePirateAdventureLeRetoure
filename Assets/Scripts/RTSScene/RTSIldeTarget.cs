using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class RTSIldeTarget : MonoBehaviour, IDamagable
{
    [SerializeField] private bool _destroyOnDeath;
    [SerializeField] private IDamagable.Alignment _alignment = IDamagable.Alignment.Ennemy;
    [SerializeField] private int _maxHP=10;
    [SerializeField] private int _currentHP;

    private bool _isDead;

    
    public bool IsDead()=> _isDead;
    public IDamagable.Alignment GetAlignment ()=>_alignment;
    public Vector3 GetCurrentPosition() =>  transform.position;
    
    protected virtual void Start() {
        if (_currentHP == 0) {
            _currentHP = _maxHP;
        }
    }

    public void TakeDamage(int damage) {
        _currentHP -= damage;
        if (_currentHP <= 0) {
            Die();
        }
    }
    public void Die() {
        _isDead = true;
        print("Target Is Dead");
        if (_destroyOnDeath) Destroy(gameObject);
    }
}