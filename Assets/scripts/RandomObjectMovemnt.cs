using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectMovemnt : MonoBehaviour
{
    public float x, y;
    public float liveTime = 20f;

    public float maxX, maxY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(x * Time.deltaTime,y * Time.deltaTime));

        liveTime -= Time.deltaTime;

        if(liveTime<=0)
        {
            Destroy(gameObject);
        }

        if(Math.Abs(transform.position.x)>maxX|| Math.Abs(transform.position.y) > maxY)
        {
            Destroy(gameObject);
            Debug.Log("dib");
        }
    }
}
