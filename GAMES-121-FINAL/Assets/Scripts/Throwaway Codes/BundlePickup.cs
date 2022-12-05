using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Temp referenced in: EnemyHealth.cs

public class BundlePickup : MonoBehaviour
{
    [SerializeField] GameObject m_bundle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            /*            GameObject _newBundle = Instantiate(m_bundle, Vector2.zero, Quaternion.identity);
                        _newBundle.SetActive(false);
                        GameObject.Find("Simple Inventory")?.GetComponent<SimpleInventory>().AddToBundleList(_newBundle);
            */
            if (CardSystem.instance.AddCard(m_bundle)) Destroy(gameObject);
        }
    }
}
