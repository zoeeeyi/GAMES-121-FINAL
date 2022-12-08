using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectRange : MonoBehaviour
{
    private Turret m_enemyParent;
    // Start is called before the first frame update
    void Start()
    {
        m_enemyParent = GetComponentInParent<Turret>();
    }

    #region Player in and out of the range
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") m_enemyParent.DetectionSwitch(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") m_enemyParent.DetectionSwitch(false);
    }
    #endregion
}
