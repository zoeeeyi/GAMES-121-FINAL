using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour {

	[SerializeField] CharacterController2D m_characterController;

	float input_horizontalMove = 0f;

	void Update () {
		input_horizontalMove = Input.GetAxisRaw("Horizontal");

        #region Handle Jump
        if (Input.GetButtonDown("Jump"))
		{
			m_characterController.ExecuteJump();
		}
        #endregion

        #region Handle Crouch
        if (Input.GetButtonDown("Crouch"))
		{
			m_characterController.ExecuteCrouch(true);
		}
		else if (Input.GetButtonUp("Crouch"))
		{
			m_characterController.ExecuteCrouch(false);
		}
		if (!Input.GetButton("Crouch")) m_characterController.CrouchCheck();
        #endregion
    }

    void FixedUpdate ()
	{
		#region Handle Basic Movement
		m_characterController.ExecuteBasicMove(input_horizontalMove);
        #endregion
    }
}
