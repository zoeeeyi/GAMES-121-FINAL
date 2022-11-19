using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour, interface_Skills
{
    #region Movement Variables
    [Header("Movement")]
	[SerializeField] private float m_horizontalSpeed = 400;
	[Range(0, 1)] [SerializeField] private float m_crouchSpeedMult = .36f;
    [Range(0, 2)][SerializeField] private float m_airSpeedMult = 1;             // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private float m_jumpForce = 400f; // Amount of force added when the player jumps.
    [SerializeField] private ForceMode2D m_jumpMode = ForceMode2D.Impulse;
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothingTime = .05f;   // How much to smooth out the movement
    [SerializeField] float m_normalGrav;
    [SerializeField] float m_fallingGrav;
    private Vector3 m_movementSmoothV = Vector3.zero;
    #endregion

    #region Collision Variables
    [Header("Collision Check")]
	[SerializeField] private LayerMask m_groundLayerMask;						// A mask determining what is ground to the character
	[SerializeField] private Transform m_groundCheckPos;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_ceilingCheckPos;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_crouchDisableCollider;				// A collider that will be disabled when crouching
	const float const_groundCheckRadius = .2f; // Radius of the overlap circle to determine if grounded
    const float const_ceilingCheckRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    #endregion

    #region Components Variables
    [Header("Components")]
    private Rigidbody2D m_rb;
    #endregion

    #region State Variables
    [Header("States")]
    private bool state_grounded; // Whether or not the player is grounded.
    private bool state_crouching;
	private bool state_jumping = false;
    private bool m_facingRight = true;  // For determining which way the player is currently facing.
    #endregion

    private void Awake()
	{
		m_rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
        #region Ground Check
        state_grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_groundCheckPos.position, const_groundCheckRadius, m_groundLayerMask);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				state_grounded = true;
		}
        #endregion
    }

    #region Utility Methods
    private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_facingRight = !m_facingRight;

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
		m_rb.gravityScale = (Mathf.Approximately(m_rb.velocity.y, 0) || m_rb.velocity.y < 0) ? m_fallingGrav : m_normalGrav;

		//Apply speed multiplier
		if (state_grounded)
		{
			if (state_crouching) _move *= m_crouchSpeedMult;
		} else
		{
			_move *= m_airSpeedMult;
		}

        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(_move * m_horizontalSpeed * Time.deltaTime, m_rb.velocity.y);
		if (m_MovementSmoothingTime > 0) m_rb.velocity = Vector3.SmoothDamp(m_rb.velocity, targetVelocity, ref m_movementSmoothV, m_MovementSmoothingTime);
		else m_rb.velocity = targetVelocity;

        // If the input is moving the player right and the player is facing left...
        if (_move > 0 && !m_facingRight)
        {
            Flip();
        }
        else if (_move < 0 && m_facingRight)
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
            m_rb.AddForce(new Vector2(0f, m_jumpForce), ForceMode2D.Impulse);
        }
    }

	public void EndJump()
	{
		if (state_jumping)
		{
			state_jumping = false;
            //m_rb.gravityScale = m_fallingGrav;
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
