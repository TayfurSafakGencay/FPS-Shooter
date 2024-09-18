using UnityEngine;

namespace Enemy
{
  public class Enemy : MonoBehaviour
  {
    public EnemyHealth Health { get; private set; }

    private void Awake()
    {
      Bindings();
    }

    private void Bindings()
    {
      Health = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
      Health.OnDeath += Die;
    }

    private void Die(Vector3 position)
    {
      Destroy(gameObject);
    }
  }
}