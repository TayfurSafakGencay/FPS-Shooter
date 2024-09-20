using Player.Gun;
using Player.Gun.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
  public class PlayerAction : MonoBehaviour
  {
    [SerializeField]
    private PlayerGunSelector _gunSelector;
    
    [SerializeField]
    private bool _autoReload = true;

    private PlayerAnimationController _playerAnimationController;

    private bool _isReloading;

    private void Awake()
    {
      _playerAnimationController = GetComponent<PlayerAnimationController>();
    }

    private void Update()
    {
      FireAction();
      ReloadAction();
    }

    private void FireAction()
    {
      _gunSelector.ActiveGun.Tick(Mouse.current.leftButton.isPressed 
         && Application.isFocused && _gunSelector.ActiveGun != null);
    }

    private void ReloadAction()
    {
      if (ShouldManualReload() || ShouldAutoReload())
      {
        _isReloading = true;
        _playerAnimationController.Reload();
      }
    }

    public void EndReload()
    {
      _gunSelector.ActiveGun.EndReload();
      _isReloading = false;
    }
    
    private bool ShouldManualReload()
    {
      return Keyboard.current.rKey.wasReleasedThisFrame
             && _gunSelector.ActiveGun.CanReload() && !_isReloading;
    }

    private bool ShouldAutoReload()
    {
      return _autoReload && _gunSelector.ActiveGun.AmmoConfig.CurrentClipAmmo == 0
                         && _gunSelector.ActiveGun.CanReload() && !_isReloading;
    }
  }
}