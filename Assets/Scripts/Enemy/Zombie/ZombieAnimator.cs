using System;
using UnityEngine;

namespace Enemy.Zombie
{
  public class ZombieAnimator : MonoBehaviour
  {
    private Animator _animator;

    private void Awake()
    {
      _animator = GetComponent<Animator>();
    }

    private void Start()
    {
      _animator.SetTrigger(AnimationType.Walk.ToString());
    }

    public void Hit()
    {
      _animator.SetTrigger(AnimationType.Hit.ToString());
    }
    
    private enum AnimationType
    {
      Walk,
      Hit,
      Attack,
    }
  }
}