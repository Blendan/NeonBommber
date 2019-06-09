using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovment : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    public Spawner spawner;
    public GameObject timer;
    public int controlerNr = -1;
    private static float movmentSpeed = 10;
    private static float dashCoolDown = 1;
    private float lastDesh = 0f;
    private float stunned = 0f;
    private float lastHit = 0f;
    public float hitRecover = 0.5f;

    private ColurControle colurControle;

    public String player = "1";

    private PointCounter pointCounter;

    private float explosionTime;
    private float fuse = 30;

    private bool hasBomb = false;

    public bool IsInDash()
    {
        return (dashCoolDown-0.3 <= lastDesh);
    }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        colurControle = GetComponent<ColurControle>();
        pointCounter = GetComponent<PointCounter>();
    }

    private void SetColour()
    {
        float delta;
        float prozent;
       
        if (hasBomb)
        {
            prozent = 1 - explosionTime / (float)fuse;

            colurControle.SetTimer(prozent);
        }

        if (stunned > 0)
        {
            prozent = 0.5f;
        }
        else
        {
            delta = (float)(dashCoolDown - lastDesh);
            prozent = delta / (float)dashCoolDown + 0.3f;
        }

        colurControle.SetStrenght(prozent);
    }

    // Start is called before the first frame update
    void Start()
    {

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

        lastHit -= Time.deltaTime;
        stunned -= Time.deltaTime;
        lastDesh -= Time.deltaTime;

        if (lastHit > 0)
        {
            smoothing = 1f-((float)(hitRecover- lastHit) / hitRecover )+0.3f;
        }


        if(hasBomb)
        {
            explosionTime -= Time.deltaTime;
            if (explosionTime <= 0)
            {
                Explode();
            }
        }

        if (jump >= 0.5f)
        {
            if (lastDesh<=0)
            {
                speed = 50;
                lastDesh = dashCoolDown;
            }
        }

        if(IsInDash())
        {
            speed = 30;
        }

        if(stunned>0)
        {
            speed = 0;
        }


        Vector4 targetVelocity = new Vector4(horizontal * speed, vertical * speed);

        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, smoothing);
        
    }

    private float getAxes(String axes)
    {
        float returnValue = 0;
        if (player == "2" || player == "1")
        {
            returnValue = Input.GetAxis(axes + player);
        }

        if(returnValue == 0 && controlerNr != 0)
        {
            switch (axes)
            {
                case "Jump":
                    returnValue = Math.Max(GetJoystickKey(0), Input.GetAxis("J" + axes + controlerNr));
                    break;
                default:
                    returnValue = Input.GetAxis("J" + axes + controlerNr);
                    break;
            }
        }

        return returnValue;
    }

    private float GetJoystickKey(int nr)
    {
        if (Input.GetKeyDown("joystick "+ controlerNr + " button "+nr))
        {
            return 1f;
        }
        return 0f;
    }

    private void Explode()
    {
        spawner.SetBomb();
        colurControle.SetTimerOff();
        spawner.Explode(gameObject);
        hasBomb = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        String name = other.name.Replace("(Clone)","");

        if (name == "Hole")
        {
            if(hasBomb)
            {
                colurControle.SetTimerOff();
                Destroy(other.gameObject);

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
                colurControle.SetTimer(1f);
                hasBomb = true;
                Destroy(other.gameObject);

                BombTimer bombTimer = (BombTimer) other.gameObject.GetComponent("BombTimer");

                fuse = bombTimer.fuse;
                explosionTime = bombTimer.TimeNow;
            }
        }
        else if(name == "Explosion")
        {
            stunned = 2.5f;
        }
    }

    public void Hit()
    {
        lastHit = hitRecover;
    }
}
