using Guns.GunParts;
using UnityEngine;

namespace Player.Gun.Animations
{
  public class PlayerAnimationController : MonoBehaviour
  {
    private Animator _animator;

    private ArmAnimator _armAnimator;
    
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
      _armAnimator = GetComponentInChildren(typeof(ArmAnimator)) as ArmAnimator;
      _gunAnimator = GetComponentInChildren(typeof(GunAnimator)) as GunAnimator;
      
      if (_armAnimator != null)
      {
        _animator = _armAnimator.GetAnimator();
        Bindings();

        _gunPart = GetComponentInChildren<GunPart>();
        
        SetInitialGunHeight();
      }
    }
    
    private void Bindings()
    {
      _armAnimator.AddEventListenerOnAnimationEvent(OnAnimationEventDispatch);
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
        case AnimationEventKey.PULL_TRIGGER:
          PullTrigger();
          break;
        case AnimationEventKey.END_RELOAD:
          EndReload();
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
      _playerAction.Reloading(0);
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
      _playerAction.Reloading(1);
      Destroy(_magazine);
    }

    private void PullTrigger()
    {
      _gunAnimator.PlayAnimation(GunAnimationEventKey.ChargingTheBolt);
      _playerAction.Reloading(2);
    }
    
    private void EndReload()
    {
      _playerAction.EndReload();
    }
    
    #endregion

    #region Run & Walk
    
    private bool _isRunning => _firstPersonController.GetIsRunning();

    private bool _movingAnimationValue;

    private Transform _gun;

    private Vector3 _defaultGunPosition;
    
    private Vector3 _leftHandDefaultPosition;
    
    private Vector3 _rightHandDefaultPosition;
    
    private FirstPersonController _firstPersonController;

    private const string _move = "Move";
    private void FixedUpdate()
    {
      if (_isRunning && !_movingAnimationValue)
      {
        _animator.SetBool(_move, true);
        _movingAnimationValue = true;
      }
      else if (!_isRunning && _movingAnimationValue)
      {
        _animator.SetBool(_move, false);
        _movingAnimationValue = false;
      }
    }

    private void SetInitialGunHeight()
    {
      _gun = _gunAnimator.transform;
      _defaultGunPosition = _gun.transform.localPosition;
      _leftHandDefaultPosition = _gunPart.LeftIkObject.localPosition;
      _rightHandDefaultPosition = _gunPart.RightIkObject.localPosition;
      _firstPersonController = GetComponent<FirstPersonController>();
    }
    
    public void ApplyBob(float timer, float bobSpeed, float bobAmount)
    {
      bobAmount /= 8;
      bobSpeed /= 8;
      
      timer += Time.deltaTime * bobSpeed;
      
      _gun.transform.localPosition = new Vector3(
        _defaultGunPosition.x + Mathf.Sin(timer) * bobAmount,
        _defaultGunPosition.y + Mathf.Sin(timer) * bobAmount,
        _defaultGunPosition.z + Mathf.Sin(timer) * bobAmount);
      
      bobAmount /= 1.25f;
      
      _gunPart.LeftIkObject.transform.localPosition = new Vector3(
        _leftHandDefaultPosition.x + Mathf.Sin(timer) * bobAmount,
        _leftHandDefaultPosition.y + Mathf.Sin(timer) * bobAmount,
        _leftHandDefaultPosition.z + Mathf.Sin(timer) * bobAmount);
      
      _gunPart.RightIkObject.transform.localPosition = new Vector3(
        _rightHandDefaultPosition.x + Mathf.Sin(timer) * bobAmount,
        _rightHandDefaultPosition.y + Mathf.Sin(timer) * bobAmount,
        _rightHandDefaultPosition.z + Mathf.Sin(timer) * bobAmount);
    }
    

    #endregion
  }
}