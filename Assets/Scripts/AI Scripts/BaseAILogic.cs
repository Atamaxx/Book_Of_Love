using UnityEngine;


    public class BaseAILogic : MonoBehaviour
    {

        #region All Conditions

        [Header("CONDITIONS")]
        [SerializeField] private bool _canPatrol;
        [SerializeField] private bool _canSearchForTarget;
        [SerializeField] private bool _canFly;
        [SerializeField] private bool _canWalk;
        [SerializeField] private bool _canJump;
        [SerializeField] private bool _canWallClimb;
        private void Start()
        {
            _lineOfSight = GetComponentInChildren<LineOfSight>();
            _lastSeenTargetPos = _target.position;
        }


        public bool ShouldMove()
        {
            if (!_canWalk) return false;

            bool shouldMove = false;

            if (_colTarget)
            {
                return false;
            }

            if (SearchingForTarget())
            {
                return true;
            }

            if (_canPatrol && ColDown)
            {
                shouldMove = true;
                if (!CanReach())
                {
                    Patrol();
                    Flip();
                }
            }
            else if (_canPatrol)
            {
                Patrol();
                shouldMove = true;
            }


            return shouldMove;
        }

        public bool ShouldJump(float moveX, float moveY)
        {
            if (!_canJump) return false;

            bool shouldJump = false;
            float jumpHeight = -(moveY / 2) / Physics2D.gravity.y;

            shouldJump = CanJumpOverWall();


            return shouldJump;
        }

        private bool CanReach()
        {
            if (CanJumpOverWall() || CanFall())
                return true;

            return false;
        }

        public void CollisionsCheck()
        {
            GapCollisions();
            SideCollisions();
        }

        #endregion
        #region Patrol
        [Header("Patrol")]
        [SerializeField] private Transform _start;
        [SerializeField] private Transform _end;
        bool goToStart = true;

        public void Patrol()
        {
            Vector2 goTo;

            if (goToStart)            
                goTo = _start.position;
            else          
                goTo = _end.position;          
            float distance = Vector2.Distance(transform.position, goTo);

            IfTargetRight(goTo);

            if (distance < 1f)// || !CanReach())
            {
                goToStart = !goToStart;
            }
        }

        #endregion


        #region Searching for Target

        [Header("Target Spotting")]
        [SerializeField] private Transform _target;
        [SerializeField] private Vector2 _targetPos;
        [SerializeField] private float _maxSearchDistance;
        [SerializeField] private bool _canSeeTarget;
        [SerializeField] private bool _agression;

        private LineOfSight _lineOfSight;
        Vector2 _lastSeenTargetPos;


        private bool SearchingForTarget()
        {
            //If enemy saw player
            if (!_lineOfSight.TargetDetected || !_canSearchForTarget)
                return false;

            float distance = Vector2.Distance(_ceilingCheck.position, _target.position);

            //When player is to far stop searching
            if (_maxSearchDistance < distance) return false;

            bool shouldFindTarget;
            bool canSeeTarget;

            shouldFindTarget = true;

            IfTargetRight(_lastSeenTargetPos);

            Vector2 direction = (_target.position - _ceilingCheck.position).normalized;

            // Make a raycast line from eyes of the enemy to the target
            RaycastHit2D hitTarget = Physics2D.Raycast(_ceilingCheck.position, direction, distance, _sideCheckLayer);

            // If enemy can directly see player
            if (hitTarget.collider != null && hitTarget.collider.CompareTag(_target.tag))
            {
                _canSeeTarget = true;
                // Save position in which enemy seen playes last time, so enemy can continue searching for player even if he does not see him directly
                _lastSeenTargetPos = _target.position;
            }
            else
            {
                _canSeeTarget = false;
            }

            return shouldFindTarget;
        }

        private void DisableAgression()
        {
            _agression = false;
        }

        private bool _canJumpX, _canJumpY, _canJumpXY;
        private float _jumpLengthX;
        private float _jumpLengthY;
        private float _jumpLengthXY;


        public void Flip()
        {
            // Switch the way the player is labelled as facing.
            FacingRight = !FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        private void IfTargetRight(Vector2 target)
        {
            bool isTargetRight;
            //Check if target is right from the enemy or not
            if (transform.position.x < target.x)
                isTargetRight = true;
            else
                isTargetRight = false;

            // When enemy is not looking at the player - flip him
            if (isTargetRight != FacingRight)
                Flip();
        }

        #endregion


        #region Collisions

        [Header("COLLISION")]

        [SerializeField] private float _fallDamageHeight = 4;
        [SerializeField] private float _maxWallJumpHeight = 4;

        [SerializeField] public LayerMask GroundLayer;
        [SerializeField] private LayerMask _sideCheckLayer;

        public Transform GroundCheck;
        [SerializeField] private Transform _sideCheck;
        [SerializeField] private Transform _gapCheck;
        [SerializeField] private Transform _ceilingCheck;
        public float GroundedRadius = .4f;

        [SerializeField][Range(0.1f, 5f)] private float _rayBuffer = 0.5f;
        [SerializeField][Range(0.1f, 5f)] private float _sideABuffer = 1.11f;
        [SerializeField][Range(0.1f, 5f)] private float _sideBBuffer = 1.11f;
        [SerializeField][Range(0.1f, 5f)] private float _gapRayBuffer = 0.1f;

        public bool FacingRight = false;




        private float _timeLeftGrounded;
        public bool ColDown;
        [field: SerializeField] public bool ColSide { get; private set; }
        public bool ColUp;

        [SerializeField] private bool _colWall, _colTarget, _colEnemy;
        [SerializeField] private bool _colGap, _colGapX, _colGapDown, _colGapUp;





        private void SideCollisions()
        {
            //RaycastHit2D hitSide;
            //hitSide = FacingRight ? Physics2D.Raycast(_gapCheck.position, Vector2.up, _sideRayBuffer, _sideCheckLayer) : Physics2D.Raycast(_sideCheck.position, Vector2.left, _sideRayBuffer, _sideCheckLayer);
            //hitSide = Physics2D.Raycast(_gapCheck.position, Vector2.up, _sideRayBuffer, _sideCheckLayer);
            Collider2D hitSide = Physics2D.OverlapBox(_sideCheck.position, new Vector2(_sideABuffer, _sideBBuffer), 0f, _sideCheckLayer);

            ColSide = hitSide;
            _colWall = false;
            _colTarget = false;
            _colEnemy = false;

            if (hitSide == null) return;

            if (hitSide.CompareTag("Platforms"))
                _colWall = true;
            else if (hitSide.CompareTag(_target.tag))
                _colTarget = true;
            else if (hitSide.CompareTag("Enemy"))
                _colEnemy = true;


            // Ceiling
            RaycastHit2D hitUp = Physics2D.Raycast(_ceilingCheck.position, Vector2.up, _rayBuffer, GroundLayer);
            ColUp = hitUp;
        }

        private void GapCollisions()
        {
            RaycastHit2D hitGap = Physics2D.Raycast(_gapCheck.position, Vector2.down, _gapRayBuffer, GroundLayer);

            if (hitGap || Physics2D.OverlapCircle(_gapCheck.position, GroundedRadius, GroundLayer))
            {
                _colGap = true;
            }
            else
                _colGap = false;
        }
        private bool CanJumpOverWall()
        {
            // If there is no wall ahead no need to jump over it
            if (!_colWall) return false;
            Vector2 hitOrigin = _gapCheck.position;
            float rayLength = _maxWallJumpHeight * 5;
            RaycastHit2D hitYUp = Physics2D.Raycast(hitOrigin, Vector2.up, rayLength, GroundLayer);

            // Now its time to calculate wall height so we can understand if enemy can jump over it
            if (hitYUp.collider == null) return false;

            Vector2 hitPoint = hitYUp.point;
            wallHeight = hitPoint.y - hitOrigin.y;

            //Debug.DrawRay(_gapCheck.position, _maxWallJumpHeight * Vector2.up, Color.magenta);
            //Debug.DrawRay(_gapCheck.position + Vector3.left, _maxWallJumpHeight * Vector2.up, Color.yellow);


            if (wallHeight > _maxWallJumpHeight) return false;
            return true;
        }

        private bool CanFall()
        {
            if (!_colGap) return false;

            Vector2 hitOrigin = _gapCheck.position;
            float rayLength = _maxWallJumpHeight;
            RaycastHit2D hitYDown = Physics2D.Raycast(hitOrigin, Vector2.down, rayLength, GroundLayer);

            if (hitYDown.collider != null) return false;


            return true;
        }
        [SerializeField] float wallHeight;
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(GroundCheck.position, GroundedRadius);
            Gizmos.DrawWireSphere(_gapCheck.position, GroundedRadius);
            Gizmos.DrawWireCube(_sideCheck.position, new Vector2(_sideABuffer, _sideBBuffer));

            //Debug.DrawRay(_celingCheck.position, _rayBuffer * Vector2.up, Color.red);
            //if (FacingRight)
            //{
            //    Debug.DrawRay(_sideCheck.position, _sideRayBuffer * Vector2.right, Color.yellow);
            //}
            //else if (!FacingRight)
            //{
            //    Debug.DrawRay(_sideCheck.position, _sideRayBuffer * Vector2.left, Color.yellow);
            //}
            //Debug.DrawRay(_gapCheck.position, _sideRayBuffer * Vector2.up, Color.yellow);
            Debug.DrawRay(_ceilingCheck.position, _rayBuffer * Vector2.up, Color.yellow);
            Debug.DrawRay(_gapCheck.position, _gapRayBuffer * Vector2.down, Color.magenta);
            //Debug.DrawLine(_ceilingCheck.position, _target.position, Color.red);
        }
    }

