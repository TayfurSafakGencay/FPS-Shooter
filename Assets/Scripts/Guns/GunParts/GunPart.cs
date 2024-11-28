using UnityEngine;

namespace Guns.GunParts
{
  public class GunPart : MonoBehaviour
  {
    [Header("IK")]
    public Transform LeftHand;

    [Header("Gun Parts")]
    public GameObject Magazine;

    public Transform GunHolder;

    public GameObject SightDot;
    
    public Vector3 MagazineColliderSize;
    
    private bool _initialPositionSet;
    
    private Vector3 _initialGunPosition;

    private void Awake()
    {
      Renderer[] renderers = GetComponentsInChildren<Renderer>();

      foreach (Renderer rd in renderers)
      {
        rd.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        rd.receiveShadows = false;
      }
    }

    public void OnScopeOpen(bool isScopeActive)
    {
      if (SightDot != null)
      {
        SightDot.SetActive(isScopeActive);
      }

      GunHolder.localRotation = Quaternion.Euler(isScopeActive ? new Vector3(6, 0, 0) : new Vector3(6, -2, 0));
    }

    public void SetInitialPosition(Vector3 position)
    {
      if (_initialPositionSet) return;
      
      _initialGunPosition = position;
      _initialPositionSet = true;
    }
    
    public Vector3 GetInitialPosition()
    {
      return _initialGunPosition;
    }
  }
}