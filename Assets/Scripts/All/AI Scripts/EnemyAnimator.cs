using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


namespace BaseEnemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private float _minImpactForce = 20;

        // Anim times can be gathered from the state itself, but 
        // for the simplicity of the video...
        [SerializeField] private float _landAnimDuration = 0.1f;
        [SerializeField] private float _attackAnimTime = 0.2f;

        private IEnemyController _enemy;
        private Animator _anim;
        private SpriteRenderer _renderer;

        private bool _grounded;
        private float _lockedTill;
        private bool _jumpTriggered;
        private bool _attacked;
        private bool _landed;
        private bool _isSprinting;

        private void Awake()
        {
            if (!TryGetComponent(out IEnemyController player))
            {
                Destroy(this);
                return;
            }

            _enemy = player;
            _anim = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
        }


        private void Start()
        {
            _enemy.Jumped += () =>
            {
                _jumpTriggered = true;
            };
            _enemy.Attacked += () =>
            {
                _attacked = true;
            };
            _enemy.GroundedChanged += (grounded, impactForce) =>
            {
                _grounded = grounded;
                _landed = impactForce >= _minImpactForce;
            };
            _enemy.Sprinting += () =>
            {
                _isSprinting = true;
            };
        }

        private void Update()
        {
            //if (_enemy.Speed > 0) _renderer.flipX = _enemy.Speed > 0;

            var state = GetState();

            _jumpTriggered = false;
            _landed = false;
            _attacked = false;
            _isSprinting = false;

            if (state == _currentState) return;
            _anim.CrossFade(state, 0, 0);
            _currentState = state;
        }

        private int GetState()
        {
            if (UnityEngine.Time.time < _lockedTill) return _currentState;

            // Priorities
            if (_attacked) return LockState(Attack, _attackAnimTime);
            if (_landed) return LockState(Land, _landAnimDuration);
            if (_jumpTriggered) return Jump;

            if (_grounded && _isSprinting) return _enemy.Speed == 0 ? Idle : Sprint;
            
            if (_grounded) return _enemy.Speed == 0 ? Idle : Run;
            return _enemy.RawMovement.y > 0 ? Jump : Fall;

            int LockState(int s, float t)
            {
                _lockedTill = UnityEngine.Time.time + t;
                return s;
            }
        }

        #region Cached Properties

        private int _currentState;

        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Sprint = Animator.StringToHash("Sprint");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Land = Animator.StringToHash("Land");
        private static readonly int Attack = Animator.StringToHash("Attack");

        #endregion
    }

}