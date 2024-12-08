﻿using UnityEngine;

namespace Enemy.Zombie
{
  public class Enemy : MonoBehaviour
  {
    public EnemyHealth Health { get; private set; }
    
    public EnemyRagdoll Ragdoll { get; private set; }
    
    public ZombieAnimator Animator { get; private set; }

    public EnemyAI AI { get; private set; }
    
    public ZombieSound Sound { get; private set; }

    public bool IsDead;
    
    public GameObject Player { get; private set; }

    private void Awake()
    {
      Player = GameObject.FindWithTag("Player");
      
      Bindings();
    }

    private void Bindings()
    {
      Health = GetComponent<EnemyHealth>();
      Ragdoll = GetComponent<EnemyRagdoll>();
      Animator = GetComponent<ZombieAnimator>();
      AI = GetComponent<EnemyAI>();
      Sound = GetComponent<ZombieSound>();
    }
    
  }
}