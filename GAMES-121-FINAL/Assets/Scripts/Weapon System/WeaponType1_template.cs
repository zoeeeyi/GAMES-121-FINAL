using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponType1_template : WeaponParent
{
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Fire()
    {
        Debug.Log("Fire");
    }
}
