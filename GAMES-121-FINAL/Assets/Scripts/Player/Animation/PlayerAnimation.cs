using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public static PlayerAnimation instance;

    Animator m_animator;

    public Rigidbody2D rb { get; private set; }

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_animator.SetBool("Ground", true);
    }

    public void Ground(bool _b)
    {
        m_animator.SetBool("Ground", _b);
    }

    public void Jump()
    {
        m_animator.SetTrigger("Jump");
    }

    public void Fall()
    {
        m_animator.SetTrigger("Fall");
    }
}
