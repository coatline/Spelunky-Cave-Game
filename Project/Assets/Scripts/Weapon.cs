using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public Sprite mySprite;
    public int minDamage;
    public int maxDamage;
    public string myName;

    public Weapon(int minDmg, int maxDmg, string name, Sprite sprite)
    {
        minDamage = minDmg;
        myName = name;
        maxDamage = maxDmg;
        mySprite = sprite;
    }
}

