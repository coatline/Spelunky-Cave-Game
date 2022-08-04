using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowGenerator : MonoBehaviour
{
    public Sprite startingBowSprite;
    public Bow startingBow;

    public List<Bow> bows;
    public Sprite[] sprites;
    public string[] names;
    public int[] minDamage;
    public int[] maxDamage;
    public int[] arrowNum;

    private void OnEnable()
    {
        bows = new List<Bow>();

        startingBow = new Bow(3, 5, "Worn Bow", startingBowSprite, 1);

        for (int i = names.Length - 1; i >= 0; i--)
        {
            getData(i);
        }
    }

    public Bow getData(int index)
    {
        var bow = new Bow(minDamage[index], maxDamage[index], names[index], sprites[index], arrowNum[index]);
        bows.Add(bow);
        return bow;
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




    static BowGenerator instance;

    public static BowGenerator I
    {
        get
        {
            if (instance == null)
            {
                var gob = new GameObject("Item Handler");
                gob.AddComponent<BowGenerator>();
            }
            return instance;
        }
    }
}
