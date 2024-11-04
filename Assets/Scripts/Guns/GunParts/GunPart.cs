using System;
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

    public void OnScopeOpen(bool isScopeActive)
    {
      if (SightDot != null)
      {
        SightDot.SetActive(isScopeActive);
      }

      GunHolder.localRotation = Quaternion.Euler(isScopeActive ? new Vector3(6, 0, 0) : new Vector3(6, -2, 0));
    }
  }
}