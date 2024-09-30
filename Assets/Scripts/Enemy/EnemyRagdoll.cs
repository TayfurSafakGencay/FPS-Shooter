﻿using UnityEngine;

namespace Enemy
{
  public class EnemyRagdoll : MonoBehaviour
  {
    private enum ZombieState
    {
      Walk,
      Ragdoll
    }
    
    private BodyPart[] _bodyParts;

    private Animator _animator;

    private ZombieState _state = ZombieState.Walk;

    private CharacterController _characterController;
    
    private void Awake()
    {
      _animator = GetComponent<Animator>();
      _bodyParts = GetComponentsInChildren<BodyPart>();
      _characterController = GetComponent<CharacterController>();

      Enemy enemy = GetComponent<Enemy>();
      for (int i = 0; i < _bodyParts.Length; i++)
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

    public void Death(int damage, Vector3 direction, Vector3 hitPoint, Rigidbody hitRb)
    {
      Vector3 force = damage * direction;
      EnableRagdoll();
      
      // Rigidbody hitRb = _ragdollRigidbodies.OrderBy(rb => Vector3.Distance(rb.transform.position, hitPoint)).First();
      
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
  }
  
  public enum BodyPartKey
  {
    Head,
    UpperBody,
    LowerBody,
    LeftArm,
    LeftForearm,
    RightArm,
    RightForearm,
    LeftUpLeg,
    LeftLeg,
    LeftFoot,
    RightUpLeg,
    RightLeg,
    RightFoot
  }
}