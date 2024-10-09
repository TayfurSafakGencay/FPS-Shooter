using System;
using System.Collections.Generic;
using Base.Interface;
using Managers;
using Managers.Manager;
using UnityEngine;

namespace Enemy
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
    
    [SerializeField]
    private Vector3 _childBodyPartRotation;

    public Rigidbody Rigidbody { get; private set; }

    private CharacterJoint _characterJoint;
    
    private Collider _collider;

    private void Awake()
    {
      Rigidbody = GetComponent<Rigidbody>();
      _characterJoint = GetComponent<CharacterJoint>();
      _collider = GetComponent<Collider>();

      if (_woundHole != null) _woundHole.SetActive(false);

      foreach (BodyPart bodyPart in _childBodyParts) bodyPart.OnBodyPartDestroyed += OnChildObjectDestroyed;
    }

    public void SetManager(Enemy enemy)
    {
      _enemy = enemy;
    }

    public void TakeDamage(int damage, Vector3 forceDirection, Vector3 hitPoint)
    {
      ParticleManager.Instance.PlayParticleEffectFromPool(hitPoint, VFX.HitZombie);
      
      float newDamage = damage * GetBodyPartDamageCoefficient();
      bool isDead = _enemy.Health.TakeDamage(newDamage);

      if (!isDead) return;
      
      _enemy.Ragdoll.Death(newDamage, forceDirection, hitPoint, Rigidbody);

      if (_limb.Length == 0) return;
        
      GameObject limb = Instantiate(_limb[_childBodyParts.Count], transform.position, Quaternion.Euler(0,0,0));
      // limb.GetComponent<Rigidbody>().AddForceAtPosition(forceDirection * newDamage / 10, hitPoint, ForceMode.Impulse);
        
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
      
      Destroy(_collider);
      Destroy(_characterJoint);
      Destroy(Rigidbody);
      Destroy(this);
    }

    private void OnChildObjectDestroyed(BodyPart bodyPart)
    {
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