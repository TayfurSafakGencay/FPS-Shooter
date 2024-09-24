using UnityEngine;

namespace Guns.GunParts
{
  public class GunAnimator : MonoBehaviour
  {
    private Animator _animator;

    private void Awake()
    {
      _animator = GetComponent<Animator>();
    }

    public void PlayAnimation(GunAnimationEventKey gunAnimationEventKey)
    {
      _animator.SetTrigger(gunAnimationEventKey.ToString());
    }
  }
  
  public enum GunAnimationEventKey
  {
    ChargingTheBolt
  }
}