using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]

public class WeaponGenerator : MonoBehaviour
{
    public Sprite startingWeaponSprite;
    public Weapon startingWeapon;

    public List<Weapon> weapons;
    public Sprite[] sprites;
    public string[] names;
    public int[] minDamage;
    public int[] maxDamage;

    private void OnEnable()
    {
        weapons = new List<Weapon>();

        startingWeapon = new Weapon(4, 6, "Small Dagger", startingWeaponSprite);

        for (int i = names.Length - 1; i >= 0; i--)
        {
            GetData(i);
        }
    }

    public Weapon GetData(int index)
    {
        var weapon = new Weapon(minDamage[index], maxDamage[index], names[index], sprites[index]);
        weapons.Add(weapon);
        return weapon;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"There is another instance of {GetType().Name} already in the scene. Deleting this one!");
            DestroyImmediate(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }







    static WeaponGenerator instance;

    public static WeaponGenerator I
    {
        get
        {
            if (instance == null)
            {
                var gob = new GameObject("Item Handler");
                gob.AddComponent<WeaponGenerator>();
            }
            return instance;
        }
    }
}
