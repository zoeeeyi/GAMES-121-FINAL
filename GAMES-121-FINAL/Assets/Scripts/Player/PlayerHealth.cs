using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthSystemParent
{
    [SerializeField] PlayerHealthAgent so_playerHealthAgent;

    protected override void Start()
    {
        base.Start();

        so_playerHealthAgent.UpdatePlayerHealth.Invoke(m_currentHealth);
    }

    public override void TakeDamage()
    {
        base.TakeDamage();

        //Invoke update health event
        if (m_currentHealth < m_totalHealth && m_currentHealth >= 0) so_playerHealthAgent.UpdatePlayerHealth.Invoke(m_currentHealth);
    }

    protected override void PreDeathEvent()
    {
        //Destroy all the existing weapon/skill bundles
        GameObject[] _bundles = GameObject.FindGameObjectsWithTag("Bundle");
        foreach (GameObject _bundle in _bundles) Destroy(_bundle);

        DeathEvent();
    }

    protected override void DeathEvent()
    {
        NeonRounds.instance?.GameStateMachine.HandleRestartLevel(GameStateMachine.TriggerType.Key);
        //Destroy self
        Destroy(gameObject);
    }
}
