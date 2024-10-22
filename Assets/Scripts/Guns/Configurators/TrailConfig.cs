using UnityEngine;
using Utilities;

namespace Guns.Configurators
{
  [CreateAssetMenu(fileName = "Trail Config", menuName = "Tools/Guns/Gun Trail Config", order = 1)]
  public class TrailConfig : ScriptableObject, System.ICloneable
  {
    public Material Material;
    public AnimationCurve WidthCurve;
    public float Duration = 0.5f;
    public float MinVertexDistance = 0.1f;
    public Gradient Gradient;
    
    public float MissDistance = 100f;
    public float SimulationSpeed = 100f;
    public object Clone()
    {
      TrailConfig clone = CreateInstance<TrailConfig>();
      
      Utility.CopyValues(this, clone);
      
      return clone;
    }
  }
}