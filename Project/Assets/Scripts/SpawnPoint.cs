using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject item = null;
    [SerializeField] float chance =0;

    void Start()
    {
        float rand = Random.Range(0, 101);

        if (rand <= chance)
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }
    }

    void Update()
    {

    }
}
