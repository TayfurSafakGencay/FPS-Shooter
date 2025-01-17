using UnityEngine;

namespace Enemy.Zombie
{
  public class LimbObject : MonoBehaviour
  {
    public Vector3 InitialRotation;

    public Rigidbody Rigidbody;

    private void Start()
    {
      Destroy(gameObject, 10);
    }
  }
}