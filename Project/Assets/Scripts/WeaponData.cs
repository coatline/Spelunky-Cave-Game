using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponData : ScriptableObject
{
    public Sprite mySprite;
    public int minDamage;
    public int maxDamage;
    public string myName;
}
