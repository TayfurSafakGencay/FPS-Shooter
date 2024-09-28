using System.Linq;
using UnityEngine;

namespace Enemy
{
  public class EnemyRagdoll : MonoBehaviour
  {
    private enum ZombieState
    {
      Walk,
      Ragdoll
    }
    
    private Rigidbody[] _ragdollRigidbodies;

    private Animator _animator;

    private CharacterController _characterController;

    private ZombieState _state = ZombieState.Walk;
    
    private void Awake()
    {
      _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
      _animator = GetComponent<Animator>();
      _characterController = GetComponent<CharacterController>();
      
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

    public void OnDeath(int damage, Vector3 direction, Vector3 hitPoint)
    {
      Vector3 force = damage * direction * 10;
      EnableRagdoll();
      
      Rigidbody hitRb = _ragdollRigidbodies.OrderBy(rb => Vector3.Distance(rb.transform.position, hitPoint)).First();
      
      hitRb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);

      _state = ZombieState.Ragdoll;
    }

    private void DisableRagdoll()
    {
      foreach (Rigidbody rb in _ragdollRigidbodies)
      {
        rb.isKinematic = true;
      }
      
      _animator.enabled = true; 
      _characterController.enabled = true;
    }

    private void EnableRagdoll()
    {
      foreach (Rigidbody rb in _ragdollRigidbodies)
      {
        rb.isKinematic = false;
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
}