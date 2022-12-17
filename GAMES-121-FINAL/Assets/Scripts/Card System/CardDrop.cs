using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Temp referenced in: EnemyHealth.cs

public class CardDrop : MonoBehaviour
{
    [SerializeField] GameObject m_dropBundle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            /*            GameObject _newBundle = Instantiate(m_bundle, Vector2.zero, Quaternion.identity);
                        _newBundle.SetActive(false);
                        GameObject.Find("Simple Inventory")?.GetComponent<SimpleInventory>().AddToBundleList(_newBundle);
            */
            if (CardSystem.instance != null)
            {
                if (CardSystem.instance.AddCard(m_dropBundle)) Destroy(gameObject);
            }
            else
            {
                Instantiate(m_dropBundle, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
