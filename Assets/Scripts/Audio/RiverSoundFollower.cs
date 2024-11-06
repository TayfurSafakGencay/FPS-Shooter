using Actor;
using Cinemachine;
using UnityEngine;
using Utilities;

namespace Audio
{
  public class RiverSoundFollower : MonoBehaviour
  {
    [SerializeField]
    private CinemachinePathBase _path;

    private Transform _player;

    private float _position;
    
    [SerializeField]
    private CinemachinePathBase.PositionUnits _positionUnits = CinemachinePathBase.PositionUnits.PathUnits;

    private bool _isReady;

    private async void Start()
    {
      await Utility.Delay(1f);
      
      _player = FindObjectOfType<Player>().transform;
      _isReady = true;
    }

    private void Update()
    {
      if (!_isReady) return;
      
      SetCartPosition(_path.FindClosestPoint(_player.position, 0, -1, 10));
    }

    private void SetCartPosition(float distanceAlongPath)
    {
      _position = _path.StandardizeUnit(distanceAlongPath, _positionUnits);  
      transform.position = _path.EvaluatePositionAtUnit(_position, _positionUnits);
      // transform.rotation = _path.EvaluateOrientationAtUnit(_position, _positionUnits);
    }
  }
}