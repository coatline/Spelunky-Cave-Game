using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float time;
    float timer;

    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > time)
        {
            //Blow up
            BlowUp();
        }
    }

    void BlowUp()
    {
        Destroy(gameObject);
    }
}
