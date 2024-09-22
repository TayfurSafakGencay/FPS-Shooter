using Guns;
using Guns.GunParts;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player.Gun.Animations
{
  public class PlayerAnimationController : MonoBehaviour
  {
    private Animator _animator;

    private GunAnimator _gunAnimator;

    private PlayerAction _playerAction;

    private GunPart _gunPart;

    private void Awake()
    {
      GetComponent<PlayerGunSelector>().AddEventListenerOnGunChanged(OnGunChanged);
      _playerAction = GetComponent<PlayerAction>();
    }

    private void OnGunChanged()
    {
      _gunAnimator = GetComponentInChildren(typeof(GunAnimator)) as GunAnimator;
      if (_gunAnimator != null)
      {
        _animator = _gunAnimator.GetAnimator();
        Bindings();

        _gunPart = GetComponentInChildren<GunPart>();
      }
    }
    
    private void Bindings()
    {
      _gunAnimator.AddEventListenerOnReloadEnd(_playerAction.EndReload);
      _gunAnimator.AddEventListenerOnAnimationEvent(OnAnimationEventDispatch);
    }

    private void OnAnimationEventDispatch(AnimationEventKey eventKey)
    {
      switch (eventKey)
      {
        case AnimationEventKey.Detach_Magazine:
          DetachMagazine();
          break;
        case AnimationEventKey.Drop_Magazine:
          DropMagazine();
          break;
        case AnimationEventKey.Attach_Magazine:
          AttachMagazine();
          break;
      }
    }

    #region Reload

    private GameObject _magazine;
      
    private const string RELOAD_TRIGGER = "Reload";
    public void Reload()
    {
      _animator.SetTrigger(RELOAD_TRIGGER);
    }

    private void DetachMagazine()
    {
      _magazine = Instantiate(_gunPart.Magazine, _gunPart.LeftHand, true);
      _gunPart.Magazine.SetActive(false);
    }
    
    private void DropMagazine()
    {
      GameObject droppedMagazine = Instantiate(_magazine, _magazine.transform.position, new Quaternion(0,0,90,1));
      droppedMagazine.AddComponent<Rigidbody>();
      droppedMagazine.AddComponent<BoxCollider>().size = new Vector3(0.025f, 0.2f, 0.075f);
    }

    private void AttachMagazine()
    {
      _gunPart.Magazine.SetActive(true);
      Destroy(_magazine);
    }
    
    #endregion
  }
}