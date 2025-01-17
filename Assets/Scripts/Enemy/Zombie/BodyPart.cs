using System;
using System.Collections.Generic;
using Base.Interface;
using Managers.Manager;
using UnityEngine;

namespace Enemy.Zombie
{
  public class BodyPart : MonoBehaviour, IDamageable
  {
    private Enemy _enemy;

    public BodyPartKey BodyPartKey;
    
    [SerializeField]
    private List<BodyPart> _childBodyParts;

    [SerializeField]
    private GameObject[] _limb;

    [SerializeField]
    private GameObject _woundHole;
    
    [SerializeField]
    private Vector3 _woundLocalPosition;
    
    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
      Rigidbody = GetComponent<Rigidbody>();
      if (_woundHole != null) _woundHole.SetActive(false);

      foreach (BodyPart bodyPart in _childBodyParts) bodyPart.OnBodyPartDestroyed += OnChildObjectDestroyed;
    }

    public void SetManager(Enemy enemy)
    {
      _enemy = enemy;
    }

    public void TakeDamage(int damage, Vector3 forceDirection, Vector3 hitPoint, out bool isHeadshot)
    {
      _enemy.Animator.Hit();
      isHeadshot = BodyPartKey == BodyPartKey.Head;
      ParticleManager.Instance.PlayParticleEffectFromPool(hitPoint, BodyPartKey == BodyPartKey.Head ? VFX.HitZombieHeadShot : VFX.HitZombie);

      float newDamage = damage * GetBodyPartDamageCoefficient();
      bool isDead = _enemy.Health.TakeDamage(newDamage);

      if (!isDead) return;
      
      _enemy.Ragdoll.Death(newDamage, forceDirection, hitPoint, Rigidbody);

      if (_limb.Length == 0) return;
      
      GameObject limbGameObject = Instantiate(_limb[_childBodyParts.Count], transform.position, Quaternion.identity, transform.parent);
      LimbObject limb = limbGameObject.GetComponent<LimbObject>();
      limbGameObject.transform.localRotation = Quaternion.Euler(limb.InitialRotation);
      limb.transform.parent = null;
      limb.Rigidbody.AddForceAtPosition(forceDirection * newDamage / 25, hitPoint, ForceMode.Impulse);

      _woundHole.transform.localPosition = _woundLocalPosition;
      _woundHole.SetActive(true);
      transform.localScale = Vector3.zero;

      for (int i = 0; i < _childBodyParts.Count; i++)
      {
        _childBodyParts[i].DestroyComponents();
      }
        
      DestroyComponents();
    }

    public Action<BodyPart> OnBodyPartDestroyed;
    public void DestroyComponents()
    {
      _enemy.Ragdoll.RemoveBodyPart(this);
      OnBodyPartDestroyed?.Invoke(this);
      OnBodyPartDestroyed = null;
      
      gameObject.SetActive(false);
    }

    private void OnChildObjectDestroyed(BodyPart bodyPart)
    {
      _destroyedBodyParts.Add(bodyPart);
      _childBodyParts.Remove(bodyPart);
    }
    
    private float GetBodyPartDamageCoefficient()
    {
      switch (BodyPartKey)
      {
        case BodyPartKey.Head:
          return 4f;
        case BodyPartKey.UpperBody:
          return 1f;
        case BodyPartKey.LowerBody:
          return 1.25f;
        default:
          return 0.75f;
      }
    }

    private const string _normalLayer = "Enemy";
    private const string targetLayer = "DeadBody";

    public void DisableCollisions()
    {
      int targetLayerIndex = LayerMask.NameToLayer(targetLayer);
      gameObject.layer = targetLayerIndex;
      
      int deadBodyLayer = LayerMask.NameToLayer("DeadBody");
      
      int playerLayer = LayerMask.NameToLayer("Player");
      int characterControllerLayer = LayerMask.NameToLayer("CharacterController");
      int enemyLayer = LayerMask.NameToLayer("Enemy");

      Physics.IgnoreLayerCollision(deadBodyLayer, playerLayer, true);
      Physics.IgnoreLayerCollision(deadBodyLayer, characterControllerLayer, true);
      Physics.IgnoreLayerCollision(deadBodyLayer, enemyLayer, true);
    }

    private readonly List<BodyPart> _destroyedBodyParts = new();
    public void Respawn()
    {
      gameObject.SetActive(true);

      gameObject.layer = LayerMask.NameToLayer(_normalLayer);
      
      if (_woundHole != null) _woundHole.SetActive(false);
      transform.localScale = Vector3.one;
      
      _childBodyParts.AddRange(_destroyedBodyParts);
      _destroyedBodyParts.Clear();
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