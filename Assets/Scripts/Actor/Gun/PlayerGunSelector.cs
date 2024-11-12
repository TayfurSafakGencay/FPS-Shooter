using System;
using Guns.Configurators;
using Guns.Enum;
using UnityEngine;
using UnityEngine.Profiling;

namespace Actor.Gun
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
    
    private Player _player;
    
    [SerializeField]
    private Light _gunLight;

    private void Awake()
    {
      _player = GetComponent<Player>();
    }

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
      ActiveGun?.Spawn(GunParent, this, _playerCamera, _player);
      _onGunChanged?.Invoke();
    }
    
    public void AddEventListenerOnGunChanged(Action action)
    {
      _onGunChanged += action;
    }

    public void GunLightSwitch()
    {
      _gunLight.enabled = !_gunLight.enabled;
    }
  }
}