using System;
using Actor;
using Base.Interface;
using Systems.Chase;
using UnityEngine;
using UnityEngine.AI;
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
    
    private ZombieRandomWalkerManager _zombieRandomWalkerManager;
    
    private Player _player;
    
    private void Awake()
    {
      _enemy = GetComponent<Enemy>();
      
      _navMeshAgent = GetComponent<NavMeshAgent>();
      _target = GameObject.FindWithTag("Player").transform;
      
      _navMeshAgent.speed = 4;
      _navMeshAgent.angularSpeed = 360;
      _navMeshAgent.stoppingDistance = 2;
    }

    private void Start()
    {
      _zombieRandomWalkerManager = ZombieRandomWalkerManager.Instance;
      _zombieRandomWalkerManager.RandomlyWalk += RandomWalkDecision;
      
    }

    private void Update()
    {
      if (!_noticed) return;
      
      Chase();        
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("RestrictArea"))
      {
        StopWalkingRandomly();
      }
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

    private void StopWalkingRandomly()
    {
      if (_enemy.IsDead) return;
      if (_noticed) return;
      
      _enemy.Animator.Idle();
    }

    public void ResetAI()
    {
      _noticed = false;
    }

    private const float _maxDistanceForWalk = 55f;
    public void RandomWalkDecision(Vector3 playerPosition)
    {
      if (_enemy.IsDead) return;
      if (_noticed) return;
      if (Random.Range(0, 10) == 0) return;
      if (Vector3.Distance(transform.position, playerPosition) > _maxDistanceForWalk) return;
      
      Vector3 position = _zombieRandomWalkerManager.GetRandomPointOnTerrain(transform.position);
      _navMeshAgent.destination = position;
      
      _navMeshAgent.isStopped = false;
      
      _enemy.Animator.Walk();
    }
  }
}