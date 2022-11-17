using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public string name;
    public Card(string nname) 
    {
        if (nname != null) { name = nname; } else { name = ""; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Fire() { }
    void MoveSkill() { }
}
