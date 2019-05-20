using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BombTimer : MonoBehaviour
{
    public float fuse = 30f;
    public Spawner spawner;
    public SpriteRenderer spriteRenderer;
    private DateTime explosionTime;

    public DateTime ExplosionTime { get => explosionTime; set => explosionTime = value; }

    // Start is called before the first frame update
    void Start()
    {
        ExplosionTime = DateTime.Now.AddSeconds(fuse);
    }

    // Update is called once per frame
    void Update()
    {
        if (ExplosionTime < DateTime.Now)
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
        float delta = (float)(ExplosionTime - DateTime.Now).TotalSeconds;
        float prozent = 1 - delta / (float)fuse;

        Color temp =  new Color((float)prozent, (float)(1 - prozent), 0f);
        spriteRenderer.color = temp;
    }
}
