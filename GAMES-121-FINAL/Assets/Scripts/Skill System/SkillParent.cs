using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SkillParent : MonoBehaviour
{
    protected CharacterMovement m_characterMovement;
    protected bool m_toBeDestroyed = false;
    protected bool m_canExecute = true;

    protected virtual void Awake()
    {
        m_characterMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
    }

    protected virtual void Update()
    {
        if (Input.GetButtonDown("Fire") && m_canExecute)
        {
            if (m_toBeDestroyed) m_canExecute = false;
            ExecuteSkill();
        }
    }

    protected abstract void ExecuteSkill();
    public abstract void SetToBeDestroyed();
}
