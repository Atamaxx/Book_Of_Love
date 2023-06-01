using System;
using UnityEngine;


    public class BaseEnemyController : MonoBehaviour
    {

        [SerializeField] private BaseAILogic aiLogic;
        private bool _active;
        void Activate() => _active = true;


        

        private void Update()
        {
            //if (!_active) return;

            // Calculate velocity
            //Velocity = (transform.position - _lastPosition) / Time.deltaTime;
            //_lastPosition = transform.position;

            //RunCollisionChecks();
            //CalculateRun(); // Horizontal movement
            //CalculateGravity(); // Vertical movement
            //CalculateJump(); // Possibly overrides vertical
            //MoveCharacter(); // Perform the axis movement
        }





        public float Speed { get; private set; }
        public Vector3 Velocity { get; private set; }
        public bool JumpingThisFrame { get; private set; }
        public bool LandingThisFrame { get; private set; }
        public Vector2 RawMovement { get; private set; }
        public bool Grounded => aiLogic.ColDown;


        public event Action<bool, float> GroundedChanged; // Grounded - Impact force
        public event Action Jumped;
        public event Action Attacked;
        public event Action Sprinting;
        public event Action TargetSpotted;



        private Vector3 _lastPosition;
        public float _currentHorizontalSpeed, _currentVerticalSpeed;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            Invoke(nameof(Activate), 0.5f);
            //_playerCollider = GetComponent<Collider2D>();
        }



        #region Run

        [Header("RUNNING")]

        [SerializeField] private float _acceleration = 90;
        [SerializeField] private float _moveClamp = 13;

        [SerializeField] private float _deAcceleration = 60f;

        [SerializeField] private bool _noAcceleration = true;

        private LineOfSight _lineOfSight;
        private void CalculateRun()
        {
            if (aiLogic.ShouldMove())
            {
                if (aiLogic.FacingRight)
                {
                _currentHorizontalSpeed += _acceleration * UnityEngine.Time.deltaTime;
                }
                else
                {
                _currentHorizontalSpeed += -_acceleration * UnityEngine.Time.deltaTime;
                }

                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);
            }
            else if (_noAcceleration)
            {
                _currentHorizontalSpeed = 0;
            }
            else
            {
            // No input. Let's slow the character down
            _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * UnityEngine.Time.deltaTime);
            }

            Speed = _currentHorizontalSpeed;

            if (_currentHorizontalSpeed > 0 && !aiLogic.FacingRight)
            {
                aiLogic.Flip();
            }
            // Otherwise if moving left and is facing right...
            else if (_currentHorizontalSpeed < 0 && aiLogic.FacingRight)
            {
                aiLogic.Flip();
            }
        }


        

        #endregion



        #region Jump

        [Header("JUMPING")]
        [SerializeField] private float _jumpHeight = 30;
        //[SerializeField] private float _jumpBuffer = 0.1f;
        float _fallDamageHeight = 5;

        private void CalculateJump()
        {

            //float jumpHeight = (_currentVerticalSpeed / 2) / Physics2D.gravity.y;
            // Jump if: grounded or within coyote threshold || sufficient jump buffer
            if (aiLogic.ShouldJump(_currentHorizontalSpeed, _jumpHeight))
            {
                //jumpHeight = (_currentVerticalSpeed / 2) / Physics2D.gravity.y;
                _currentVerticalSpeed = _jumpHeight;
                JumpingThisFrame = true;
            }
            else
            {
                //jumpHeight = (_currentVerticalSpeed / 2) / Physics2D.gravity.y;
                JumpingThisFrame = false;
            }

            if (aiLogic.ColUp)
            {
                if (_currentVerticalSpeed > 0)
                {
                    _currentVerticalSpeed = 0;
                }
            }
        }

        private void RunCollisionChecks()
        {
            // Ground
            LandingThisFrame = false;
            bool groundedCheck = Physics2D.OverlapCircle(aiLogic.GroundCheck.position, aiLogic.GroundedRadius, aiLogic.GroundLayer);
            float _impactForce;

            if (aiLogic.ColDown && !groundedCheck)
            {
                _impactForce = 0;
                GroundedChanged?.Invoke(groundedCheck, _impactForce);

            }
            else if (!aiLogic.ColDown && groundedCheck)
            {
                LandingThisFrame = true;

                _impactForce = _currentVerticalSpeed;
                GroundedChanged?.Invoke(groundedCheck, _impactForce);
            }

            aiLogic.ColDown = groundedCheck;

            aiLogic.CollisionsCheck();
        }

        #endregion




        #region Gravity

        [Header("GRAVITY")][SerializeField] private float _fallClamp = -40f;
        [SerializeField] private float _minFallSpeed = 80f;
        //[SerializeField] private float _maxFallSpeed = 120f;
        private float _fallSpeed;

        private void CalculateGravity()
        {
            if (aiLogic.ColDown)
            {
                // Move out of the ground
                if (_currentVerticalSpeed < 0)
                {
                    _currentVerticalSpeed = 0;

                }
            }
            else
            {
                _fallSpeed = _minFallSpeed;
            // Fall
            _currentVerticalSpeed -= _fallSpeed * UnityEngine.Time.deltaTime;

                // Clamp
                if (_currentVerticalSpeed < _fallClamp) _currentVerticalSpeed = _fallClamp;
            }
        }

        #endregion


       


        #region Move

        private void MoveCharacter()
        {
            RawMovement = new Vector2(_currentHorizontalSpeed, _currentVerticalSpeed); // Used externally

            _rigidbody2D.velocity = RawMovement;
        }

        #endregion

        



        


    }
