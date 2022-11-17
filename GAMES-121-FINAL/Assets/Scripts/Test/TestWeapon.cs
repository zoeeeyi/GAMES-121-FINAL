using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestWeapon : MonoBehaviour
{
    [SerializeField] private Skills.SkillType m_skillType;
    private UnityEvent event_ExecuteSkill;

    private void Start()
    {
        CharacterMovement _cm = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
        event_ExecuteSkill.AddListener(() => _cm.ExecuteSkill(m_skillType));
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire"))
        {
            Fire();
        }

        if (Input.GetButtonDown("Skill Move"))
        {
            event_ExecuteSkill.Invoke();
        }
    }

    void Fire() 
    {
        Debug.Log("Fire");
    }

    public void Test(string _test)
    {

    }
}
