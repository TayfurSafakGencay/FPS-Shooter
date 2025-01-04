using System.Collections.Generic;
using Systems.EndGame;
using UnityEngine;
using UnityEngine.AI;
using Utilities;
using ZombiePool;

namespace Enemy.Zombie
{
  public class EnemyRagdoll : MonoBehaviour
  {
    private Enemy _enemy;
    
    private List<BodyPart> _bodyParts;

    private Animator _animator;

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

    public async void Death(float damage, Vector3 direction, Vector3 hitPoint, Rigidbody hitRb)
    {
      if (!_enemy.IsDead) 
      {
        _enemy.IsDead = true;

        EnableRagdoll();
        _enemy.Sound.PlayDeadSound();

        ChangeAllChildLayers();

        Component[] components = gameObject.GetComponents<Component>();
        
        EndGameSystem.Instance.DeathZombie();

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
      
      Vector3 force = damage * direction;
      
      hitRb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);

      await Utility.Delay(20);
      ZombiePoolManager.Instance.ReturnToPool(_enemy);
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

    public void RemoveBodyPart(BodyPart bodyPart)
    {
      _bodyParts.Remove(bodyPart);
      _removedBodyParts.Add(bodyPart);
    }
    
    private const string _normalLayer = "CharacterController";
    private const string targetLayer = "DeadBody";
    public void ChangeAllChildLayers()
    {
      int targetLayerIndex = LayerMask.NameToLayer(targetLayer);
      gameObject.layer = targetLayerIndex;
      
      foreach (BodyPart bodyPart in _bodyParts)
      {
        bodyPart.DisableCollisions();
      }
      
      int deadBodyLayer = LayerMask.NameToLayer("DeadBody");
      int playerLayer = LayerMask.NameToLayer("Player");
      int characterControllerLayer = LayerMask.NameToLayer("CharacterController");
      int enemyLayer = LayerMask.NameToLayer("Enemy");

      Physics.IgnoreLayerCollision(deadBodyLayer, playerLayer, true);
      Physics.IgnoreLayerCollision(deadBodyLayer, characterControllerLayer, true);
      Physics.IgnoreLayerCollision(deadBodyLayer, enemyLayer, true);
    }

    private readonly List<BodyPart> _removedBodyParts = new();
    public void Respawn()
    {
      gameObject.layer = LayerMask.NameToLayer(_normalLayer);
      _bodyParts.AddRange(_removedBodyParts);
      DisableRagdoll();
      
      Component[] components = gameObject.GetComponents<Component>();
      foreach (Component component in components)
      {
        switch (component)
        {
          case Transform:
            continue;
          case MonoBehaviour monoBehaviour:
            monoBehaviour.enabled = true;
            break;
          case NavMeshAgent navMeshAgent:
            navMeshAgent.enabled = true;
            break;
          case Collider col:
            col.enabled = true;
            break;
        }
      }
    }
  }
}