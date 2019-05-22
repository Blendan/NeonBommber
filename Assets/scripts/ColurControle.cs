using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColurControle : MonoBehaviour
{
    public SpriteRenderer self;
    public GameObject glow, timer;

    public float r = 0f, g = 0f, b = 0f;

    private void Start()
    {
        self.color = new Color(r, g, b);
        glow.GetComponent<Light>().color = new Color(r, g, b);
    }

    public void SetColur(float r, float g, float b)
    {
        this.r = r;
        this.g = g;
        this.b = b;

        self.color = new Color(r, g, b);
        glow.GetComponent<Light>().color = new Color(r, g, b);
    }


    public void SetStrenght(float percent)
    {
        self.color = new Color(r * percent, g * percent, b * percent);
    }

    public void SetTimerOff()
    {
        timer.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }

    public void SetTimer(float percent)
    {
        timer.GetComponent<SpriteRenderer>().color = new Color((float)percent, (float)(1 - percent), 0f);
    }
}
