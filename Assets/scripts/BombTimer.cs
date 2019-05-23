using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BombTimer : MonoBehaviour
{
    public float fuse = 7f;
    public Spawner spawner;
    public SpriteRenderer spriteRenderer;
    private float timeNow;

    public float TimeNow { get => timeNow; set => timeNow = value; }

    // Start is called before the first frame update
    void Start()
    {
        TimeNow = fuse;
    }

    // Update is called once per frame
    void Update()
    {
        /* if (ExplosionTime < DateTime.Now)
         {
             explode();
         }*/
        TimeNow -= Time.deltaTime;
        if (TimeNow<=0)
        {
            explode();
        }
        SetColour();
    }

    private void explode()
    {
        spawner.SetBomb();
        Debug.Log("BOOOOOOOMMMM");
        spawner.Explode(gameObject);
        Destroy(gameObject);
    }

    private void SetColour()
    {
        float prozent = 1 - TimeNow / (float)fuse;

        Color temp =  new Color((float)prozent, (float)(1 - prozent), 0f);
        spriteRenderer.color = temp;
    }
}
