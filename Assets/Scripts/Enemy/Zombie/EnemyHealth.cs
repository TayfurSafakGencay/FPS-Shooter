using UnityEngine;

namespace Enemy.Zombie
{
  public class EnemyHealth : MonoBehaviour
  {
    [SerializeField]
    private float _maxHealth = 100;

    public float CurrentHealth { get; set; }

    private void OnEnable()
    {
      CurrentHealth = _maxHealth;
    }
    
    public bool TakeDamage(float damage)
    {
      float damageTaken = Mathf.Clamp(damage, 0, CurrentHealth);
      CurrentHealth -= damageTaken;

      return CurrentHealth <= 0;
    }

    public void ResetHealth()
    {
      CurrentHealth = _maxHealth;
    }
  }
}