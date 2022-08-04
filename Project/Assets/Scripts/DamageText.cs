using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    TMP_Text myText;

    void Start()
    {
        myText = GetComponent<TMP_Text>();

        y = transform.position.y + Random.Range(.1f, .25f);

        x = Random.Range(-.005f, .006f);
    }

    float alpha = 1;
    float y;
    float x;

    void Update()
    { 
        transform.position = new Vector3(transform.position.x + x, y);

        y += Time.deltaTime;

        alpha -= Time.deltaTime / 1.75f;

        myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alpha);

        if (alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}
