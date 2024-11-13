using System;
using System.Collections.Generic;
using Actor.Gun;
using Actor.Gun.Animations;
using UnityEngine;
using UserInterface.Panel;

namespace Actor
{
  public class Player : MonoBehaviour
  {
    private FirstPersonController _firstPersonController;

    private PlayerAnimationController _playerAnimationController;

    private PlayerScreenPanel _playerScreenPanel;

    private PlayerGunSelector _playerGunSelector;
    
    private Inventory _inventory;
    
    private PlayerLoot _playerLoot;

    public Action<float> OnFire;

    public Action<bool> OnHit;

    [SerializeField]
    private List<Camera> _cameras;

    private bool _isScoped;

    private void Awake()
    {
      _firstPersonController = GetComponent<FirstPersonController>();
      _playerAnimationController = GetComponent<PlayerAnimationController>();
      _playerGunSelector = GetComponent<PlayerGunSelector>();
      _playerLoot = GetComponent<PlayerLoot>();
      _inventory = GetComponent<Inventory>();
    }

    public void Fire(Vector3 spread)
    {
      float maxValue = Mathf.Max(spread.x, spread.y, spread.z);

      OnFire?.Invoke(maxValue);
    }

    public void Hit(bool headshot)
    {
      OnHit?.Invoke(headshot);
    }

    private const float _standardFov = 60;

    private void SetCameraFov(float fov = _standardFov)
    {
      foreach (Camera cam in _cameras)
      {
        cam.fieldOfView = fov;
      }
    }

    public Action<bool> OnScopeOpened;
    public void ChangeScope(float fov = _standardFov)
    {
      _isScoped = !_isScoped;
      SetCameraFov(_isScoped ? fov : _standardFov);

      _playerAnimationController.GetGunPart().OnScopeOpen(_isScoped);
      
      OnScopeOpened?.Invoke(_isScoped);
    }

    public bool GetIsScoped()
    {
      return _isScoped;
    }

    public FirstPersonController GetFirstPersonController()
    {
      return _firstPersonController;
    }

    public PlayerGunSelector GetPlayerGunSelector()
    {
      return _playerGunSelector;
    }
    
    public void SetPlayerScreenPanel(PlayerScreenPanel playerScreenPanel)
    {
      _playerScreenPanel = playerScreenPanel;
    }
    
    public PlayerScreenPanel GetPlayerScreenPanel()
    {
      return _playerScreenPanel;
    }
    
    public PlayerLoot GetPlayerLoot()
    {
      return _playerLoot;
    }
    
    public Inventory GetInventory()
    {
      return _inventory;
    }
  }
}