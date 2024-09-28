using UnityEngine;

namespace Base.Interface
{
  public interface IDamageable
  {
    public int MaxHealth { get; }

    public int CurrentHealth { get; }

    public delegate void TakeDamageEvent(int damage);
    public event TakeDamageEvent OnTakeDamage;

    public delegate void DeathEvent(int damage, Vector3 forceDirection, Vector3 hitPoint);
    public event DeathEvent OnDeath;
    
    public void TakeDamage(int damage, Vector3 forceDirection, Vector3 hitPoint);
  }
}