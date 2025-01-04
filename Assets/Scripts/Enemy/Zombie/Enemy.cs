using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Utilities;

namespace Enemy.Zombie
{
    // [RequireComponent(typeof(Outline))]  
  public class Enemy : MonoBehaviour, IOutlineable
  {
    public EnemyHealth Health { get; private set; }
    
    public EnemyRagdoll Ragdoll { get; private set; }
    
    public ZombieAnimator Animator { get; private set; }

    public EnemyAI AI { get; private set; }
    
    public ZombieSound Sound { get; private set; }

    public bool IsDead;
    
    public GameObject Player { get; private set; }

    public Outline Outline;
    
    public ZombieSound ZombieSound { get; private set; }
    
    public List<BodyPart> BodyParts { get; private set; }

    private void Awake()
    {
      Player = GameObject.FindWithTag("Player");
      
      Bindings();
      CloseOutline();
      
      BodyParts = new List<BodyPart>(GetComponentsInChildren<BodyPart>());
    }

    private void Bindings()
    {
      Health = GetComponent<EnemyHealth>();
      Ragdoll = GetComponent<EnemyRagdoll>();
      Animator = GetComponent<ZombieAnimator>();
      AI = GetComponent<EnemyAI>();
      Sound = GetComponent<ZombieSound>();
      ZombieSound = GetComponent<ZombieSound>();
    }
    
    private void CloseOutline()
    {
      Outline.enabled = false;
      Outline.OutlineColor = Color.white;
    }

    private const float OutlineTime = 15f;

    private const float initialInterval = 1f;
    private const float finalInterval = 0.1f;  
    public async void OutlineMesh()
    {
      Outline.enabled = true;

      await Utility.Delay(OutlineTime - 2f);
      
      int blinkCount = Mathf.RoundToInt(4f / ((initialInterval + finalInterval) / 2)); 

      float intervalStep = (initialInterval - finalInterval) / blinkCount;

      Sequence sequence = DOTween.Sequence(); 
        
      for (int i = 0; i < blinkCount; i++)
      {
        float currentInterval = initialInterval - intervalStep * i;

        sequence.AppendCallback(() => Outline.enabled = !Outline.enabled)
          .AppendInterval(currentInterval);
      }

      sequence.OnComplete(() => Outline.enabled = false); 
    }

    public void Respawn()
    {
      IsDead = false;

      Health.ResetHealth();
      AI.ResetAI();
      Ragdoll.Respawn();
      Animator.Respawn();
      ZombieSound.Respawn();

      foreach (BodyPart bodyPart in BodyParts)
      {
        bodyPart.Respawn();
      }
    }
  }
}