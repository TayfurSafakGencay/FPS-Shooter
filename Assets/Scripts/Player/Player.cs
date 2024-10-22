using System;
using UnityEngine;

namespace Player
{
  public class Player : MonoBehaviour
  {
    private FirstPersonController _firstPersonController;

    public Action<float> OnFire;
    
    private void Awake()
    {
      _firstPersonController = GetComponent<FirstPersonController>();
    }

    public FirstPersonController GetFirstPersonController()
    {
      return _firstPersonController;
    }

    public void Fire(Vector3 spread)
    {
      float maxValue = Mathf.Max(spread.x, spread.y, spread.z);

      OnFire?.Invoke(maxValue);
    }
  }
}