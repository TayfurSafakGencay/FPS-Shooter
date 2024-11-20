using System.Collections;
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

      // StartCoroutine(x());
    }
    
    // IEnumerator x()
    // {
    //   if (IsDead)
    //   {
    //     yield break;
    //   }
    //   yield return new WaitForSeconds(1);
    //   TakeDamage(5);
    //   StartCoroutine(x());
    // }

    public void TakeDamage(float damage)
    {
      if (IsDead) return;

      Health -= damage;

      //TODO: Safak
      // StaminaEffect.OnHealthChange(Health, MaxHealth);

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
      
      // StaminaEffect.OnHealthChange(Health, MaxHealth);
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

    public float GetHealthPercentage()
    {
      return Health / MaxHealth;
    }
  }
}