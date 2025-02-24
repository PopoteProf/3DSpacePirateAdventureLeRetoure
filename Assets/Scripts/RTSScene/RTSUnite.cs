using System.Diagnostics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.MeshOperations;

[SelectionBase]
public class RTSUnite : MonoBehaviour , IDamagable, IRtsSelectable {
    [SerializeField] protected NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _selecterElement;
    [SerializeField] private Animator _animator;
    [Space(10)] 
    [SerializeField] protected RTSUnitStat _rtsUnitStat;
    [SerializeField] private IDamagable.Alignment _alignment = IDamagable.Alignment.Ally;
    [SerializeField] private int _maxHP=10;
    [SerializeField] private int _currentHP;
    [SerializeField] private bool _destroyOnDeath;
    [Space(10)] 
    [Header("Attaque")]
    [SerializeField] private GameObject _prefabAttackVFX;
    [Tooltip("l'endrois ou le vfx d'attaque sera instancier")]
    [SerializeField] private Transform _attackVFXPos;
    [Tooltip("nombre d'attaque en une second")]
    [SerializeField] private float _attackRate = 1;
    [Tooltip("delay entre le début de l'attaque et l'application des dégàts")]
    [SerializeField] private float _attackDelay=0.5f;
    [SerializeField] private int _attackDamage = 2;
    [SerializeField] private float _attackRange = 2;

    public enum RTSUnitStat {
        Idles,Attack,Dead,Moving,GoToRessource,GoToDepot,Mining  
    }
    
    private bool _isSelected;
    protected bool _isDead;
    private float _attackTimer;
    private bool _hadDealDamage;
    protected bool _isAttacking;
    [SerializeField] private IDamagable _currentTarget;
    
    public bool IsDead() => _isDead;
    public IDamagable.Alignment GetAlignment() => _alignment;
    public Vector3 GetCurrentPosition() => transform.position;
    public bool CanBeSelected() => _alignment == IDamagable.Alignment.Ally;

    public void SetSelected(bool value)
    {

        if (gameObject == null) return;
        _selecterElement.gameObject.SetActive(value);
        _isSelected = value;
    }

    public virtual void SetDestination(Vector3 value) {
        if (_isDead) return;
        _navMeshAgent.SetDestination(value);
    }

    public virtual void SetAttackTarget(IDamagable target)
    {
        if (_isDead) return;
        _currentTarget = target;
    }

    public void Start() {
        if (_currentHP == 0) {
            _currentHP = _maxHP;
        }

        if (_attackDelay > 1 / _attackRate) {
            _attackDelay = 1 / _attackRate;
        }
    }
    protected virtual void Update() {
        if (_isDead) return;

        if (_currentTarget != null&& !_currentTarget.IsDead()) {
           ManageAttackStat();
        }
        else {
            ManageAttack(false);
        }

        if (_animator!=null) {
            _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
        }
    }

    protected void ManageAttackStat() {
        if (IsTargetInRange()) {
            _navMeshAgent.SetDestination(transform.position);
            if( _isAttacking)ManageAttack(true);
            else StartAttack();
        }
        else {
            SetDestination(_currentTarget.GetCurrentPosition());
            ManageAttack(false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (IsDead()) return;
        _currentHP -= damage;
        
        if (_currentHP <= 0) {
            Die();
        }
        else
        {
            if (_animator) _animator.SetTrigger("TakeHit");
        }
    }

    public void Die() {
        _isDead = true;
        _navMeshAgent.enabled = false;
        GetComponent<Collider>().enabled = false;
        if (_animator) _animator.SetTrigger("Dead");
        if (_destroyOnDeath) Destroy(gameObject);
    }

    private bool IsTargetInRange() {
        if (Vector3.Distance(_currentTarget.GetCurrentPosition(), transform.position) <= _attackRange) {
            print(" is in attack range");
            return true;
        }
        return false;
    }

    private void StartAttack() {
        _isAttacking = true;
        if (_animator) _animator.SetTrigger("Attack");
    }
    
    private void ManageAttack(bool isInRange) {
        if (_isAttacking) {
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _attackDelay&& !_hadDealDamage) {
                _hadDealDamage = true;
                if (isInRange) {
                    _currentTarget.TakeDamage(_attackDamage);
                    if (_prefabAttackVFX != null)
                        Instantiate(_prefabAttackVFX, _attackVFXPos.position, Quaternion.identity);
                    if (_currentTarget != null && _currentTarget.IsDead())
                    {
                        SetAttackTarget(null);
                    }
                }
            }
            if (_attackTimer >= 1 / _attackRate) {
               _attackTimer = 0;
               _hadDealDamage = false;
               _isAttacking = false;
            }
        }
    }
}