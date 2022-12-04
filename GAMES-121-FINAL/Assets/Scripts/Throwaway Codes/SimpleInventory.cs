using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Temp referenced in: SkillParent.cs, BundlePickup.cs

public class SimpleInventory : MonoBehaviour
{
    [SerializeField] List<GameObject> m_bundles;

    [Header("Card System")]
    CardSystem m_cardSystem;

    private void Start()
    {
        m_bundles = GameObject.FindGameObjectsWithTag("Bundle").ToList();

        m_cardSystem = GameObject.FindGameObjectWithTag("Card System").GetComponent<CardSystem>();
    }

    public void AddToBundleList(GameObject _b)
    {
        if (m_bundles.Count == 0) _b.SetActive(true);
        m_bundles.Add(_b);
    }

    public void ChangeActiveBundle()
    {
        if (m_bundles.Count > 1) m_bundles[1].SetActive(true);
        m_bundles.Remove(m_bundles[0]);
    }
}
