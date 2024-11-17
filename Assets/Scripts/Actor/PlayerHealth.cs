using PostProcess;
using UnityEngine;

namespace Actor
{
  public class PlayerHealth : MonoBehaviour
  {
    public float MaxHealth { get; private set; } = 100;

    public float Health { get; private set; }

    public bool IsDead => Health <= 0;

    private void Awake()
    {
      Health = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
      if (IsDead) return;

      Health -= damage;

      DamageEffect.OnHealthChange(Health, MaxHealth);

      if (Health <= 0)
      {
        Die();
      }
    }

    public void Heal(int healAmount)
    {
      if (IsDead) return;

      Health += healAmount;

      if (Health > MaxHealth)
      {
        Health = MaxHealth;
      }
      
      DamageEffect.OnHealthChange(Health, MaxHealth);
    }

    public void Die()
    {
      Component[] components = gameObject.GetComponents<Component>();

      foreach (Component component in components)
      {
        switch (component)
        {
          case Transform:
            continue;
          case MonoBehaviour monoBehaviour:
            monoBehaviour.enabled = false;
            break;
        }
      }
    }
  }
}