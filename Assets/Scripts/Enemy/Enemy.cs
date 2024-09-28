using UnityEngine;

namespace Enemy
{
  public class Enemy : MonoBehaviour
  {
    public EnemyHealth Health { get; private set; }
    
    public EnemyRagdoll Ragdoll { get; private set; }

    private void Awake()
    {
      Bindings();
    }

    private void Bindings()
    {
      Health = GetComponent<EnemyHealth>();
      Ragdoll = GetComponent<EnemyRagdoll>();
    }

    private void Start()
    {
      Health.OnDeath += Die;
      Health.OnTakeDamage += TakeDamage;
    }

    private void Die(int damage, Vector3 forceDirection, Vector3 hitPoint)
    {
      Ragdoll.OnDeath(damage, forceDirection, hitPoint);
    }

    private void TakeDamage(int damage)
    {
    }
  }
}