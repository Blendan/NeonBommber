using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movment : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    public Spawner spawner;
    public GameObject timer;
    private static float movmentSpeed = 10;
    private static float dashCoolDown = 1;
    private SpriteRenderer spriteRenderer;
    private DateTime lastDesh = DateTime.Now;
    private DateTime stunned = DateTime.Now;
    private DateTime lastHit = DateTime.Now;
    public float hitRecover = 0.5f; 

    public String player = "1";

    public PointCounter pointCounter;

    private DateTime explosionTime;
    private float fuse = 30;


    private bool hasBomb = false;

    public bool isInDash()
    {
        return (0.2 > (DateTime.Now - lastDesh).TotalSeconds);
    }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void SetColour()
    {
        float delta;
        float prozent;
        Color temp;
        if (hasBomb)
        {
            delta = (float)(explosionTime - DateTime.Now).TotalSeconds;
            prozent = 1 - delta / (float)fuse;

            temp = new Color((float)prozent, (float)(1 - prozent), 0f);
            spriteRenderer.color = temp;
        }

        if (stunned > DateTime.Now)
        {
            prozent = 0.5f;
        }
        else
        {
            delta = (float)(DateTime.Now - lastDesh).TotalSeconds;
            prozent = delta / (float)dashCoolDown + 0.3f;
        }

        if (player == "1")
        {
            temp = new Color(0, (float)prozent, 0f);
        }
        else
        {
            temp = new Color((float)prozent, 0f, (float)prozent);
        }
        gameObject.GetComponent<SpriteRenderer>().color = temp;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = timer.GetComponent<SpriteRenderer>();

        if(player=="1")
        {
            spawner.SetHole();
        }
        spawner.SetBomb();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = movmentSpeed;
        float smoothing = 0f;

        float horizontal = getAxes("Horizontal");
        float vertical = getAxes("Vertical");
        float jump = getAxes("Jump");

        SetColour();

        if(lastHit.AddSeconds(hitRecover) > DateTime.Now)
        {
            smoothing = 1f-((float)(DateTime.Now- lastHit).TotalSeconds / hitRecover )+0.3f;
        }

        if (explosionTime < DateTime.Now && hasBomb)
        {
            explode();
        }

        if (jump >= 0.5f)
        {
            if (dashCoolDown < (DateTime.Now - lastDesh).TotalSeconds)
            {
                speed = 50;
                lastDesh = DateTime.Now;
            }
        }

        if(isInDash())
        {
            speed = 30;
        }

        if(stunned>DateTime.Now)
        {
            speed = 0;
        }


        Vector4 targetVelocity = new Vector4(horizontal * speed, vertical * speed);

        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, smoothing);
        
    }

    private float getAxes(String axes)
    {
        float returnValue = Input.GetAxis(axes + player);

        if(returnValue == 0)
        {
            returnValue = Input.GetAxis("J"+axes + player);
        }

        return returnValue;
    }

    private void explode()
    {
        spawner.SetBomb();
        spriteRenderer.color = new Color(1f, 1f, 1f);
        spawner.Explode(gameObject);
        hasBomb = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        String name = other.name.Replace("(Clone)","");
        bool destroy = false;

        if (name == "Hole")
        {
            if(hasBomb)
            {
                spriteRenderer.color = new Color(1f, 1f, 1f);
                destroy = true;
                hasBomb = false;
                spawner.SetBomb();
                spawner.SetHole();
                pointCounter.madePoint();
            }
        }
        else if(name == "bomb")
        {
            if (!hasBomb)
            {
                spriteRenderer.color = new Color(1f, 0f, 0f);
                hasBomb = true;
                destroy = true;

                BombTimer bombTimer = (BombTimer) other.gameObject.GetComponent("BombTimer");

                fuse = bombTimer.fuse;
                explosionTime = bombTimer.ExplosionTime;
            }
        }
        else if(name == "Explosion")
        {
            stunned = DateTime.Now.AddSeconds(5);
        }

        if (destroy)
        {
            Destroy(other.gameObject);
        }
    }

    public void Hit()
    {
        lastHit = DateTime.Now;
    }
}
