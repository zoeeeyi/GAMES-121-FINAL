using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour, interface_Skills
{
    #region Movement Variables
    [Header("Movement")]
    //Horizontal Speed
	[SerializeField] private float m_horizontalSpeed = 400;
	[Range(0, 1)] [SerializeField] private float m_crouchSpeedMult = .36f;
    [Range(0, 2)][SerializeField] private float m_airSpeedMult = 1;
    private bool m_disableHorizontalControl = false;
    //Jump
    [SerializeField] private float m_jumpForce = 400f;
    [SerializeField] private ForceMode2D m_jumpMode = ForceMode2D.Impulse;
    [SerializeField] private float m_jumpCoyoteTime = 0.2f;
    private float m_jumpCoyoteTimer = 0;
    //Wall Jump
    [SerializeField] private Vector2 m_wallJumpForce = new Vector2(1, 1);
    [SerializeField] private ForceMode2D m_wallJumpMode = ForceMode2D.Impulse;
    [SerializeField] private float m_wallJumpCoyoteTime = 0.2f;
    [SerializeField] private float m_wallStickTime = 0.5f;
    private float m_wallJumpCoyoteTimer = 0;
    private float m_wallStickTimer = 0;
    //Others
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothingTime = .05f;   // How much to smooth out the movement
    [SerializeField] float m_normalGrav;
    [SerializeField] float m_fallingGrav;
    [SerializeField] float m_wallGrav;
    private bool m_disableGrav = false;
    private Vector3 m_movementSmoothV = Vector3.zero;
    private float m_bulletTimeScaleMult = 1;
    MovementRecorder m_wallMovementRecorder;
    #endregion

    #region Collision Variables
    [Header("Collision Check")]
    //Ground
	[SerializeField] private LayerMask m_groundLayerMask;
	[SerializeField] private Transform m_groundCheckPos;
    [SerializeField] float m_groundCheckRadius = .2f;
    //Wall
    [SerializeField] private LayerMask m_wallLayerMask;
    [SerializeField] private Transform m_wallCheckPos;
    int m_wallOutDirection;
    [SerializeField] float m_wallCheckRadius = .2f;
    //Ceiling
    [SerializeField] private Transform m_ceilingCheckPos;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_crouchDisableCollider;				// A collider that will be disabled when crouching
    [SerializeField] float m_ceilingCheckRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    #endregion

    #region Components Variables
    [Header("Components")]
    private Rigidbody2D m_rb;
    #endregion

    #region State Variables
    [Header("States")]
    private static bool state_grounded;
    private static bool state_onWall;
    private static bool state_crouching;
	private static bool state_jumping = false;
    private static bool state_wallJumping = false;
    private static int m_facingRight = 1;  // For determining which way the player is currently facing.
    #endregion

    private void Awake()
	{
		m_rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
        #region Flip Player
        if (Mathf.Abs(m_rb.velocity.x) > 0.1f && Mathf.Sign(m_rb.velocity.x) != m_facingRight)
        {
            Flip();
        }
        #endregion

        #region Ground Check
        bool _lastGroundedState = state_grounded;
        state_grounded = false;
		Collider2D[] _groundColliders = Physics2D.OverlapCircleAll(m_groundCheckPos.position, m_groundCheckRadius, m_groundLayerMask);
        if (_groundColliders.Length != 0)
        {
            //if (!_lastGroundedState) PlayerAnimation.instance.Ground();
            state_grounded = true;
            if (m_rb.velocity.y <= 0) EndJump();
        }

        //Ground Jump coyote time
        if (_lastGroundedState && !state_grounded && !state_jumping) m_jumpCoyoteTimer = m_jumpCoyoteTime;
        if (m_jumpCoyoteTimer > 0) m_jumpCoyoteTimer -= Time.fixedDeltaTime;
        else m_jumpCoyoteTimer = 0;
        #endregion

        #region Wall Check
        bool _lastOnWallState = state_onWall;
        state_onWall = false;
        Collider2D[] _wallColliders = Physics2D.OverlapCircleAll(m_wallCheckPos.position, m_wallCheckRadius, m_wallLayerMask);
        if (_wallColliders.Length != 0)
        {
            //if (!_lastOnWallState) PlayerAnimation.instance.Ground();
            state_onWall = true;
            m_wallOutDirection = - m_facingRight;
            if ((m_rb.velocity.x == 0 || Mathf.Sign(m_rb.velocity.x) != m_wallOutDirection) && state_wallJumping) EndJump();
            RecordMovementData(ref m_wallMovementRecorder);
        }

        //wall jump coyote time
        if (_lastOnWallState && !state_onWall && !state_wallJumping)
        {
            m_wallJumpCoyoteTimer = m_wallJumpCoyoteTime;
        }
        if (m_wallJumpCoyoteTimer > 0) m_wallJumpCoyoteTimer -= Time.fixedDeltaTime;
        else m_wallJumpCoyoteTimer = 0;

        //Wall stick time
        if (!state_onWall) m_wallStickTimer = 0;
        if (!_lastOnWallState && state_onWall) m_wallStickTimer = m_wallStickTime;
        if (m_wallStickTimer > 0 && state_onWall) m_wallStickTimer -= Time.fixedDeltaTime;
        else m_wallStickTimer = 0;
        #endregion

        #region Set animation based on check results
        PlayerAnimation.instance.Ground(state_grounded);
        #endregion
    }

    #region Movement Recorder
    struct MovementRecorder
    {
        public Vector2 st_position { get; private set; }
        public Vector2 st_velocity { get; private set; }
        public float st_gravScale { get; private set; }

/*        public MovementRecorder(Vector2 _pos, Vector2 _v, float _grav)
        {
            st_position = _pos;
            st_velocity = _v;
            st_gravScale = _grav;
        }
*/
        public void UpdateData(Vector2 _pos, Vector2 _v, float _grav)
        {
            st_position = _pos;
            st_velocity = _v;
            st_gravScale = _grav;
        }
    }

    void RecordMovementData(ref MovementRecorder _mr)
    {
        _mr.UpdateData(transform.position, m_rb.velocity, m_rb.gravityScale);
    }

    void ExtractMovementData(MovementRecorder _mr)
    {
        transform.position = _mr.st_position;
        m_rb.velocity = _mr.st_velocity;
        m_rb.gravityScale = _mr.st_gravScale;
    }
    #endregion

    #region Utility Methods
    private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_facingRight = (int) Mathf.Sign(m_rb.velocity.x);

        // Change by rotation
        Vector3 _newRotation = transform.localRotation.eulerAngles;
        _newRotation.y = (m_facingRight == 1) ? 0 : 180;
        transform.localRotation = Quaternion.Euler(_newRotation);

/*        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x = m_facingRight;
        transform.localScale = theScale;
*/    }

    public void ChangeGravityScale(float _scale)
    {
        m_rb.gravityScale = _scale;
    }

    public void DisableGravity(bool _disable)
    {
        //Use when we want to disable gravity but remain player control
        m_disableGrav = _disable;
    }

    public void DisableHorizontalControl(bool _disable)
    {
        m_disableHorizontalControl = _disable;
    }

    public void SetBulletTimeScaleMult()
    {
        m_bulletTimeScaleMult = 1 / Time.timeScale;
    }

    private void OnDrawGizmosSelected()
    {
        if (m_groundCheckPos != null) Gizmos.DrawWireSphere(m_groundCheckPos.position, m_groundCheckRadius);
        if (m_wallCheckPos != null) Gizmos.DrawWireSphere(m_wallCheckPos.position, m_wallCheckRadius);
    }

    #endregion

    #region Basic Movement Methods

    public void ExecuteBasicMove(float _move)
	{
        //Determine Gravity Scale
        if (!m_disableGrav)
        {
            if (m_wallStickTimer > 0 && m_rb.velocity.y < 0) m_rb.gravityScale = m_wallGrav;
            else m_rb.gravityScale = (Mathf.Approximately(m_rb.velocity.y, 0) || m_rb.velocity.y < 0) ? m_fallingGrav : m_normalGrav;
        } else
        {
            m_rb.gravityScale = 0;
        }

        //Apply speed multiplier
        if (state_grounded)
		{
			if (state_crouching) _move *= m_crouchSpeedMult;
		} else
		{
			_move *= m_airSpeedMult;
		}

        // Move the character by finding the target velocity
        float _targetHorizontalV = (m_disableHorizontalControl) ? m_rb.velocity.x : _move * m_horizontalSpeed * Time.deltaTime;
        Vector3 targetVelocity = new Vector2(_targetHorizontalV, m_rb.velocity.y);
		if (m_MovementSmoothingTime > 0) m_rb.velocity = m_bulletTimeScaleMult * Vector3.SmoothDamp(m_rb.velocity, targetVelocity, ref m_movementSmoothV, m_MovementSmoothingTime);
		else m_rb.velocity = targetVelocity * m_bulletTimeScaleMult;
    }

    public void ExecuteJump()
	{
		if (state_grounded || (m_jumpCoyoteTimer > 0 && !state_onWall))
		{
			state_grounded = false;
			state_jumping = true;
            m_rb.AddForce(new Vector2(0f, m_jumpForce), m_jumpMode);
        }
        else if (state_onWall || m_wallJumpCoyoteTimer > 0)
        {
            state_onWall = false;
            state_wallJumping = true;
            DisableHorizontalControl(true);
            Vector2 _wallJumpForce = new Vector2(m_wallJumpForce.x * m_wallOutDirection, m_wallJumpForce.y);
            ExtractMovementData(m_wallMovementRecorder);
            m_rb.AddForce(_wallJumpForce, m_wallJumpMode);
        }

        //Reset coyote times
        m_jumpCoyoteTimer = 0;
        m_wallJumpCoyoteTimer = 0;
    }

	public void EndJump()
	{
        if (state_jumping)
		{
			state_jumping = false;
            m_rb.velocity = Vector3.zero;
        }

        if (state_wallJumping)
        {
            state_wallJumping = false;
            DisableHorizontalControl(false);
            m_rb.velocity = Vector3.zero;
        }
    }

	public void CrouchCheck()
	{
		//if the player is crouching but there's no ceiling detected, we finish crouching
		if (state_crouching)
		{
			if (!Physics2D.OverlapCircle(m_ceilingCheckPos.position, m_ceilingCheckRadius, m_groundLayerMask)) ExecuteCrouch(false);
		}
	}

	public void ExecuteCrouch(bool _crouch)
	{
		state_crouching = _crouch;
        // Enable/Disable the collider when not crouching
        if (m_crouchDisableCollider != null) m_crouchDisableCollider.enabled = _crouch;
    }
    #endregion

    #region Movement Skills
    //Double Jump by force
    public void DoubleJump(float _jumpForce, ForceMode2D _jumpMode)
    {
        EndJump();
        m_rb.AddForce(new Vector2(0f, _jumpForce), _jumpMode);
    }

    //Double jump by speed
    public void DoubleJump(float _jumpSpeed)
    {
        EndJump();
        m_rb.velocity = new Vector2(m_rb.velocity.x, _jumpSpeed);
    }

    //Dash by imposing force
    public void Dash(Vector3 _dashDir, float _dashForce, ForceMode2D _dashMode)
    {
        EndJump();
        m_rb.AddForce(_dashDir * _dashForce, _dashMode);
    }

    //Dash by setting speed
    public void Dash(Vector3 _dashDir, float _dashSpeed)
    {
        EndJump();
        m_rb.velocity = _dashDir * _dashSpeed;
    }

    public void Parry()
    {
        throw new System.NotImplementedException();
    }

    public void SlowDescend()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
