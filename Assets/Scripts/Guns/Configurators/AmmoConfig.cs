using UnityEngine;
using Utilities;

namespace Guns.Configurators
{
  [CreateAssetMenu(fileName = "Ammo Config", menuName = "Tools/Guns/Ammo Config", order = 0)]
  public class AmmoConfig : ScriptableObject, System.ICloneable
  {
    public int MaxAmmo;
    public int ClipSize;

    public int CurrentAmmo;
    public int CurrentClipAmmo;

    public void Reload()
    {
      int maxReloadAmount = Mathf.Min(ClipSize, CurrentAmmo);
      int availableBulletsInCurrentClip = ClipSize - CurrentClipAmmo;
      int reloadAmount = Mathf.Min(maxReloadAmount, availableBulletsInCurrentClip);

      CurrentClipAmmo += reloadAmount;
      CurrentAmmo -= reloadAmount;
    }

    public bool CanReload()
    {
      return CurrentClipAmmo < ClipSize && CurrentAmmo > 0;
    }
    
    public object Clone()
    {
      AmmoConfig clone = CreateInstance<AmmoConfig>();
      
      Utility.CopyValues(this, clone);

      return clone;
    }
    
    public void AddAmmo(int amount)
    {
      CurrentAmmo += amount;
      CurrentAmmo = Mathf.Min(CurrentAmmo, MaxAmmo);
    }
  }
}