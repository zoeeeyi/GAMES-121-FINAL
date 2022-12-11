using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthSystemParent
{
    [SerializeField] Image m_healthBarUI;
    [SerializeField] Sprite[] m_healthBars;

    protected override void Start()
    {
        base.Start();
        m_healthBarUI.sprite = m_healthBars[m_healthBars.Length - 1];
    }

    public override void TakeDamage()
    {
        base.TakeDamage();

        //Update UI
        if (m_currentHealth < m_healthBars.Length) m_healthBarUI.sprite = m_healthBars[m_currentHealth];
    }

    protected override void PreDeathEvent()
    {
        //Destroy all the existing weapon/skill bundles
        GameObject[] _bundles = GameObject.FindGameObjectsWithTag("Bundle");
        foreach (GameObject _bundle in _bundles) Destroy(_bundle);

        //Destroy self
        Destroy(gameObject);
    }

    protected override void DeathEvent()
    {
    }
}
