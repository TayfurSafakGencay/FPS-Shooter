using System.Collections.Generic;
using UnityEngine;

namespace Guns.Configurators
{
  [CreateAssetMenu(fileName = "Guns List", menuName = "Tools/Guns/Create Gun List", order = 0)]
  public class GunsList : ScriptableObject
  {
    public List<GunConfig> GunConfigs;
  }
}