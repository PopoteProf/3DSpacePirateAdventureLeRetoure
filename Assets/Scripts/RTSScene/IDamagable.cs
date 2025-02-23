using System.Net.Security;
using UnityEngine;
using UnityEngine.Rendering;

public interface IDamagable
{
    public enum Alignment
    {
        Ally, Ennemy
    }
    public void TakeDamage(int damage);
    public void Die();

    public bool IsDead();

    public Alignment GetAlignment();

    public Vector3 GetCurrentPosition();
}