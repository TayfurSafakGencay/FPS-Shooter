using UnityEngine;

namespace Enemy.Zombie
{
  public class FrontCollisionChecker : MonoBehaviour
  {
    [SerializeField]
    private EnemyAI _enemyAI;

    private GameObject _mySelf;

    private void Awake()
    {
      _mySelf = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject == _mySelf) return;
      
      _enemyAI.ObstacleDetected();
    }
  }
}