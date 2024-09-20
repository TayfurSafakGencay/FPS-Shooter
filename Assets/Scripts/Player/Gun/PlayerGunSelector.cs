using System;
using System.Collections.Generic;
using Guns.Configurators;
using Guns.Enum;
using UnityEngine;

namespace Player.Gun
{
  [DisallowMultipleComponent]
  public class PlayerGunSelector : MonoBehaviour
  {
    [SerializeField]
    private GunType Gun;

    [SerializeField]
    private Transform GunParent;

    [SerializeField]
    private List<GunConfig> Guns;

    [Header("Runtime Filled")]
    public GunConfig ActiveGun;
    
    private Action _onGunChanged;

    private void Start()
    {
      EquipGun();
    }

    private void EquipGun()
    {
      GunConfig gun = Guns.Find(g => g.GunType == Gun);

      if (gun == null)
      {
        Debug.LogError($"No Gun found for type {Gun}");
        return;
      }

      ActiveGun = gun;
      gun.Spawn(GunParent, this);
      _onGunChanged?.Invoke();
    }
    
    public void AddEventListenerOnGunChanged(Action action)
    {
      _onGunChanged += action;
    }
  }
}