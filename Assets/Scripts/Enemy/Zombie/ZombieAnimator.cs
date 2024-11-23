using System.Collections.Generic;
using Actor;
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

    public void Idle()
    {
      _animator.ResetTrigger(AnimationType.Hit.ToString());
      _animator.ResetTrigger(AnimationType.Scream.ToString());
      _animator.ResetTrigger(AnimationType.Attack.ToString());
      _animator.ResetTrigger(AnimationType.Walk.ToString());
      _animator.SetTrigger(AnimationType.Idle.ToString());
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
    
    public void Attack()
    {
      _animator.ResetTrigger(AnimationType.Walk.ToString());
      _animator.SetTrigger(AnimationType.Attack.ToString());
    }
    
    public void AttackAnimationStarted()
    {
      _enemy.Sound.PlayAttackSound();
    }
    
    public void AttackAnimationEnded()
    {
      bool success = _enemy.AI.CheckDistance();

      if (!success) return;
        
      if (_enemy.Player.TryGetComponent(out PlayerHealth playerHealth))
      {
        playerHealth.TakeDamage(Random.Range(10, 20));
      }
    }
    
    public void Walk()
    {
      _animator.ResetTrigger(AnimationType.Attack.ToString());
      _animator.SetTrigger(AnimationType.Walk.ToString());
    }
    
    private enum AnimationType
    {
      Idle,
      Walk,
      Hit,
      Scream,
      Attack,
    }
  }
}