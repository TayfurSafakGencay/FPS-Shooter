using System;
using Guns.Configurators;
using Guns.Enum;
using UnityEngine;
using Utilities;

namespace Actor.Gun
{
  [DisallowMultipleComponent]
  public class PlayerGunSelector : MonoBehaviour
  {
    [SerializeField]
    private Camera _playerCamera;

    [SerializeField]
    private Transform GunParent;

    [SerializeField]
    private GunsList Guns;

    [HideInInspector]
    public GunConfig ActiveGun;

    [HideInInspector]
    public GunConfig SecondaryGun;

    private Action<Transform> _onGunChanged;

    private Player _player;

    [SerializeField]
    private Light _gunLight;
    
    public bool HasGun => ActiveGun != null;
    
    public bool HasSecondaryGun => SecondaryGun != null;

    private void Awake()
    {
      _player = GetComponent<Player>();
      _gunLight.enabled = false;
    }

    public void EquipGun(GunType gunType)
    {
      GunConfig gun = Guns.GunConfigs.Find(g => g.GunType == gunType);

      if (gun == null)
      {
        Debug.LogError($"No Gun found for type {gunType}");
        return;
      }

      if (ActiveGun == null)
      {
        EquipGun(gun, out ActiveGun);
        _onGunChanged?.Invoke(ActiveGun.GetModel().transform);
        ActiveGun.GetModel().SetActive(true);
      }
      else if (SecondaryGun == null)
      {
        EquipGun(gun, out SecondaryGun);
      }
      else
      {
        Destroy(ActiveGun);
        EquipGun(gun, out ActiveGun);
      }
    }
    
    public void AddAmmo(GunType gunType, int ammo)
    {
      if (gunType == ActiveGun.GunType)
      {
        ActiveGun.AmmoConfig.AddAmmo(ammo);
      }
      else if (gunType == SecondaryGun.GunType)
      {
        SecondaryGun.AmmoConfig.AddAmmo(ammo);
      }
      else
      {
        Debug.LogError($"No Gun found for type {gunType}");
      }
    }
    
    private void EquipGun(ICloneable newGun, out GunConfig gunConfigSlot)
    {
      gunConfigSlot = newGun.Clone() as GunConfig;
      gunConfigSlot.Spawn(GunParent, this, _playerCamera, _player);
      gunConfigSlot.StopVisuals();
      gunConfigSlot.GetModel().SetActive(false);
    }

    public async void SwitchGun()
    {
      if (!HasGun || !HasSecondaryGun) return;

      await _player.GetPlayerAnimationController().GunChangingAnimation();
      
      (ActiveGun, SecondaryGun) = (SecondaryGun, ActiveGun);
      ActiveGun.StopVisuals();
      SecondaryGun.GetModel().SetActive(false);
      ActiveGun.GetModel().SetActive(true);

      _onGunChanged?.Invoke(ActiveGun.GetModel().transform);
    }

    public void AddEventListenerOnGunChanged(Action<Transform> action)
    {
      _onGunChanged += action;
    }

    public void GunLightSwitch()
    {
      _gunLight.enabled = !_gunLight.enabled;
    }
  }
}