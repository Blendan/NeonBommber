using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private DateTime end;
    // Start is called before the first frame update
    void Start()
    {
        end = DateTime.Now.AddSeconds(0.2);
    }

    // Update is called once per frame
    void Update()
    {
        if(end<DateTime.Now)
        {
            Destroy(gameObject);
        }
        transform.Rotate(Vector3.forward * -30);
    }
}
