using System;
using Guns.Configurators;
using Guns.Enum;
using UnityEngine;

namespace Player.Gun
{
  [DisallowMultipleComponent]
  public class PlayerGunSelector : MonoBehaviour
  {
    [SerializeField]
    private Camera _playerCamera;
    
    [SerializeField]
    private GunType Gun;

    [SerializeField]
    private Transform GunParent;

    [SerializeField]
    private GunsList Guns;

    [Header("Runtime Filled")]
    public GunConfig ActiveGun;
    
    private Action _onGunChanged;

    private void Start()
    {
      EquipGun();
      
    }

    private void EquipGun()
    {
      GunConfig gun = Guns.GunConfigs.Find(g => g.GunType == Gun);

      if (gun == null)
      {
        Debug.LogError($"No Gun found for type {Gun}");
        return;
      }

      ActiveGun = gun.Clone() as GunConfig;
      ActiveGun?.Spawn(GunParent, this, _playerCamera);
      _onGunChanged?.Invoke();
    }
    
    public void AddEventListenerOnGunChanged(Action action)
    {
      _onGunChanged += action;
    }
  }
}