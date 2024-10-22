using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Guns.Configurators
{
  [CreateAssetMenu(fileName = "Damage Config", menuName = "Tools/Guns/Damage Config", order = 0)]
  public class DamageConfig : ScriptableObject, System.ICloneable
  {
    public ParticleSystem.MinMaxCurve DamageCurve;

    private void Reset()
    {
      DamageCurve.mode = ParticleSystemCurveMode.Curve;
    }

    public int GetDamage(float distance = 0)
    {
      return Mathf.CeilToInt(DamageCurve.Evaluate(distance, Random.value));
    }

    public object Clone()
    {
      DamageConfig clone = CreateInstance<DamageConfig>();

      Utility.CopyValues(this, clone);

      return clone;
    }
  }
}