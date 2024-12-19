using Base.Interface;
using Managers.Manager;
using UnityEngine;

namespace Enemy.Zombie
{
  public class Limb : MonoBehaviour, IDamageable
  {
    private void Start()
    {
      Destroy(gameObject, 10f);
    }

    public void TakeDamage(int damage, Vector3 forceDirection, Vector3 hitPoint, out bool isHeadshot)
    {
      isHeadshot = false;
      ParticleManager.Instance.PlayParticleEffectFromPool(hitPoint, VFX.HitZombie);
    }
  }
}