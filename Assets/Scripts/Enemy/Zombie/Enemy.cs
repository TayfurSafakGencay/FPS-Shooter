using DG.Tweening;
using UnityEngine;
using Utilities;

namespace Enemy.Zombie
{
    [RequireComponent(typeof(Outline))]  
  public class Enemy : MonoBehaviour, IOutlineable
  {
    public EnemyHealth Health { get; private set; }
    
    public EnemyRagdoll Ragdoll { get; private set; }
    
    public ZombieAnimator Animator { get; private set; }

    public EnemyAI AI { get; private set; }
    
    public ZombieSound Sound { get; private set; }

    public bool IsDead;
    
    public GameObject Player { get; private set; }
    
    public Outline Outline { get; private set; }

    private void Awake()
    {
      Player = GameObject.FindWithTag("Player");
      
      Bindings();
      CloseOutline();
    }

    private void Bindings()
    {
      Health = GetComponent<EnemyHealth>();
      Ragdoll = GetComponent<EnemyRagdoll>();
      Animator = GetComponent<ZombieAnimator>();
      AI = GetComponent<EnemyAI>();
      Sound = GetComponent<ZombieSound>();
      Outline = GetComponent<Outline>();
    }
    
    private void CloseOutline()
    {
      Outline.enabled = false;
      Outline.OutlineColor = Color.red;
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
  }
}