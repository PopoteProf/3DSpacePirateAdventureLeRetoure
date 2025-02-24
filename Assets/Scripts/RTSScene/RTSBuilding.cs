using System;
using UnityEngine;

[SelectionBase]
public class RTSBuilding : MonoBehaviour, IDamagable, IRtsSelectable
{
    
    [SerializeField] private Transform _selecterElement;
    [Space(10)]
    [SerializeField] private IDamagable.Alignment _alignment = IDamagable.Alignment.Ally;
    [SerializeField] private int _maxHP=10;
    [SerializeField] private int _currentHP;
    [SerializeField] private bool _destroyOnDeath;

    
    
    private bool _isDead;
    private bool _isSelected;
    
    public IDamagable.Alignment GetAlignment() => _alignment;
    public Vector3 GetCurrentPosition() => transform.position;
    public bool IsDead() => _isDead;
    public bool CanBeSelected() => _alignment == IDamagable.Alignment.Ally;
    
    
    public void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
    
    public virtual void SetSelected(bool isSelected)
    {
        _selecterElement.gameObject.SetActive(isSelected);
        _isSelected = isSelected;
    }
}