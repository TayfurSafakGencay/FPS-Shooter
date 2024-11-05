using UnityEngine;

namespace Enemy.Zombie
{
  public class Enemy : MonoBehaviour
  {
    public EnemyHealth Health { get; private set; }
    
    public EnemyRagdoll Ragdoll { get; private set; }
    
    public ZombieAnimator Animator { get; private set; }

    private void Awake()
    {
      Bindings();
    }

    private void Bindings()
    {
      Health = GetComponent<EnemyHealth>();
      Ragdoll = GetComponent<EnemyRagdoll>();
      Animator = GetComponent<ZombieAnimator>();
    }
  }
}