using Actor;
using DayCycle;
using DG.Tweening;
using Systems.EndGame;
using UnityEngine;

namespace HelicopterAction
{
  public class Helicopter : MonoBehaviour
  {
    private Vector3 _targetPosition;
    
    public float moveDuration = 20f;
    private const float _tiltAngle = -15f;
    public float rotationDuration = 0.5f;

    public Transform Rotor;
    public float RotorSpeed = 360f;

    public GameObject Disabled;
    
    private AudioSource _audioSource;
    
    [SerializeField]
    private AudioClip _helicopterSound;
    
    private BoxCollider _boxCollider;

    [SerializeField]
    private GameObject _lights;

    private void Awake()
    {
      _audioSource = GetComponent<AudioSource>();
      _boxCollider = GetComponent<BoxCollider>();
      
      Disabled.SetActive(false);
      
      _targetPosition = new Vector3(196, 95, 111);
      
      _audioSource.clip = _helicopterSound;
      _audioSource.Play();
      
      _lights.SetActive(false);
    }

    private void Start()
    {
      FlyToTargetPosition();
      AnimateRotor();
    }

    private bool _isHelicopterReachedToPoint;
    private void FlyToTargetPosition()
    {
      Sequence helicopterSequence = DOTween.Sequence();

      _audioSource.pitch = 1.25f;
      helicopterSequence.Append(transform.DORotate(new Vector3(0, 180, _tiltAngle), rotationDuration).SetEase(Ease.OutSine));
      helicopterSequence.Append(transform.DOMove(_targetPosition, moveDuration).SetEase(Ease.InOutQuad));
      helicopterSequence.Append(transform.DORotate(new Vector3(0, 180, 0), rotationDuration).SetEase(Ease.InSine)).OnComplete(
        () =>
        {
          AdjustPitchOverTime();
          EndGameSystem.Instance.HelicopterReached();
          _isHelicopterReachedToPoint = true;

          if (_isLandingRequested)
          {
            Land();
          }
        });

      helicopterSequence.Play();
    }
    
    private void AnimateRotor()
    {
      Rotor.DORotate(new Vector3(0, 360, 0), 1f / RotorSpeed, RotateMode.LocalAxisAdd)
        .SetEase(Ease.Linear).SetLoops(-1);
    }

    private readonly Vector3 _landPosition = new(196, 45.8f, 111);

    private const float _landDuration = 7f;

    private bool _isLanded;
    
    private bool _isLandingRequested;
    
    public void Land()
    {
      _isLandingRequested = true;
      if (!_isHelicopterReachedToPoint) return;
      
      _targetPosition = _landPosition;
      
      Sequence helicopterSequence = DOTween.Sequence();

      helicopterSequence.Append(transform.DOMove(_targetPosition, _landDuration).SetEase(Ease.InOutQuad));

      helicopterSequence.OnComplete(Landed);
    }

    private void Landed()
    {
      _isLanded = true;
      _boxCollider.size = new Vector3(4, 1, 3);
      EndGameSystem.Instance.HelicopterLanded();

      DayCycleSystem.OnDayTimeChanged += OnDayTimeChange;
    }

    private void OnDayTimeChange(DayTime dayTime)
    {
      switch (dayTime)
      {
        case DayTime.Night:
        case DayTime.Evening:
          _lights.SetActive(true);
          break;
        case DayTime.Morning:
        case DayTime.Noon:
          _lights.SetActive(false);
          break;
      }
    }

    private void AdjustPitchOverTime()
    {
      DOTween.To(() => _audioSource.pitch, x => _audioSource.pitch = x, 1f, 5f)
        .SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (!other.CompareTag("Player")) return;
      
      if (_isLanded)
      {
        EndGameSystem.Instance.EndGame();
      }
      else
      {
        other.gameObject.GetComponent<Player>().GetPlayerHealth().TakeDamage(100);
      }
    }
  }
}