﻿using UnityEngine;
using UnityEngine.Events;

namespace Player.Gun.Animations
{
  public class ArmAnimator : MonoBehaviour
  {
    private Animator _animator;
    
    private readonly AnimationEvent GunAnimationEvent = new();

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
  }
  
  public enum AnimationEventKey
  {
    Detach_Magazine,
    Drop_Magazine,
    Attach_Magazine,
    END_RELOAD,
  }
  public class AnimationEvent : UnityEvent<AnimationEventKey>
  {
     
  }
}