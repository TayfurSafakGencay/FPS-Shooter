using System;
using System.Collections;
using System.Collections.Generic;
using Managers.Base;
using UnityEngine;

namespace Managers.Manager
{
  public class ParticleManager : ManagerBase
  {
    public static ParticleManager Instance { get; private set; }
    
    [SerializeField]
    private Transform _parent;
    
    [SerializeField]
    private List<ParticleEffectVo> _particleEffectVos;

    private readonly Dictionary<string, Queue<ParticleSystem>> _particleSystemPools = new();

    [SerializeField]
    private List<ParticleEffectVo> _instantiateParticleEffects;

    private readonly Dictionary<string, ParticleSystem> _instantiateParticleEffectsDictionary = new();

    private void Awake()
    {
      if (Instance == null) Instance = this; 
      else Destroy(gameObject);
    }

    public override void Initialize()
    {
      AddAction(ref GameManager.Instance.GameStateChanged, OnGameStateChanged);
    }

    private void OnGameStateChanged()
    {
      if (ActivatedGameStates.Contains(GameManager.Instance.CurrentGameState))
      {
        Activate();
      }
    }

    private void Start()
    {
      for (int i = 0; i < _particleEffectVos.Count; i++)
      {
        ParticleEffectVo particleEffectVo = _particleEffectVos[i];
        CreateParticlesInPool(particleEffectVo.Count, particleEffectVo.ParticleSystem, particleEffectVo.Name);
      }

      for (int i = 0; i < _instantiateParticleEffects.Count; i++)
      {
        ParticleEffectVo particleEffectVo = _instantiateParticleEffects[i];
        _instantiateParticleEffectsDictionary.Add(particleEffectVo.Name.ToString(), particleEffectVo.ParticleSystem);
      }
    }

    public void PlayParticleEffectFromPool(Vector3 position, VFX vfx)
    {
      string particlePoolName = vfx.ToString();

      if (_particleSystemPools[particlePoolName].Count <= 0) return;
      
      ParticleSystem particleInstance = _particleSystemPools[particlePoolName].Dequeue();
      particleInstance.transform.parent = _parent;
      particleInstance.transform.position = position;
      particleInstance.gameObject.SetActive(true);
      particleInstance.Play();

      StartCoroutine(ReturnParticleToPool(particleInstance, _particleSystemPools[particlePoolName]));
    }

    public void PlayParticleEffect(Vector3 position, VFX vfx, float time = 3f)
    {
      ParticleSystem particle = Instantiate(_instantiateParticleEffectsDictionary[vfx.ToString()], position, Quaternion.identity, transform);
      Destroy(particle.gameObject, time);
    }

    private void CreateParticlesInPool(int count, ParticleSystem particle, VFX vfx)
    {
      string poolName = vfx.ToString();

      _particleSystemPools.Add(poolName, new Queue<ParticleSystem>());

      Transform oTransform = transform;
      Vector3 position = oTransform.position;

      for (int i = 0; i < count; i++)
      {
        ParticleSystem particleInstance = Instantiate(particle, position, Quaternion.identity, oTransform);
        particleInstance.gameObject.SetActive(false);
        _particleSystemPools[poolName].Enqueue(particleInstance);
      }
    }

    private static IEnumerator ReturnParticleToPool(ParticleSystem particleInstance, Queue<ParticleSystem> pool)
    {
      yield return new WaitWhile(() => particleInstance.IsAlive(true));
      particleInstance.gameObject.SetActive(false);
      pool.Enqueue(particleInstance);
    }
  }

  public enum VFX
  {
    HitZombie,
  }
  
  [Serializable]
  public struct ParticleEffectVo
  {
    public VFX Name;

    public ParticleSystem ParticleSystem;

    public int Count;
  }
}