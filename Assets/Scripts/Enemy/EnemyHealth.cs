using UnityEngine;

namespace Enemy
{
  public class EnemyHealth : MonoBehaviour
  {
    [SerializeField]
    private int _maxHealth = 100;

    public int CurrentHealth { get; set; }

    private void OnEnable()
    {
      CurrentHealth = _maxHealth;
    }
    
    public bool TakeDamage(int damage)
    {
      int damageTaken = Mathf.Clamp(damage, 0, CurrentHealth);
      CurrentHealth -= damageTaken;

      return CurrentHealth <= 0;
    }
  }
}