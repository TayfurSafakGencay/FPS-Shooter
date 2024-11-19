using Audio;
using Managers.Manager;
using UnityEngine;
using Utilities;

namespace Systems.Chase
{
  public class ChaseSystem : MonoBehaviour
  {
    private const float _chaseTime = 60f;

    private static bool _isChasing;
    
    public static async void ChaseHit()
    {
      if (_isChasing) return;
      
      _isChasing = true;
      SoundManager.Instance.PlaySoundEffect(SoundKey.ChaseHit);

      await Utility.Delay(_chaseTime);
      _isChasing = false;
    }
  }
}