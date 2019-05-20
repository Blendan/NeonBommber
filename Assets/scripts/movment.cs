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
    private SpriteRenderer spriteRenderer;
    private DateTime lastDesh = DateTime.Now;
    private DateTime stunned = DateTime.Now;
    [Range(0, 0.3f)] [SerializeField] private float m_MovementSmoothing = .05f;

    public String player = "1";

    public PointCounter pointCounter;

    private DateTime explosionTime;
    private float fuse = 30;


    private bool hasBomb = false;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void SetColour()
    {
        float delta = (float)(explosionTime - DateTime.Now).TotalSeconds;
        float prozent = 1 - delta / (float)fuse;

        Color temp = new Color((float)prozent, (float)(1 - prozent), 0f);
        spriteRenderer.color = temp;
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
        float speed = 10;

        float horizontal = Input.GetAxis("Horizontal" + player);
        float vertical = Input.GetAxis("Vertical" + player);
        float jump = Input.GetAxis("Jump" + player);

        if (hasBomb)
        {
            SetColour();

            if (explosionTime < DateTime.Now)
            {
                explode();
            }
        }

       

        if (jump >= 0.5f)
        {
            if (2 < (DateTime.Now - lastDesh).TotalSeconds)
            {
                speed = 50;
                lastDesh = DateTime.Now;
            }
        }

        if(0.2 > (DateTime.Now - lastDesh).TotalSeconds)
        {
            speed = 30;
        }

        if(stunned>DateTime.Now)
        {
            speed = 0;
        }

        Vector3 targetVelocityY = new Vector2(horizontal * speed, m_Rigidbody2D.velocity.y);

        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocityY, ref m_Velocity, m_MovementSmoothing);

        Vector4 targetVelocityX = new Vector4(m_Rigidbody2D.velocity.x, vertical * speed);

        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocityX, ref m_Velocity, m_MovementSmoothing);
        
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
}
