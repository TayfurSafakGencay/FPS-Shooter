using UnityEngine;

namespace Base.Interface
{
  public interface IDamageable
  {
    public void TakeDamage(int damage, Vector3 forceDirection, Vector3 hitPoint);
  }
}