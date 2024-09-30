using UnityEngine;

namespace Enemy
{
  public class Enemy : MonoBehaviour
  {
    public EnemyHealth Health { get; private set; }
    
    public EnemyRagdoll Ragdoll { get; private set; }

    private void Awake()
    {
      Bindings();
    }

    private void Bindings()
    {
      Health = GetComponent<EnemyHealth>();
      Ragdoll = GetComponent<EnemyRagdoll>();
    }
  }
}