using UnityEngine;

namespace Guns.Configurators
{
  [CreateAssetMenu(fileName = "Shoot Config", menuName = "Tools/Guns/Shoot Configuration", order = 0)]
  public class ShootConfig : ScriptableObject
  {
    public LayerMask HitMask;
    public Vector3 Spread = new(0.1f, 0.1f, 0.1f);
    public float FireRate = 0.25f;
  }
}