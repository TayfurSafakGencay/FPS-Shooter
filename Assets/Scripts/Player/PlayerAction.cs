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

    private FirstPersonController _firstPersonController;
    
    private bool _isReloading;

    private void Awake()
    {
      _playerAnimationController = GetComponent<PlayerAnimationController>();
      _firstPersonController = GetComponent<FirstPersonController>();
    }

    private void Update()
    {
      FireAction();
      ReloadAction();
    }

    private void FireAction()
    {
      if (_isReloading || _firstPersonController.GetIsRunning()) return;
      
      bool conditions = Mouse.current.leftButton.isPressed 
                        && Application.isFocused && _gunSelector.ActiveGun != null;
      _gunSelector.ActiveGun.Tick(conditions);

      if (conditions)
      {
        _firstPersonController.ApplyRecoil(_gunSelector.ActiveGun.ShootConfig.GetNormalSpread());
      }
      else
      {
        _firstPersonController.StopFiring();
      }
    }

    public void Reloading(int section)
    {
      _gunSelector.ActiveGun.Reloading(section);
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