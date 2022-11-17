using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SkillParent : MonoBehaviour
{
    [SerializeField] protected CharacterMovement m_characterMovement;

    protected virtual void Awake()
    {
        m_characterMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
    }

    protected virtual void Update()
    {
        if (Input.GetButtonDown("Fire"))
        {
            ExecuteSkill();
        }
    }

    protected abstract void ExecuteSkill();
}
