using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Weapon weapon = null;
    public Bow bow = null;
    public string myName = "";
    public string type = "";
    public bool callStart = true;

    private void Start()
    {
        if (!callStart)
        {
            return;
        }


        if (type == "Weapon")
        {
            var rand = Random.Range(0, WeaponGenerator.I.weapons.Count);

            weapon = WeaponGenerator.I.weapons[rand];

            myName = weapon.myName;

            if (!GetComponent<SpriteRenderer>())
            {
                gameObject.AddComponent<SpriteRenderer>();
            }

            GetComponent<SpriteRenderer>().sprite = weapon.mySprite;

        }
        else if(type == "Bow")
        {
            var rand = Random.Range(0, BowGenerator.I.bows.Count);

            bow = BowGenerator.I.bows[rand];

            myName = bow.myName;

            if (!GetComponent<SpriteRenderer>())
            {
                gameObject.AddComponent<SpriteRenderer>();
            }

            GetComponent<SpriteRenderer>().sprite = bow.mySprite;
        }

        gameObject.name = myName;
    }


}
