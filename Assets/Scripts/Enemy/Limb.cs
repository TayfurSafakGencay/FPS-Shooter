using Base.Interface;
using Managers.Manager;
using UnityEngine;

namespace Enemy
{
  public class Limb : MonoBehaviour, IDamageable
  {
    public void TakeDamage(int damage, Vector3 forceDirection, Vector3 hitPoint)
    {
      ParticleManager.Instance.PlayParticleEffectFromPool(hitPoint, VFX.HitZombie);
    }
  }
}