using System.Collections;
using UnityEngine;

namespace Player
{
    public class FirstPersonController : MonoBehaviour
    {
        public bool CanMove { get; private set; } = true;
        private bool _isSprinting => _canSprint && Input.GetKey(_sprintKey);
        private bool _shouldJump => _canJump && Input.GetKeyDown(_jumpKey) && _characterController.isGrounded;
        
        private bool _shouldCrouch => (Input.GetKeyDown(_crouchKey)||Input.GetKeyUp(_crouchKey)) && !_duringCrouchAnimation && _characterController.isGrounded; 

        [SerializeField] private Camera _camera;
        
        [Header("Functional Options")]
        [SerializeField] private bool _canSprint = true;
        [SerializeField] private bool _canJump = true;
        [SerializeField] private bool _canCrouch = true;
        [SerializeField] private bool _canHeadBob = true;
        [SerializeField] private bool _willSlideOnSlope = true;
        
        [Header("Controls")]
        [SerializeField] private KeyCode _sprintKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
        [SerializeField] private KeyCode _crouchKey = KeyCode.LeftControl;
        
        [Header("Movement Parameters")]
        [SerializeField] private float _walkSpeed = 3f;
        [SerializeField] private float _sprintSpeed = 6f;
        [SerializeField] private float _crouchSpeed = 1.5f;
        [SerializeField] private float _slopeSpeed = 8f;
        private float _currentSpeed => _isCrouching ? _crouchSpeed : _isSprinting ? _sprintSpeed : _walkSpeed;
        
        [Header("Look Parameters")]
        [SerializeField, Range(1, 10)] private float _lookSpeedX = 2f;
        [SerializeField, Range(1, 10)] private float _lookSpeedY = 2f;
        [SerializeField, Range(1, 180)] private float _upperLookLimit = 80f;
        [SerializeField, Range(1, 180)] private float _lowerLookLimit = 80f;
        
        [Header("Jump Parameters")]
        [SerializeField] private float _jumpForce = 8f;
        [SerializeField] private float _gravity = 30f;
        
        [Header("Crouch Parameters")]
        [SerializeField] private float _crouchHeight = 0.5f;
        [SerializeField] private float _standingHeight = 2f;
        [SerializeField] private float _timeToCrouch = 0.25f;
        [SerializeField] private Vector3 _crouchingCenter = new(0, 0.5f, 0);
        [SerializeField] private Vector3 _standingCenter = new(0, 0, 0);
        private bool _isCrouching;
        private bool _duringCrouchAnimation;
        
        [Header("Head Bob Parameters")]
        [SerializeField] private float _walkBobSpeed = 14f;
        [SerializeField] private float _walkBobAmount = 0.05f;
        [SerializeField] private float _runBobSpeed = 18f;
        [SerializeField] private float _runBobAmount = 0.01f;
        [SerializeField] private float _crouchBobSpeed = 8f;
        [SerializeField] private float _crouchBobAmount = 0.025f;
        private float _defaultYPos;
        private float _timer;
        private float _bobSpeed => _isCrouching ? _crouchBobSpeed : _isSprinting ? _runBobSpeed : _walkBobSpeed;
        private float _bobAmount => _isCrouching ? _crouchBobAmount : _isSprinting ? _runBobAmount : _walkBobAmount;
        
        [Header("Slide Parameters")]
        private Vector3 _hitPointNormal;

        private bool _isSliding
        {
            get
            {
                if (_characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
                {
                    _hitPointNormal = slopeHit.normal;
                    return Vector3.Angle(_hitPointNormal, Vector3.up) > _characterController.slopeLimit;
                }
                
                return false;
            }
        }
        
        private CharacterController _characterController;

        private Vector3 _moveDirection;
        private Vector2 _currentInput;
        
        private float _rotationX;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _defaultYPos = _camera.transform.localPosition.y;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        private void Update()
        {
            if (CanMove)
            {
                HandleMovement();
                HandleLook();
                
                if (_canJump)
                    HandleJump();

                if (_canCrouch)
                    HandleCrouch();

                if (_canHeadBob)
                    HandleHeadBob();
                
                ApplyFinalMovement();
            }
        }
        
        private void HandleMovement()
        {
            _currentInput = new Vector2(Input.GetAxis("Horizontal") * _currentSpeed,
                Input.GetAxis("Vertical") * _currentSpeed);
            
            float moveDirectionY = _moveDirection.y;
            _moveDirection = transform.TransformDirection(Vector3.forward) * _currentInput.y +
                             transform.TransformDirection(Vector3.right) * _currentInput.x;
            _moveDirection.y = moveDirectionY;
        }
        
        private void HandleLook()
        {
            _rotationX -= Input.GetAxis("Mouse Y") * _lookSpeedY;
            _rotationX = Mathf.Clamp(_rotationX, -_upperLookLimit, _lowerLookLimit);
            _camera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeedX, 0);
        }
        
        private void HandleJump()
        {
            if (_shouldJump)
                _moveDirection.y = _jumpForce;
        }
        
        private void HandleCrouch()
        {
            if (_shouldCrouch)
            {
                StartCoroutine(CrouchStand());
            }
        }

        private IEnumerator CrouchStand()
        {
            if (_isCrouching && Physics.Raycast(_camera.transform.position, Vector3.up, 1f))
                yield break;
            
            _duringCrouchAnimation = true;
            
            float timeElapsed = 0;
            float targetHeight = _isCrouching ? _standingHeight : _crouchHeight;
            float currentHeight = _characterController.height;
            Vector3 targetCenter = _isCrouching ? _standingCenter : _crouchingCenter;
            Vector3 currentCenter = _characterController.center;
            
            while(timeElapsed < _timeToCrouch)
            {
                _characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / _timeToCrouch);
                _characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / _timeToCrouch);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            
            _characterController.height = targetHeight;
            _characterController.center = targetCenter;
            
            _isCrouching = !_isCrouching;
            
            _duringCrouchAnimation = false;
        }

        private void HandleHeadBob()
        {
            if(!_characterController.isGrounded) return;

            if (Mathf.Abs(_moveDirection.x) > 0.1f || Mathf.Abs(_moveDirection.z) > 0.1f)
            {
                _timer += Time.deltaTime * _bobSpeed;
                _camera.transform.localPosition = new Vector3(
                    _camera.transform.localPosition.x,
                    _defaultYPos + Mathf.Sin(_timer) * _bobAmount,
                    _camera.transform.localPosition.z);
            }
        }

        private void ApplyFinalMovement()
        {
            if (!_characterController.isGrounded)
                _moveDirection.y -= _gravity * Time.deltaTime;

            if (_willSlideOnSlope && _isSliding)
            {
                _moveDirection += new Vector3(_hitPointNormal.x, -_hitPointNormal.y, _hitPointNormal.z) * _slopeSpeed;
            }
            
            _characterController.Move(_moveDirection * Time.deltaTime);
        }
    }
}
