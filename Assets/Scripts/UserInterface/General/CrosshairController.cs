using UnityEngine;

namespace UserInterface.General
{
  public class CrosshairController : MonoBehaviour
  {
    [Range(0, 100)]
    [SerializeField] private float _expandValue;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _margin;
    
    [SerializeField]
    private float _expandSpeed;
    
    [SerializeField]
    private float _shrinkSpeed;
    
    [SerializeField]
    private RectTransform _top, _bottom, _left, _right, _center;

    private float _topValue, _bottomValue, _leftValue, _rightValue;
    
    private Player.Player _player;

    private void Awake()
    {
      _player = FindObjectOfType<Player.Player>();
      _player.OnFire += ExpandCrossHair;
    }

    private void Update()
    {
      CheckMovement();
      ShrinkingCrossHair();
      
      _topValue = Mathf.Lerp(_top.position.y, _center.position.y + _margin + _expandValue, _speed * Time.deltaTime);
      _bottomValue = Mathf.Lerp(_bottom.position.y, _center.position.y - _margin - _expandValue, _speed * Time.deltaTime);
      
      _leftValue = Mathf.Lerp(_left.position.x, _center.position.x - _margin - _expandValue, _speed * Time.deltaTime);
      _rightValue = Mathf.Lerp(_right.position.x, _center.position.x + _margin + _expandValue, _speed * Time.deltaTime);
      
      _top.position = new Vector2(_top.position.x, _topValue);
      _bottom.position = new Vector2(_bottom.position.x, _bottomValue);
      
      _left.position = new Vector2(_leftValue, _left.position.y);
      _right.position = new Vector2(_rightValue, _right.position.y);
    }

    private void CheckMovement()
    {
      if (_player.GetFirstPersonController().GetIsSliding() || !_player.GetFirstPersonController().GetIsGrounded())
      {
        SetMargin(_otherMovements);
      }
      else if (_player.GetFirstPersonController().GetIsRunning())
      {
        SetMargin(_runningMargin);
      }
      else if (_player.GetFirstPersonController().GetIsMoving())
      {
        SetMargin(_normalMargin);
      }
      else if (_player.GetFirstPersonController().GetIsCrouching())
      {
        SetMargin(_crouchingMargin);
      }
      else
      {
        SetMargin(_normalMargin);
      }
    }
    
    public void ExpandCrossHair(float addValue)
    {
      _expandValue += addValue * _expandSpeed;
      
      if (_expandValue >= 100)
      {
        _expandValue = 100;
      }
    }
    
    private void ShrinkingCrossHair()
    {
      if (_expandValue <= 0)
      {
        _expandValue = 0;
        return;
      }
      
      _expandValue -= _shrinkSpeed * Time.deltaTime;
    }

    [SerializeField]
    private float _crouchingMargin = 20f;
    [SerializeField]
    private float _normalMargin = 30f;
    [SerializeField]
    private float _runningMargin = 100f;
    [SerializeField]
    private float _otherMovements = 125f;
    
    public void SetMargin(float margin)
    {
      _margin = margin;
    }
  }
}