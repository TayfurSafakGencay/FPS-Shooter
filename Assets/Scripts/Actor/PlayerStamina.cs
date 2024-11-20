using PostProcess;
using UnityEngine;

namespace Actor
{
  public class PlayerStamina : MonoBehaviour
  {
    private Player _player;

    private const float _maxStamina = 100f;
    private float _currentStamina;
    private const float _staminaDrainRate = 10f;
    private const float _staminaRegenRate = 5f;

    private const float _slowDownThreshold = 25;
    private const float _stopRunningThreshold = 5;
    
    public float GetStaminaPercentage() => _currentStamina / _maxStamina;

    private void Awake()
    {
      _player = GetComponent<Player>();
      
      _currentStamina = _maxStamina;
    }

    private void Update()
    {
      if (_player.GetFirstPersonController().GetIsRunning())
      {
        _currentStamina -= _staminaDrainRate * Time.deltaTime;
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);
      }
      else
      {
        _currentStamina += _staminaRegenRate * Time.deltaTime;
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);
      }

      switch (_currentStamina)
      {
        case <= _stopRunningThreshold:
          _player.GetFirstPersonController().SetCanSprint(false);
          break;
        case <= _slowDownThreshold:
        {
          float speed = Mathf.Lerp((
              _player.GetFirstPersonController().GetWalkSpeed() + _player.GetFirstPersonController().GetInitialSprintSpeed()) / 2,
            _player.GetFirstPersonController().GetInitialSprintSpeed(), (_currentStamina - 10f) / 20f);
        
          _player.GetFirstPersonController().SetSprintSpeed(speed);
          break;
        }
        default:
          _player.GetFirstPersonController().SetCanSprint(true);
          _player.GetFirstPersonController().SetSprintSpeed(_player.GetFirstPersonController().GetInitialSprintSpeed());
          break;
      }
      
      StaminaEffect.OnStaminaChanged(_currentStamina);
    }
  }
}