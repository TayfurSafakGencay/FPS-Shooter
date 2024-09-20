using UnityEngine;

namespace Guns.Configurators
{
  [CreateAssetMenu(fileName = "Ammo Config", menuName = "Tools/Guns/Ammo Config", order = 0)]
  public class AmmoConfig : ScriptableObject
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
  }
}