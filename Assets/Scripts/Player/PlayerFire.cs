using Player.Gun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
  public class PlayerFire : MonoBehaviour
  {
    [SerializeField]
    private PlayerGunSelector _gunSelector;

    private void Update()
    {
      if (Mouse.current.leftButton.isPressed && _gunSelector.ActiveGun != null)
      {
        _gunSelector.ActiveGun.Shoot();
      }
    }
  }
}