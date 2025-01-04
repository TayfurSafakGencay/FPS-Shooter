using Base.Interface;
using DG.Tweening;
using Systems.Chase;
using UnityEngine;
using UnityEngine.AI;
using Utilities;
using Random = UnityEngine.Random;

namespace Enemy.Zombie
{
  public class EnemyAI : MonoBehaviour, ISoundDetector
  {
    private NavMeshAgent _navMeshAgent;
    
    private Transform _target;

    private bool _noticed;
    
    private const float _maxDistanceForChase = 40;

    private Enemy _enemy;
    
    private void Awake()
    {
      _enemy = GetComponent<Enemy>();
      
      _navMeshAgent = GetComponent<NavMeshAgent>();
      _target = GameObject.FindWithTag("Player").transform;
      
      _isDestroyed = false;

      _navMeshAgent.speed = 4;
      _navMeshAgent.angularSpeed = 360;
      _navMeshAgent.stoppingDistance = 2;
    }

    private void Start()
    {
      // WalkRandomly();
    }

    private void Update()
    {
      if (!_noticed) return;
      
      Chase();        
    }

    private void Chase()
    {
      float distance = Vector3.Distance(transform.position, _target.position);

      if (distance > _maxDistanceForChase)
      {
        _noticed = false;
        _navMeshAgent.isStopped = true;
        _enemy.Animator.Idle();

        return;
      }
      
      _navMeshAgent.destination = _target.position;

      if (distance <= _navMeshAgent.stoppingDistance)
      {
        Attack();
      }
      else
      {
        _navMeshAgent.updateRotation = true;

        _enemy.Animator.Run();
      }
    }
    
    private void Attack()
    {
      _enemy.Animator.Attack();
      
      _navMeshAgent.updateRotation = false;
      
      Vector3 direction = _target.position - transform.position; 
      direction.y = 0;

      if (direction != Vector3.zero)
      {
        Quaternion targetRotation = Quaternion.LookRotation(direction); 
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); 
      }
    }
    
    public bool CheckDistance()
    {
      float distance = Vector3.Distance(transform.position, _target.position);

      return !(distance > _navMeshAgent.stoppingDistance + 1f);
    }

    public void NoticeSound()
    {
      if (_noticed) return;
      ChaseSystem.ChaseHit();
      _noticed = true;
      _navMeshAgent.isStopped = false;

      _enemy.Animator.Scream();
    }

    private bool _isWalkingRandomly;
    
    private const float _minTimeForRandomWalk = 20;
    
    private const float _maxTimeForRandomWalk = 50;
    
    private const float _repeatTimeForRandomWalk = 60;
    // public async void WalkRandomly()
    // {
    //   if (_enemy.IsDead) return;
    //   if (_isDestroyed) return;
    //
    //   if (_noticed)
    //   {
    //     await Utility.Delay(_repeatTimeForRandomWalk);
    //     WalkRandomly();
    //     return;
    //   }
    //
    //   if (_isWalkingRandomly)
    //   {
    //     if (Random.Range(0, 3) == 0)
    //     {
    //       StopWalkingRandomly();
    //     }
    //
    //     await Utility.Delay(_repeatTimeForRandomWalk);
    //     WalkRandomly();
    //   }
    //
    //   if (_enemy.IsDead) return;
    //   if (_isDestroyed) return;
    //
    //   float randomYRotation = Random.Range(0f, 360f);
    //   Quaternion targetRotation = Quaternion.Euler(0, randomYRotation, 0);
    //
    //   transform.DORotateQuaternion(targetRotation, 0.5f).SetEase(Ease.InOutSine);
    //
    //   _enemy.Animator.Walk();
    //   _isWalkingRandomly = true;
    //
    //   WalkRandomly();
    // }
    
    public void ObstacleDetected()
    {
      if (!_isWalkingRandomly) return;
      
      StopWalkingRandomly();
    }
    
    private void StopWalkingRandomly()
    {
      if (_enemy.IsDead) return;
      
      _enemy.Animator.Idle();
      _isWalkingRandomly = false;
    }

    private bool _isDestroyed;
    private void OnDestroy()
    { 
      _isDestroyed = true;
    }

    public void ResetAI()
    {
      _noticed = false;

      // WalkRandomly();
    }
  }
}