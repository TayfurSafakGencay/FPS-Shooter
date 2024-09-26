using UnityEngine;

namespace Guns.GunParts
{
  public class GunPart : MonoBehaviour
  {
    [Header("IK")]
    public Transform LeftHand;

    public Transform LeftIkObject;
    
    public Transform RightIkObject;
    
    [Header("Gun Parts")]
    public GameObject Magazine;
    
    public Vector3 MagazineColliderSize;

  }
}