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
        return;
      }
      
      _navMeshAgent.destination = _target.position;

      if (distance <= _navMeshAgent.stoppingDistance)
      {
        Attack();
      }
    }
    
    private void Attack()
    {
      // Attack
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