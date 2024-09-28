using System;
using DG.Tweening;
using UnityEngine;

namespace Player
{
  public class CameraOperations : MonoBehaviour
  {
    [SerializeField]
    private Transform _cameraTransform;

    [SerializeField]
    private float _shakeDuration = 0.1f;

    [SerializeField]
    private float _shakeStrength = 0.5f;

    [SerializeField]
    private int _vibrato = 1;

    [SerializeField]
    private float _randomness = 90;

    // private Quaternion _originalRotation;
    
    private Vector3 _originalPosition;
    private void Awake()
    {
      _originalPosition = _cameraTransform.localPosition;
    }

    public void ShakingForShooting()
    {
      // _cameraTransform.DOShakeRotation(_shakeDuration, _shakeStrength, _vibrato, _randomness)
      //   .OnComplete(ResetRotation);
      _cameraTransform.DOShakePosition(_shakeDuration, _shakeStrength, _vibrato, _randomness).OnComplete(() => 
      {
        _cameraTransform.DOLocalMove(_originalPosition, _shakeDuration);
      });
    }

    // private void ResetRotation()
    // {
    //   // _cameraTransform.DOLocalRotateQuaternion(_originalRotation, _shakeDuration);
    // }
  }
}