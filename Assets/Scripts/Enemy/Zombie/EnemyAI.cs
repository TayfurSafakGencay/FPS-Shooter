using Base.Interface;
using Systems.Chase;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Zombie
{
  public class EnemyAI : MonoBehaviour, ISoundDetector
  {
    private NavMeshAgent _navMeshAgent;
    
    private Transform _target;

    private bool _noticed;
    
    private const float _maxDistanceForChase = 10;

    private Enemy _enemy;

    private void Awake()
    {
      _enemy = GetComponent<Enemy>();
      
      _navMeshAgent = GetComponent<NavMeshAgent>();
      _target = GameObject.FindWithTag("Player").transform;

      _navMeshAgent.speed = 4;
      _navMeshAgent.angularSpeed = 360;
      _navMeshAgent.stoppingDistance = 2;
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

        _enemy.Animator.Walk();
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
    
    public bool IsNoticed()
    {
      return _noticed;
    }
  }
}