using System;
using UnityEngine;
using UnityEngine.Events;

namespace Guns
{
  public class GunAnimator : MonoBehaviour
  {
    private Animator _animator;
    
    private AnimationEvent GunAnimationEvent = new();

    private void Awake()
    {
      _animator = GetComponent<Animator>();
    }
    
    public Animator GetAnimator()
    {
      return _animator;
    }

    public void OnAnimationEvent(AnimationEventKey eventKey)
    {
      GunAnimationEvent.Invoke(eventKey);
    }
    
    public void AddEventListenerOnAnimationEvent(UnityAction<AnimationEventKey> action)
    {
      GunAnimationEvent.AddListener(action);
    }

    private Action _onReloadEnd;

    public void EndReload()
    {
      _onReloadEnd?.Invoke();
    }

    public void AddEventListenerOnReloadEnd(Action action)
    {
      _onReloadEnd += action;
    }
  }

  public class AnimationEvent : UnityEvent<AnimationEventKey>
  {
     
  }

  public enum AnimationEventKey
  {
    Detach_Magazine,
    Drop_Magazine,
    Attach_Magazine,
    PULL_TRIGGER,
  }
}