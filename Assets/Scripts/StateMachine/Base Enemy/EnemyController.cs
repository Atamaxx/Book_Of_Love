using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private Vector2 _targetPos;
    private LineOfSight _lineOfSight;
    private Rigidbody2D _rigidbody2D;
    private bool _facingRight;
    private float _currentHorizontalSpeed, _currentVerticalSpeed;

    #region Run
    [Header("RUNNING")]
    [SerializeField] private float _acceleration;
    [SerializeField] private float _moveClamp;
    [SerializeField] private float _deAcceleration = 60f;
    [SerializeField] private bool _noAcceleration = true;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _lineOfSight = GetComponentInChildren<LineOfSight>();
    }


    public float CalculateRun()
    {
        if (_facingRight)
        {
            _currentHorizontalSpeed += _acceleration * Time.deltaTime;
        }
        else
        {
            _currentHorizontalSpeed += -_acceleration * Time.deltaTime;
        }

        _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);

        return _currentHorizontalSpeed;
    }
    public void Stop()
    {
        if (_noAcceleration)
        {
            _currentHorizontalSpeed = 0;
        }
        else
        {
            _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
        }
    }
    #endregion

    #region Jump

    [Header("JUMPING")]
    [SerializeField] private PreciseJump _preciseJump;

    public void Jump()
    {
        _preciseJump.SetUpJump(_rigidbody2D, _groundCheck.position, JumpPoint);
        MoveCharacter(_preciseJump.CalculateVelocity());
    }
    #endregion

    #region Collisions

    [Header("COLLISIONS")]

    [SerializeField] float _groundedRadius = .4f;
    [SerializeField] private LayerMask _groundLayer, _sideCheckLayer;
    [SerializeField] private Transform _groundCheck, _gapCheck, _ceilingCheck, _sideCheck;

    [SerializeField][Range(0.1f, 15f)] private float _upRayBuffer, _gapRayBuffer, _sideABuffer, _sideBBuffer;

    [SerializeField] private bool _colDown, _colUp;
    [SerializeField] private bool _colWall, _colTarget, _colEnemy;
    [SerializeField] private bool _colGap, _colGapX, _colGapDown, _colGapUp;
    public void RunCollisionChecks()
    {
        // Ground
        bool groundedCheck = Physics2D.OverlapCircle(_groundCheck.position, _groundedRadius, _groundLayer);
        _colDown = groundedCheck;

        GapCollisions();
        SideCollisions();

        // Ceiling
        RaycastHit2D hitUp = Physics2D.Raycast(_ceilingCheck.position, Vector2.up, _upRayBuffer, _groundLayer);
        _colUp = hitUp;
    }

    private void GapCollisions()
    {
        RaycastHit2D hitGap = Physics2D.Raycast(_gapCheck.position, Vector2.down, _gapRayBuffer, _groundLayer);
        _colGapDown = hitGap;

        _colGap = Physics2D.OverlapCircle(_gapCheck.position, _groundedRadius, _groundLayer);
        //if (hitGap || Physics2D.OverlapCircle(_gapCheck.position, _groundedRadius, _groundLayer))
        //{
        //    _colGap = true;
        //}
        //else
        //    _colGap = false;
    }

    private void SideCollisions()
    {
        //RaycastHit2D hitSide;
        //hitSide = _facingRight ? Physics2D.Raycast(_gapCheck.position, Vector2.up, _sideRayBuffer, _sideCheckLayer) : Physics2D.Raycast(_sideCheck.position, Vector2.left, _sideRayBuffer, _sideCheckLayer);
        //hitSide = Physics2D.Raycast(_gapCheck.position, Vector2.up, _sideRayBuffer, _sideCheckLayer);
        Collider2D hitSide = Physics2D.OverlapBox(_sideCheck.position, new Vector2(_sideABuffer, _sideBBuffer), 0f, _sideCheckLayer);

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
    }
    #endregion


    #region Gravity

    [Header("GRAVITY")]
    [SerializeField] private float _fallClamp = -40f;
    [SerializeField] private float _fallSpeed = 80f;

    public void CalculateGravity()
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
            // Fall
            _currentVerticalSpeed -= _fallSpeed * Time.deltaTime;

            // Clamp
            if (_currentVerticalSpeed < _fallClamp) _currentVerticalSpeed = _fallClamp;
        }
    }

    #endregion

    #region 
    [Header("PATROL")]
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;
    [SerializeField] private bool goToStart;

    public void Patroling()
    {
        if (goToStart)
            _targetPos = _start.position;
        else
            _targetPos = _end.position;
        float distance = Vector2.Distance(transform.position, _targetPos);

        FlipWhenTargetRight(_targetPos);

        if (distance < 1f)// || !CanReach())
        {
            goToStart = !goToStart;
        }
    }
    public bool FollowingTarget(Animator animator)
    {
        _targetPos = _target.position;
        // float distance = Vector2.Distance(transform.position, _targetPos);
        FlipWhenTargetRight(_targetPos);

        if (_colTarget && _colDown)
        {
            animator.SetBool("targetReached", true);
            return false;
        }
        else
        {
            animator.SetBool("targetReached", false);
            return true;
        }
    }

    public void SearchingTarget(Animator animator)
    {
        _targetPos = _lastSeenTargetPos;
        float distance = Mathf.Abs(transform.position.x - _targetPos.x);// CHANGE
        FlipWhenTargetRight(_targetPos);
        if (distance < 1f)
        {
            animator.SetBool("isSearching", false);// CHANGE
        }

    }

    private void FlipWhenTargetRight(Vector2 target)
    {
        bool isTargetRight;
        //Check if target is right from the enemy or not
        if (transform.position.x < target.x)
            isTargetRight = true;
        else
            isTargetRight = false;

        // When enemy is not looking at the player - flip him
        if (isTargetRight != _facingRight)
            Flip();
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        _facingRight = !_facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    #endregion

    #region Move
    public void MoveCharacter(Vector2 velocity)
    {
        //RawMovement = new Vector2(_currentHorizontalSpeed, _currentVerticalSpeed);
        //_rigidbody2D.velocity = new Vector2(horizontalSpeed, verticalSpeed);
        _rigidbody2D.velocity = velocity;
    }

    public void MoveCharacterX(float horizontalSpeed)
    {
        _rigidbody2D.velocity = new Vector2(horizontalSpeed, _rigidbody2D.velocity.y);
    }

    public void MoveCharacterY(float verticalSpeed)
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, verticalSpeed);
    }
    #endregion

    #region Conditions

    [Header("Conditions")]
    [SerializeField] private bool _canSearchForTarget;
    [SerializeField] private float _maxSearchDistance;
    [SerializeField] private float _maxJumpHeight;
    [SerializeField] private float _maxJumpDistance;
    [SerializeField] private float _jumpOffsetX;
    [SerializeField] private float _jumpOffsetY;
    [SerializeField] private Vector2 _lastSeenTargetPos;

    public Vector2 JumpPoint;
    public bool ShouldJumpY()
    {
        // If there is no wall ahead no need to jump over it
        if (!_colWall || !_colDown) return false;
        float wallHeight;
        Vector2 hitOrigin = _gapCheck.position;
        float rayLength = _maxJumpHeight;
        RaycastHit2D hitUp = Physics2D.Raycast(hitOrigin, Vector2.up, rayLength, _groundLayer);

        // Now its time to calculate wall height so we can understand if enemy can jump over it
        if (hitUp.collider == null) return false;

        Vector2 hitPoint = hitUp.point;
        wallHeight = hitPoint.y - hitOrigin.y;

        Debug.DrawRay(_gapCheck.position, _maxJumpHeight * Vector2.up, Color.magenta);
        Debug.DrawRay(_gapCheck.position + Vector3.left, _maxJumpHeight * Vector2.up, Color.yellow);

        if (wallHeight > _maxJumpHeight) return false;

        JumpPoint = hitPoint;
        return true;
    }

    public bool ShouldJumpX()
    {
        if (_colWall || _colGap || _colGapDown || !_colDown) return false;
        Vector2 hitOriginPos = _sideCheck.position;
        Vector2 hitOrigin = new(hitOriginPos.x, hitOriginPos.y);
        float rayLength = _gapRayBuffer;
        RaycastHit2D hitGap;

        hitGap = Physics2D.Raycast(hitOrigin, Vector2.down, rayLength, _groundLayer);

        for (int i = 1; i < Mathf.RoundToInt(_maxJumpDistance); i++)
        {
            if (hitGap.collider != null) break;
            hitOrigin = _facingRight ? new(hitOriginPos.x + i, hitOriginPos.y) : new(hitOriginPos.x - i, hitOriginPos.y);
            hitGap = Physics2D.Raycast(hitOrigin, Vector2.down, rayLength, _groundLayer);

        }


        // Now its time to calculate wall height so we can understand if enemy can jump over it
        if (hitGap.collider == null) return false;

        Vector2 hitPoint = hitGap.point;
        //print("hitPoint" + hitPoint);
        //float gapLength = hitPoint.x - hitOrigin.x;

        //Debug.DrawRay(_gapCheck.position, _maxJumpHeight * Vector2.up, Color.magenta);
        //Debug.DrawRay(_gapCheck.position + Vector3.left, _maxJumpHeight * Vector2.up, Color.yellow);

        //if (wallHeight > _maxJumpHeight) return false;
        _jumpOffsetX = _facingRight ? _jumpOffsetX : -_jumpOffsetX;
        //_jumpOffsetY = _facingRight ? _jumpOffsetY : -_jumpOffsetY;

        hitPoint = new(hitPoint.x + _jumpOffsetX, hitPoint.y + _jumpOffsetY);
        JumpPoint = hitPoint;
        return true;
    }

    public bool SearchingForTarget(Animator animator)
    {
        //If enemy saw player
        if (!_lineOfSight.TargetDetected || !_canSearchForTarget)
            return false;
        float distance = Vector2.Distance(_ceilingCheck.position, _target.position);

        //When player is to far stop searching
        if (_maxSearchDistance < distance)
        {
            animator.SetBool("seeTarget", false);
            return false;
        }

        Vector2 direction = (_target.position - _ceilingCheck.position).normalized;

        // Make a raycast line from eyes of the enemy to the target
        RaycastHit2D hitTarget = Physics2D.Raycast(_ceilingCheck.position, direction, distance, _sideCheckLayer);

        // If enemy can directly see player
        if (hitTarget.collider != null && hitTarget.collider.CompareTag(_target.tag))
        {
            animator.SetBool("seeTarget", true);
            // Save position in which enemy seen playes last time, so enemy can continue searching for player even if he does not see him directly
            _lastSeenTargetPos = _target.position;
            return true;
        }
        else
        {
            animator.SetBool("seeTarget", false);
            return false;
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(JumpPoint, _groundedRadius * 10);
        Gizmos.DrawWireSphere(_groundCheck.position, _groundedRadius);
        Gizmos.DrawWireSphere(_gapCheck.position, _groundedRadius);
        Gizmos.DrawWireCube(_sideCheck.position, new Vector2(_sideABuffer, _sideBBuffer));
        Debug.DrawRay(_ceilingCheck.position, _upRayBuffer * Vector2.up, Color.yellow);
        Debug.DrawRay(_gapCheck.position, _gapRayBuffer * Vector2.down, Color.magenta);
    }
}
