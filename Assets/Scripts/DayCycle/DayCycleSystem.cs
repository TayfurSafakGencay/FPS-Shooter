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

    public const float DayTimeChangingTime = 120f;

    private void Start()
    {
      ChangeDayTime(DayTime.Morning);

      StartDayCycle();
    }

    public static void EndGame()
    {
      _dayCycleSequence.Kill();
    }
    
    private static Sequence _dayCycleSequence;
    
    private void StartDayCycle()
    {
      _dayCycleSequence = DOTween.Sequence();
      
      _dayCycleSequence.Append(_directionalLight.DORotate(new Vector3(90, 130, 90),
        DayTimeChangingTime, RotateMode.FastBeyond360).OnComplete( () => ChangeDayTime(DayTime.Noon)));
      
      _dayCycleSequence.Append(_directionalLight.DORotate(new Vector3(180, 130, 90),
        DayTimeChangingTime, RotateMode.FastBeyond360).OnComplete( () => ChangeDayTime(DayTime.Evening)));
         
      _dayCycleSequence.Append(_directionalLight.DORotate(new Vector3(270, 130, 90),
        DayTimeChangingTime, RotateMode.FastBeyond360).OnComplete( () => ChangeDayTime(DayTime.Night)));
      
      _dayCycleSequence.Append(_directionalLight.DORotate(new Vector3(360, 130, 90),
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