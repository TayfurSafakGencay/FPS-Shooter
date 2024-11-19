using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Zombie
{
  public class ZombieAnimator : MonoBehaviour
  {
    private Animator _animator;

    [SerializeField]
    private List<RuntimeAnimatorController> _animators;
    
    private Enemy _enemy;

    private void Awake()
    {
      _animator = GetComponent<Animator>();
      int animator = Random.Range(1, _animators.Count);

      if (animator == 1)
      {
        GetComponent<NavMeshAgent>().radius = 1.1f;
      }
      _animator.runtimeAnimatorController = _animators[animator];
      _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
      // _animator.SetTrigger(AnimationType.Walk.ToString());
    }

    public void Hit()
    {
      _animator.SetTrigger(AnimationType.Hit.ToString());
    }
    
    public void Scream()
    {
      _animator.SetTrigger(AnimationType.Scream.ToString());
      
      _enemy.Sound.PlayScreamSound();
    }
    
    private enum AnimationType
    {
      Walk,
      Hit,
      Scream,
      Attack,
    }
  }
}