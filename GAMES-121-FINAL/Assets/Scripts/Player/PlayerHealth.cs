using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthSystemParent
{
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
