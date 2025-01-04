using System;
using System.Threading.Tasks;
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
    private GameObject _emptyHand;
    
    public bool HasGun => ActiveGun != null;
    
    public bool HasSecondaryGun => SecondaryGun != null;

    private void Awake()
    {
      _player = GetComponent<Player>();
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
    
    private void EquipGun(ICloneable newGun, out GunConfig gunConfigSlot)
    {
      gunConfigSlot = newGun.Clone() as GunConfig;
      gunConfigSlot.Spawn(GunParent, this, _playerCamera, _player);
      gunConfigSlot.StopVisuals();
      gunConfigSlot.GetModel().SetActive(false);
      _player.GetPlayerScreenPanel().OnGunSwitch();
    }

    public async void SwitchGun()
    {
      if (!HasGun) return;
      
      if (_player.GetInventory().IsRadarUsed)
      {
        _player.GetInventory().UseRadar();
        await Utility.Delay(0.5f);
      }
      
      if (!HasSecondaryGun) return;
      
      await _player.GetPlayerAnimationController().GunChangingAnimation();
      
      (ActiveGun, SecondaryGun) = (SecondaryGun, ActiveGun);
      
      _player.GetPlayerScreenPanel().OnGunSwitch();
      ActiveGun.StopVisuals();
      SecondaryGun.GetModel().SetActive(false);
      ActiveGun.GetModel().SetActive(true);

      _onGunChanged?.Invoke(ActiveGun.GetModel().transform);
    }
    
    public void GetFirstGun()
    {
      _emptyHand.SetActive(false);

      if (!HasGun) return;
      
      ActiveGun.GetModel().SetActive(true);

      _onGunChanged?.Invoke(ActiveGun.GetModel().transform);
    }
    
    public async Task EmptyHand()
    {
      if (HasGun)
      {
        await _player.GetPlayerAnimationController().GunChangingAnimation();
        
        _player.GetPlayerAnimationController().ResetAnimator();

        ActiveGun.GetModel().SetActive(false);
      }

      _emptyHand.SetActive(true);
      _player.GetPlayerAnimationController().ChangeArmAnimator(_emptyHand.transform);
    }

    public void Death()
    {
      _emptyHand.SetActive(false);

      if (HasGun)
      {
        ActiveGun.GetModel().SetActive(false);
      }
      else if (HasSecondaryGun)
      {
        SecondaryGun.GetModel().SetActive(false);
      }
    }
    
    public bool AddAmmo(GunType gunType, int ammo)
    {
      if (!HasGun) return false;
      
      if (gunType == ActiveGun.GunType)
      {
        ActiveGun.AmmoConfig.AddAmmo(ammo);
        return true;
      }

      if (!HasSecondaryGun) return false;
      
      if (gunType == SecondaryGun.GunType)
      {
        SecondaryGun.AmmoConfig.AddAmmo(ammo);
        return true;
      }
      
      return false;
    }

    public void AddEventListenerOnGunChanged(Action<Transform> action)
    {
      _onGunChanged += action;
    }
  }
}