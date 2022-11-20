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
    private bool m_disableVerticalControl = false;
    //Jump
    [SerializeField] private float m_jumpForce = 400f;
    [SerializeField] private ForceMode2D m_jumpMode = ForceMode2D.Impulse;
    //Wall Jump
    [SerializeField] private Vector2 m_wallJumpForce = new Vector2(1, 1);
    [SerializeField] private ForceMode2D m_wallJumpMode = ForceMode2D.Impulse;
    //Others
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothingTime = .05f;   // How much to smooth out the movement
    [SerializeField] float m_normalGrav;
    [SerializeField] float m_fallingGrav;
    [SerializeField] float m_wallGrav;
    private Vector3 m_movementSmoothV = Vector3.zero;
    #endregion

    #region Collision Variables
    [Header("Collision Check")]
    //Ground
	[SerializeField] private LayerMask m_groundLayerMask;
	[SerializeField] private Transform m_groundCheckPos;
    const float const_groundCheckRadius = .2f;
    //Wall
    [SerializeField] private LayerMask m_wallLayerMask;
    [SerializeField] private Transform m_wallCheckPosLeft;
    [SerializeField] private Transform m_wallCheckPosRight;
    int m_wallOutDirection;
    const float const_wallCheckRadius = .2f;
    //Ceiling
    [SerializeField] private Transform m_ceilingCheckPos;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_crouchDisableCollider;				// A collider that will be disabled when crouching
    const float const_ceilingCheckRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
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
        #region Ground Check
        state_grounded = false;
		Collider2D[] _groundColliders = Physics2D.OverlapCircleAll(m_groundCheckPos.position, const_groundCheckRadius, m_groundLayerMask);
        if (_groundColliders.Length != 0)
        {
            state_grounded = true;
            if (m_rb.velocity.y <= 0) EndJump();
        }
        #endregion

        #region Wall Check
        state_onWall = false;
        Collider2D[] _wallColliders = Physics2D.OverlapCircleAll(m_wallCheckPosRight.position, const_wallCheckRadius, m_wallLayerMask);
        if (_wallColliders.Length != 0)
        {
            state_onWall = true;
            m_wallOutDirection = - m_facingRight;
            if ((m_rb.velocity.x == 0 || Mathf.Sign(m_rb.velocity.x) != m_wallOutDirection) && state_wallJumping) EndJump();
        }
        #endregion
    }

    #region Utility Methods
    private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_facingRight = - m_facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    public void ChangeGravityScale(float _scale)
    {
        m_rb.gravityScale = _scale;
    }
    #endregion

    #region Basic Movement Methods

    public void ExecuteBasicMove(float _move)
	{
        //Determine Gravity Scale
        if (state_onWall) m_rb.gravityScale = m_wallGrav;
		else m_rb.gravityScale = (Mathf.Approximately(m_rb.velocity.y, 0) || m_rb.velocity.y < 0) ? m_fallingGrav : m_normalGrav;

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
		if (m_MovementSmoothingTime > 0) m_rb.velocity = Vector3.SmoothDamp(m_rb.velocity, targetVelocity, ref m_movementSmoothV, m_MovementSmoothingTime);
		else m_rb.velocity = targetVelocity;

        // If the input is moving the player right and the player is facing left...
        if (_move > 0 && m_facingRight == -1)
        {
            Flip();
        }
        else if (_move < 0 && m_facingRight == 1)
        {
            Flip();
        }
    }

    public void ExecuteJump()
	{
		if (state_grounded)
		{
			state_grounded = false;
			state_jumping = true;
            m_rb.AddForce(new Vector2(0f, m_jumpForce), m_jumpMode);
        } 
        else if (state_onWall)
        {
            state_onWall = false;
            state_wallJumping = true;
            m_disableHorizontalControl = true;
            Vector2 _wallJumpForce = new Vector2(m_wallJumpForce.x * m_wallOutDirection, m_wallJumpForce.y);
            m_rb.AddForce(_wallJumpForce, m_wallJumpMode);
        }
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
            m_disableHorizontalControl = false;
            m_rb.velocity = Vector3.zero;
        }
    }

	public void CrouchCheck()
	{
		//if the player is crouching but there's no ceiling detected, we finish crouching
		if (state_crouching)
		{
			if (!Physics2D.OverlapCircle(m_ceilingCheckPos.position, const_ceilingCheckRadius, m_groundLayerMask)) ExecuteCrouch(false);
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
    public void DoubleJump(float _jumpForce, ForceMode2D _jumpMode)
    {
        EndJump();
        m_rb.AddForce(new Vector2(0f, _jumpForce), _jumpMode);
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
