using UnityEngine;

namespace Actor
{
  public class Flashlight : MonoBehaviour
  {
    [SerializeField]
    private Light _gunLight;
    
    public int minLightIntensity;
    
    public int maxLightIntensity = 50;
    
    public int changingLightIntensity = 5;

    private void Awake()
    {
      _gunLight.enabled = false;
    }

    private void Update()
    {
      if (!_gunLight.enabled) return;
      float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

      switch (scrollDelta)
      {
        case > 0f:
          _gunLight.intensity += changingLightIntensity;
          break;
        case < 0f:
          _gunLight.intensity -= changingLightIntensity;
          break;
      }

      _gunLight.intensity = Mathf.Clamp(_gunLight.intensity, minLightIntensity, maxLightIntensity);
    }

    public void GunLightSwitch()
    {
      _gunLight.enabled = !_gunLight.enabled;
    }
  }
}