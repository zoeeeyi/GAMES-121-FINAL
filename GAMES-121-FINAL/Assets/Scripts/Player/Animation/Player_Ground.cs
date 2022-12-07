using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ground : StateMachineBehaviour
{
    Rigidbody2D m_rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_rb == null) m_rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("Horizontal Speed", Mathf.Abs(m_rb.velocity.x));

        //Vertical Movement Triggers
        if (m_rb.velocity.y > 0) PlayerAnimation.instance.Jump();
        if (m_rb.velocity.y < 0) PlayerAnimation.instance.Fall();
    }
}