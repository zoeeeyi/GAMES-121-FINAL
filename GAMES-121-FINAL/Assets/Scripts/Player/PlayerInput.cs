using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerInput : MonoBehaviour {

	[SerializeField] CharacterMovement m_characterMovement;

	float input_horizontalMove = 0f;

	bool m_disableMovementInput = false;

	void Update () {
		if (m_disableMovementInput) return;

		float _inputX = Input.GetAxisRaw("Horizontal");
        input_horizontalMove = (_inputX != 0) ? Mathf.Sign(_inputX) : 0;

        #region Handle Jump
        if (Input.GetButtonDown("Jump"))
		{
			m_characterMovement.ExecuteJump();
		}

		if (Input.GetButtonUp("Jump"))
		{
			m_characterMovement.EndJump();
		}
        #endregion

        #region Handle Crouch
        if (Input.GetButtonDown("Crouch"))
		{
			m_characterMovement.ExecuteCrouch(true);
		}
		else if (Input.GetButtonUp("Crouch"))
		{
			m_characterMovement.ExecuteCrouch(false);
		}
		if (!Input.GetButton("Crouch")) m_characterMovement.CrouchCheck();
        #endregion
    }

    void FixedUpdate ()
	{
		if (m_disableMovementInput) return;

		#region Handle Basic Movement
		m_characterMovement.ExecuteBasicMove(input_horizontalMove);
        #endregion
    }

    #region Utility Methods
    public void DisableMovementInput(bool _disable, bool _freezeMovement = false)
	{
		m_disableMovementInput = _disable;
		m_characterMovement.SetFreezeMovement(_freezeMovement);
	}

	public float GetInput()
	{
		return input_horizontalMove;
	}
    #endregion
}
