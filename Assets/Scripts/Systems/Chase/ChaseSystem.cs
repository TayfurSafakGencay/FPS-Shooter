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
    
    public static bool Screaming { get; set; }
    
    public static async void ChaseHit()
    {
      if (_isChasing) return;
      
      _isChasing = true;
      SoundManager.Instance.PlaySoundEffect(SoundKey.ChaseHit);

      await Utility.Delay(_chaseTime);
      _isChasing = false;
    }

    public static async void Screamed(float screamTime)
    {
      if (Screaming) return;
      Screaming = true;
      
      await Utility.Delay(screamTime);
      
      Screaming = false;
    }
  }
}