using Base.Interface;
using UnityEngine;

namespace Enemy
{
  public class EnemyHealth : MonoBehaviour, IDamageable
  {
    [SerializeField]
    private int _maxHealth = 100;

    public int MaxHealth { get => _maxHealth; private set => _maxHealth = value; }

    public int CurrentHealth { get; private set; }

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    private void OnEnable()
    {
      CurrentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
      print(damage);
      int damageTaken = Mathf.Clamp(damage, 0, CurrentHealth);
      CurrentHealth -= damageTaken;

      if (damageTaken != 0)
      {
        OnTakeDamage?.Invoke(damageTaken);
      }

      if (CurrentHealth == 0 && damageTaken != 0)
      {
        OnDeath?.Invoke(transform.position);
      }
    }
  }
}