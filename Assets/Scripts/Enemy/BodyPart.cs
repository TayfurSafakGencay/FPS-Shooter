using Base.Interface;
using Managers;
using UnityEngine;

namespace Enemy
{
  public class BodyPart : MonoBehaviour, IDamageable
  {
    public BodyPartKey BodyPartKey;

    public Rigidbody Rigidbody { get; private set; }
    
    private Enemy _enemy;

    private void Awake()
    {
      Rigidbody = GetComponent<Rigidbody>();
    }

    public void SetManager(Enemy enemy)
    {
      _enemy = enemy;
    }

    public void TakeDamage(int damage, Vector3 forceDirection, Vector3 hitPoint)
    {
      ParticleManager.Instance.PlayParticleEffectFromPool(hitPoint, VFX.HitZombie);
      
      bool isDead = _enemy.Health.TakeDamage(damage);
      print(BodyPartKey);

      if (isDead)
      {
        _enemy.Ragdoll.Death(damage, forceDirection, hitPoint, Rigidbody);
      }
    }
  }
}