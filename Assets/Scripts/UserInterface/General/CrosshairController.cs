using UnityEngine;
using UnityEngine.UI;
using UserInterface.Panel;

namespace UserInterface.General
{
  public class CrosshairController : MonoBehaviour
  {
    [Header("Player Screen Panel")]
    [SerializeField]
    private PlayerScreenPanel _playerScreenPanel;
    
    [Header("Values")]
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

    [Header("For Scope")]
    [SerializeField]
    private GameObject _crossHair;

    private void Awake()
    {
      _playerScreenPanel.Player.OnFire += ExpandCrossHair;
      _playerScreenPanel.Player.OnHit += OnHit;
      _playerScreenPanel.Player.OnScopeOpened += OnScopeOpen;
      
      _hitCrosshair.enabled = false;
    }

    private void Update()
    {
      CheckMovement();
      ShrinkingCrossHair();
      CheckingHitIndicator();
      
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
      if (_playerScreenPanel.Player.GetFirstPersonController().GetIsSliding() || !_playerScreenPanel.Player.GetFirstPersonController().GetIsGrounded())
      {
        SetMargin(_otherMovements);
      }
      else if (_playerScreenPanel.Player.GetFirstPersonController().GetIsRunning())
      {
        SetMargin(_runningMargin);
      }
      else if (_playerScreenPanel.Player.GetFirstPersonController().GetIsMoving())
      {
        SetMargin(_normalMargin);
      }
      else if (_playerScreenPanel.Player.GetFirstPersonController().GetIsCrouching())
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
    
    [Header("Margins")]
    [SerializeField]
    private float _crouchingMargin = 20f;
    [SerializeField]
    private float _normalMargin = 25f;
    [SerializeField]
    private float _runningMargin = 100f;
    [SerializeField]
    private float _otherMovements = 125f;
    
    public void SetMargin(float margin)
    {
      _margin = margin;
    }

    [Header("Hit")]
    [SerializeField]
    private Image _hitCrosshair;

    private const float _hitIndicatorTime = 0.1f;

    private float _currentVisibleTime;

    private bool _isVisible;
    

    private void CheckingHitIndicator()
    {
      if (!_isVisible) return;
      
      _currentVisibleTime -= Time.deltaTime;
      if (_currentVisibleTime <= 0)
      {
        HideIndicator();
      }
    }
    
    public void ShowHitIndicator()
    {
      _currentVisibleTime = _hitIndicatorTime;

      if (_isVisible) return;
      _isVisible = true;
      _hitCrosshair.enabled = true;
    }

    private void HideIndicator()
    {
      _isVisible = false;
      _hitCrosshair.enabled = false;
    }

    private const float _randomness = 4f;
    private void OnHit(bool headshot)
    {
      _hitCrosshair.color = headshot ? Color.red : Color.white;
      _hitCrosshair.rectTransform.localScale = headshot ? new Vector3(1.3f, 1.3f, 1) : new Vector3(1, 1, 1);
      _hitCrosshair.rectTransform.localPosition = new Vector3(Random.Range(-_randomness, _randomness), Random.Range(-_randomness, _randomness), 0);
      ShowHitIndicator();
    }

    #region Scope

    public void OnScopeOpen(bool isScoped)
    {
      _crossHair.SetActive(!isScoped);
    }

    #endregion
  }
}