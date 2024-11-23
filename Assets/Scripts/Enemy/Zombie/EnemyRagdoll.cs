using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Zombie
{
  public class EnemyRagdoll : MonoBehaviour
  {
    private enum ZombieState
    {
      Walk,
      Ragdoll
    }

    private Enemy _enemy;
    
    private List<BodyPart> _bodyParts;

    private Animator _animator;

    private ZombieState _state = ZombieState.Walk;

    private CharacterController _characterController;
    
    private void Awake()
    {
      _enemy = GetComponent<Enemy>();
      _animator = GetComponent<Animator>();
      _bodyParts = new List<BodyPart>(GetComponentsInChildren<BodyPart>());
      _characterController = GetComponent<CharacterController>();

      Enemy enemy = GetComponent<Enemy>();
      for (int i = 0; i < _bodyParts.Count; i++)
      {
        _bodyParts[i].SetManager(enemy);
      }
    }

    private void Start()
    {
      DisableRagdoll();
    }

    private void Update()
    {
      switch (_state)
      {
        case ZombieState.Walk:
          WalkingBehaviour();
          break;
        case ZombieState.Ragdoll:
          RagdollBehaviour();
          break;
      }
    }

    public void Death(float damage, Vector3 direction, Vector3 hitPoint, Rigidbody hitRb)
    {
      if (!_enemy.IsDead) 
      {
        EnableRagdoll();
        _enemy.Sound.PlayDeadSound();
        
        Component[] components = gameObject.GetComponents<Component>();

        foreach (Component component in components)
        {
          switch (component)
          {
            case Transform:
              continue;
            case MonoBehaviour monoBehaviour:
              monoBehaviour.enabled = false;
              break;
            case NavMeshAgent navMeshAgent:
              navMeshAgent.enabled = false;
              break;
            case Collider col:
              col.enabled = false;
              break;
          }
        }
      }
      
      Vector3 force = damage * direction * 3;
      
      _enemy.IsDead = true;
      
      hitRb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);

      _state = ZombieState.Ragdoll;
    }

    private void DisableRagdoll()
    {
      foreach (BodyPart bodyPart in _bodyParts)
      {
        bodyPart.Rigidbody.isKinematic = true;
      }
      
      _animator.enabled = true; 
      _characterController.enabled = true;
    }

    private void EnableRagdoll()
    {
      foreach (BodyPart bodyPart in _bodyParts)
      {
        bodyPart.Rigidbody.isKinematic = false;
      }
      
      _animator.enabled = false;
      _characterController.enabled = false;
    }

    private void WalkingBehaviour()
    {
    }

    private void RagdollBehaviour()
    {
    }

    public void RemoveBodyPart(BodyPart bodyPart)
    {
      _bodyParts.Remove(bodyPart);
    }
  }
}