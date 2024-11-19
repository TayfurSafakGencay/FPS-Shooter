using Base.Interface;
using UnityEngine;
using Utilities;

namespace Actor
{
  public class MakeSound : MonoBehaviour    
  {
    private SphereCollider _collider;

    private Player _player;
    
    private bool _soundMade;

    private bool _isReady;

    private void Awake()
    {
      _collider = gameObject.GetComponent<SphereCollider>();

      _isReady = false;
    }
    
    public void SetPlayer(Player player)
    {
      _player = player;
      _isReady = true;
    }

    private void Update()
    {
      if(!_isReady) return;

      if (_player.GetFirstPersonController().GetIsCrouching()) return;
      
      if (_player.GetFirstPersonController().GetIsJumping())
      {
        MakeSoundAtPosition(SoundDistances.Jump);
      }
      else if (_player.GetFirstPersonController().GetIsRunning())
      {
        MakeSoundAtPosition(SoundDistances.FootstepRun);
      }
      else if (_player.GetFirstPersonController().GetIsMoving())
      {
        MakeSoundAtPosition(SoundDistances.FootstepWalk);
      }
    }

    public async void MakeSoundAtPosition(float radius)
    { 
      if (_collider.radius >= radius) return;
      
      _soundMade = true;
      _collider.radius = radius;

      await Utility.Delay(0.4f);
      _soundMade = false;
      
      await Utility.Delay(0.1f);
      if (_soundMade) return;
      
      _collider.radius = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.TryGetComponent(out ISoundDetector soundDetector))
      {
        soundDetector.NoticeSound();
      }
    }
  }

  public record SoundDistances
  {
    public const float Fire = 10f;
    
    public const float FootstepWalk = 2f;
    
    public const float FootstepRun = 5f;
    
    public const float Jump = 7f;
  }
}