using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Card[3] Hand;
    public Card Card_in_Hand;
    int Active_Card;

    // Start is called before the first frame update
    void Start()
    {
        Card_in_Hand[0] = Card("Katana");
        Card_in_Hand[1] = Card();
        Card_in_Hand[2] = Card();
        Active_Card= 0;
        Card_in_Hand= Hand[Active_Card];
    }

    // Update is called once per frame
    void Update()
    {
        Card_in_Hand = Hand[Active_Card];
        Weapon();
    }
    void Weapon() 
    {
        //weapontrigger
        //Card_in_Hand.Fire();
        //weaponchange
        //weaponaiming
    }
    public void CardPickup(Card crd)
    {
        Card_in_Hand = crd;
    }

}
