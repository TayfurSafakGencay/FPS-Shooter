using System;
using DG.Tweening;
using UnityEngine;

namespace DayCycle
{
  public class DayCycleSystem : MonoBehaviour
  {
    [SerializeField]
    private Transform _directionalLight;

    public static Action<DayTime> OnDayTimeChanged;
    
    private static DayTime _currentDayTime;
    public static DayTime CurrentDayTime
    {
      get => _currentDayTime;
      private set
      {
        _currentDayTime = value;
        OnDayTimeChanged?.Invoke(_currentDayTime);
      }
    }

    public const float DayTimeChangingTime = 20f;

    private void Start()
    {
      ChangeDayTime(DayTime.Morning);

      StartDayCycle();
    }
    
    private void StartDayCycle()
    {
      Sequence dayCycleSequence = DOTween.Sequence();
      
      dayCycleSequence.Append(_directionalLight.DORotate(new Vector3(90, 130, 90),
        DayTimeChangingTime, RotateMode.FastBeyond360).OnComplete( () => ChangeDayTime(DayTime.Noon)));
      
      dayCycleSequence.Append(_directionalLight.DORotate(new Vector3(180, 130, 90),
        DayTimeChangingTime, RotateMode.FastBeyond360).OnComplete( () => ChangeDayTime(DayTime.Evening)));
         
      dayCycleSequence.Append(_directionalLight.DORotate(new Vector3(270, 130, 90),
        DayTimeChangingTime, RotateMode.FastBeyond360).OnComplete( () => ChangeDayTime(DayTime.Night)));
      
      dayCycleSequence.Append(_directionalLight.DORotate(new Vector3(360, 130, 90),
        DayTimeChangingTime, RotateMode.FastBeyond360).OnComplete(() =>
        {
          ChangeDayTime(DayTime.Morning);
          StartDayCycle();
        }));
    }

    private void ChangeDayTime(DayTime dayTime)
    {
      CurrentDayTime = dayTime;
    }
  }

  public enum DayTime
  {
    Morning,
    Noon,
    Evening,
    Night
  }
}