using System;
using System.Collections.Generic;
using Actor.Gun;
using Actor.Gun.Animations;
using DG.Tweening;
using Managers.Manager;
using PostProcess;
using UnityEngine;
using UserInterface.Panel;
using Utilities;

namespace Actor
{
  public class Player : MonoBehaviour
  {
    private FirstPersonController _firstPersonController;

    private PlayerAnimationController _playerAnimationController;

    private PlayerScreenPanel _playerScreenPanel;

    private PlayerGunSelector _playerGunSelector;
    
    private PlayerHealth _playerHealth;
    
    private Inventory _inventory;
    
    private PlayerLoot _playerLoot;
    
    private PlayerStamina _playerStamina;
    
    private BreathSound _breathSound;
    
    [SerializeField]
    private Flashlight _flashlight;
    
    [SerializeField]
    private PlayerRadar _playerRadar;
    
    [SerializeField]
    private MakeSound _makeSound;

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
      _playerHealth = GetComponent<PlayerHealth>();
      _playerStamina = GetComponent<PlayerStamina>();
      _breathSound = GetComponent<BreathSound>();
    }

    private void Start()
    {
      SoundManager.Instance.SetVolumeFromPlayerPrefs();

      _makeSound.SetPlayer(this);
    }

    public void Fire(Vector3 spread)
    {
      float maxValue = Mathf.Max(spread.x, spread.y, spread.z);

      OnFire?.Invoke(maxValue);
      _makeSound.MakeSoundAtPosition(SoundDistances.Fire);
    }

    public void Hit(bool headshot)
    {
      OnHit?.Invoke(headshot);
    }

    private const float _standardFov = 60;
    private const float _scopeAnimationTime = 0.1f;

    private void SetCameraFov(float fov = _standardFov)
    {
      foreach (Camera cam in _cameras)
      {
        DOTween.To(() => cam.fieldOfView, x => cam.fieldOfView = x, fov, _scopeAnimationTime)
          .SetEase(Ease.InOutSine);
      }
    }

    public Action<bool> OnScopeOpened;
    public void ChangeScope(float fov = _standardFov)
    {
      _isScoped = !_isScoped;
      _playerAnimationController.OnScopeOpen(_isScoped);

      SetCameraFov(_isScoped ? fov : _standardFov);

      _playerAnimationController.GetGunPart().OnScopeOpen(_isScoped);
      
      OnScopeOpened?.Invoke(_isScoped);
    }

    public async void LevelCompleted()
    {
      GetPlayerAnimationController().EndGame();

      await Utility.Delay(1f);
      
      _playerGunSelector.EmptyHand();
      VignetteEffect.GameCompleted();
      _playerScreenPanel.gameObject.SetActive(false);

      Component[] components = gameObject.GetComponents<Component>();

      foreach (Component component in components)
      {
        switch (component)
        {
          case Transform:
            continue;
          case MonoBehaviour monoBehaviour:
            monoBehaviour.enabled = false;
            break;
        }
      }
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
    
    public PlayerAnimationController GetPlayerAnimationController()
    {
      return _playerAnimationController;
    }
    
    public PlayerHealth GetPlayerHealth()
    {
      return _playerHealth;
    }
    
    public PlayerStamina GetPlayerStamina()
    {
      return _playerStamina;
    }
    
    public BreathSound GetBreathSound()
    {
      return _breathSound;
    }
    
    public PlayerRadar GetPlayerRadar()
    {
      return _playerRadar;
    }
    
    public Flashlight GetFlashlight()
    {
      return _flashlight;
    }
  }
}