using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow
{
    public Sprite mySprite;
    public int minDamage;
    public int maxDamage;
    public string myName;
    public bool equipped;
    public int arrowNumber;

    public Bow(int minDmg, int maxDmg, string name, Sprite sprite, int arrowNum)
    {
        minDamage = minDmg;
        myName = name;
        maxDamage = maxDmg;
        mySprite = sprite;
        arrowNumber = arrowNum;
    }
}
