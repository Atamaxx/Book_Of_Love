using System;
using UnityEngine;

namespace MainController
{
    public class CharacterController2D : MonoBehaviour, IPlayerController
    {
        public static CharacterController2D PlayerInstance { get; private set; }
        public Vector3 Velocity { get; private set; }
        public FrameInput Input { get; private set; }
        public bool JumpingThisFrame { get; private set; }
        public bool LandingThisFrame { get; private set; }
        public Vector2 RawMovement { get; private set; }
        public bool Grounded => _colDown;

        //public Vector2 InputAnim { get; private set; }
        //public Vector2 Speed { get; }
        public bool Crouching { get; }

        public event Action<bool, float> GroundedChanged; // Grounded - Impact force
        public event Action Jumped;
        public event Action Attacked;
        public event Action Sprinting;


        private Vector3 _lastPosition;
        public float _currentHorizontalSpeed, _currentVerticalSpeed;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            Invoke(nameof(Activate), 0.5f);
        }

        private bool _active;
        void Activate() => _active = true;

        private void Update()
        {

            if (!_active) return;

            // Calculate velocity
            Velocity = (transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;

            GatherInput();
            RunCollisionChecks();

            CalculateRun(); // Horizontal movement
            CalculateJumpApex(); // Affects fall speed, so calculate before gravity
            CalculateGravity(); // Vertical movement
            CalculateJump(); // Possibly overrides vertical

            MoveCharacter(); // Perform the axis movement
            //SetAnimationValues();
        }


        #region Gather Input

        private void GatherInput()
        {
            Input = new FrameInput
            {
                JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
                JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
                X = UnityEngine.Input.GetAxisRaw("Horizontal"),
                Sprint = UnityEngine.Input.GetButton("Sprint")
            };
            if (Input.JumpDown)
            {
                _lastJumpPressed = Time.time;
            }
        }

        #endregion

        #region Run

        [Header("RUNNING")]

        [SerializeField] private float _acceleration = 90;
        [SerializeField] private float _moveClamp = 13;
        [SerializeField] private float _deAcceleration = 60f;
        [SerializeField] private float _apexBonus = 2;
        [SerializeField] private float _sprintMultiplier = 2;

        [SerializeField] private bool _noAcceleration = true;

        private void CalculateRun()
        {
            if (Input.X != 0)
            {
                if (Input.Sprint)
                {
                    Sprinting?.Invoke();
                    // Set horizontal sprint speed
                    _currentHorizontalSpeed += _sprintMultiplier * Input.X * _acceleration * Time.deltaTime;

                    // clamped by max frame movement
                    _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp * _sprintMultiplier, _moveClamp * _sprintMultiplier);
                }
                else
                {
                    // Set horizontal move speed
                    _currentHorizontalSpeed += Input.X * _acceleration * Time.deltaTime;

                    // clamped by max frame movement
                    _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);
                }

                // Apply bonus at the apex of a jump
                var apexBonus = Mathf.Sign(Input.X) * _apexBonus * _apexPoint;
                _currentHorizontalSpeed += apexBonus * Time.deltaTime;
            }
            else if (_noAcceleration)
            {
                _currentHorizontalSpeed = 0;
            }
            else
            {
                // No input. Let's slow the character down
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
            }

        }

        #endregion

        #region Jump

        [Header("JUMPING")]
        [SerializeField] private float _jumpHeight = 30;
        [SerializeField] private int _numberOfJumps = 2;
        [SerializeField] private float _jumpApexThreshold = 10f;
        [SerializeField] private float _coyoteTimeThreshold = 0.1f;
        [SerializeField] private float _jumpBuffer = 0.1f;
        [SerializeField] private float _jumpEndEarlyGravityModifier = 3;
        private bool _coyoteUsable;
        [SerializeField] private bool _endedJumpEarly = true;
        private float _apexPoint; // Becomes 1 at the apex of a jump
        private float _lastJumpPressed;
        [SerializeField] private int _currentJump;
        private bool CanUseCoyote => _coyoteUsable && !_colDown && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
        private bool HasBufferedJump => _colDown && _lastJumpPressed + _jumpBuffer > Time.time;

        private void CalculateJumpApex()
        {
            if (!_colDown)
            {
                // Gets stronger the closer to the top of the jump
                _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(Velocity.y));
                _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
            }
            else
            {
                _apexPoint = 0;
            }
        }

        private void CalculateJump()
        {
            // Jump if: grounded or within coyote threshold || sufficient jump buffer
            if (HasBufferedJump)
            {
                _currentVerticalSpeed = _jumpHeight;
                _endedJumpEarly = false;
                //_coyoteUsable = false;
                _timeLeftGrounded = float.MinValue;
                JumpingThisFrame = true;
                _currentJump = 1;
            }
            else if (Input.JumpDown && CanUseCoyote)
            {
                _currentVerticalSpeed = _jumpHeight;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                //_timeLeftGrounded = float.MinValue;
                JumpingThisFrame = true;
                _currentJump = 1;
            }
            else if (Input.JumpDown && _currentJump < _numberOfJumps)
            {
                _currentVerticalSpeed = _jumpHeight;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                JumpingThisFrame = true;
                _currentJump++;
            }            
            else if (_colDown)
            {
                _currentJump = 0;
                JumpingThisFrame = false;
            }
            else
            {
                JumpingThisFrame = false;
            }

            // End the jump early if button released
            if (!_colDown && Input.JumpUp && !_endedJumpEarly && Velocity.y > 0)
            {
                // _currentVerticalSpeed = 0;
                _endedJumpEarly = true;
            }

            if (_colUp)
            {
                if (_currentVerticalSpeed > 0)
                {
                    _currentVerticalSpeed = 0;
                }
            }
        }

        #endregion


        #region Gravity

        [Header("GRAVITY")][SerializeField] private float _fallClamp = -40f;
        [SerializeField] private float _minFallSpeed = 80f;
        [SerializeField] private float _maxFallSpeed = 120f;
        private float _fallSpeed;

        private void CalculateGravity()
        {
            if (_colDown)
            {
                // Move out of the ground
                if (_currentVerticalSpeed < 0)
                {
                    _currentVerticalSpeed = 0;
                }
            }
            else
            {
                // Add downward force while ascending if we ended the jump early
                var fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;

                // Fall
                _currentVerticalSpeed -= fallSpeed * Time.deltaTime;

                // Clamp
                if (_currentVerticalSpeed < _fallClamp) _currentVerticalSpeed = _fallClamp;
            }
        }

        #endregion


        #region Collisions

        [Header("COLLISION")]

        //[SerializeField] private Bounds _characterBounds;

        [SerializeField] private LayerMask _groundLayer;

        [SerializeField] private Transform _groundCheck;
        [SerializeField] float _groundedRadius = .4f;

        [SerializeField] private Transform _ceilingCheck;



        [SerializeField][Range(0.1f, 0.3f)] private float _rayBuffer = 0.1f;




        private float _timeLeftGrounded;
        private bool _colDown, _colUp;


        private void RunCollisionChecks()
        {
            // Ground
            LandingThisFrame = false;
            var groundedCheck = Physics2D.OverlapCircle(_groundCheck.position, _groundedRadius, _groundLayer);
            float _impactForce;

            if (_colDown && !groundedCheck)
            {
                _timeLeftGrounded = Time.time; // Only trigger when first leaving
                _impactForce = 0;
                GroundedChanged?.Invoke(groundedCheck, _impactForce);

            }
            else if (!_colDown && groundedCheck)
            {
                _coyoteUsable = true; // Only trigger when first touching
                LandingThisFrame = true;

                _impactForce = _currentVerticalSpeed;
                GroundedChanged?.Invoke(groundedCheck, _impactForce);
            }

            _colDown = groundedCheck;

            // Ceiling
            RaycastHit2D hitUp = Physics2D.Raycast(_ceilingCheck.position, Vector2.up, _rayBuffer, _groundLayer);
            _colUp = hitUp;
        }

        #endregion


        #region Move

        private void MoveCharacter()
        {
            RawMovement = new Vector2(_currentHorizontalSpeed, _currentVerticalSpeed); // Used externally

            _rigidbody2D.velocity = RawMovement;
        }

        #endregion


 
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_groundCheck.position, _groundedRadius);

            Debug.DrawRay(_ceilingCheck.position, _rayBuffer * Vector2.up, Color.blue);
        }
    }
}