using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovment : MonoBehaviour
{
    public float maxDistance = 1.5f;
    public Spawner spawner;
    public GameObject timer;
    private static float movmentSpeed = 10;
    private static float dashCoolDown = 1;
    private float lastDesh = 0f;
    private float stunned = 0f;
    private float lastHit = 0f;
    public float hitRecover = 0.5f;

    public ColurControle colurControle;

    public String player = "1";

    public PointCounter pointCounter;

    private float explosionTime;
    private float fuse = 30;
    private bool hasBomb = false;

    private Vector3 target;
    private float lastTargeting = 0;

    private List<DirectonLooker> directonLookers = new List<DirectonLooker>();
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        spawner.SetBomb();

        Physics2D.queriesStartInColliders = false;

        directonLookers.Add(new DirectonLooker(1, 0));
        directonLookers.Add(new DirectonLooker(1, 1));
        directonLookers.Add(new DirectonLooker(0, 1));
        directonLookers.Add(new DirectonLooker(-1, 0));
        directonLookers.Add(new DirectonLooker(-1, -1));
        directonLookers.Add(new DirectonLooker(0, -1));
        directonLookers.Add(new DirectonLooker(1, -1));
        directonLookers.Add(new DirectonLooker(-1, 1));
    }

    // Update is called once per frame
    void Update()
    {
        SetColour();

        lastHit -= Time.deltaTime;
        stunned -= Time.deltaTime;
        lastDesh -= Time.deltaTime;
        lastTargeting -= Time.deltaTime;

        if (hasBomb)
        {
            explosionTime -= Time.deltaTime;
            if (explosionTime <= 0)
            {
                Explode();
            }
        }

        if (stunned <= 0)
        {
            if (lastHit <= 0)
            {
                float moveToY = 0f, moveToX = 0f;
                bool allSave = true;

                foreach (DirectonLooker directonLooker in directonLookers)
                {
                    float dicance = directonLooker.getDistance(maxDistance, transform);

                    if (dicance != -1)
                    {
                        moveToX -= directonLooker.MoveX;
                        moveToY -= directonLooker.MoveX;
                        allSave = false;
                    }
                    else
                    {
                        moveToX += directonLooker.MoveX;
                        moveToY += directonLooker.MoveX;
                    }
                }

                if (allSave)
                {
                    Transform other = null;
                    if (hasBomb)
                    {
                        other = GameObject.FindGameObjectsWithTag("Hole")[0].transform;
                    }
                    else
                    {
                        float closets = 100;
                        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");

                        foreach (GameObject bomb in bombs)
                        {
                            if (closets > Vector3.Distance(bomb.transform.position, transform.position))
                            {
                                closets = Vector3.Distance(bomb.transform.position, transform.position);
                                other = bomb.transform;
                            }
                        }
                    }

                    if (other != null && lastTargeting <= 0)
                    {
                        lastTargeting = 1f;

                        target = other.position;
                    }
                }
                else
                {
                    target = new Vector3(moveToX, moveToY) + transform.position;
                }
            }

            if (target != null)
            {
                if (lastHit <= 0)
                {
                    m_Rigidbody2D.MovePosition(Vector3.MoveTowards(transform.position, target, movmentSpeed * Time.deltaTime));
                }
                else
                {
                    Vector3 moveDirection = Vector3.MoveTowards(transform.position, target, movmentSpeed * Time.deltaTime) - transform.position;

                    m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, moveDirection, ref m_Velocity, 1f);
                }
            }

        }
    }

    private void Explode()
    {
        spawner.SetBomb();
        colurControle.SetTimerOff();
        spawner.Explode(gameObject);
        hasBomb = false;
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

    public bool IsInDash()
    {
        return (dashCoolDown - 0.3 <= lastDesh);
    }

    public void Hit()
    {
        lastHit = hitRecover;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        String name = other.tag;

        if (name == "Hole")
        {
            if (hasBomb)
            {
                colurControle.SetTimerOff();
                hasBomb = false;
                Destroy(other.gameObject);

                spawner.SetBomb();
                spawner.SetHole();
                pointCounter.madePoint();
            }
        }
        else if (name == "Bomb")
        {
            if (!hasBomb)
            {
                colurControle.SetTimer(1f);
                hasBomb = true;

                Destroy(other.gameObject);

                BombTimer bombTimer = (BombTimer)other.gameObject.GetComponent("BombTimer");

                fuse = bombTimer.fuse;
                explosionTime = bombTimer.TimeNow;
            }
        }
        else if (name == "Explosion")
        {
            stunned = 2.5f;
        }
    }
}

class DirectonLooker
{
    private float moveX, moveY;

    public DirectonLooker(float moveX, float moveY)
    {
        this.moveX = moveX;
        this.moveY = moveY;
    }

    public float MoveX { get => moveX; set => moveX = value; }
    public float MoveY { get => moveY; set => moveY = value; }

    public float getDistance(float maxDistance, Transform transform)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(moveX, moveY), maxDistance);

        if (hit.collider != null && hit.collider.tag != "Bomb" && hit.collider.tag != "Hole" && hit.collider.tag != "Border" && hit.collider.tag != "Player")
        {
            return Vector3.Distance(hit.transform.position, transform.position);
        }
        else
        {
            return -1;
        }
    }

}
