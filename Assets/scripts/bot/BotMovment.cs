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

    private List<DirectonLooker> directonLookers = new List<DirectonLooker>();
    // Start is called before the first frame update
    void Start()
    {
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
            float moveToY = 0f, moveToX = 0f;
            bool allSave = true;

            foreach(DirectonLooker directonLooker in directonLookers)
            {
                float dicance = directonLooker.getDistance(maxDistance, transform);

                if(dicance != -1)
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

            if(allSave)
            {
                Debug.Log("Save");
                Transform other = null;
                if (hasBomb)
                {
                    other = GameObject.FindGameObjectsWithTag("Hole")[0].transform;
                }
                else
                {
                    float closets = 100;
                    GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");

                    foreach(GameObject bomb in bombs)
                    {
                        if(closets> Vector3.Distance(bomb.transform.position, transform.position))
                        {
                            closets = Vector3.Distance(bomb.transform.position, transform.position);
                            other = bomb.transform;
                        }
                    }
                }
                if (other != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, other.position, movmentSpeed * Time.deltaTime);
                }

            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(moveToX, moveToY) + transform.position, movmentSpeed * Time.deltaTime);
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

    internal void Hit()
    {
        throw new NotImplementedException();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        String name = other.tag;
        bool destroy = false;

        if (name == "Hole")
        {
            if (hasBomb)
            {
                colurControle.SetTimerOff();
                destroy = true;
                hasBomb = false;
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
                destroy = true;

                BombTimer bombTimer = (BombTimer)other.gameObject.GetComponent("BombTimer");

                fuse = bombTimer.fuse;
                explosionTime = bombTimer.TimeNow;
            }
        }
        else if (name == "Explosion")
        {
            stunned = 2.5f;
        }

        if (destroy)
        {
            Destroy(other.gameObject);
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(moveX,moveY), maxDistance);

        if (hit.collider != null && hit.collider.tag != "Bomb" && hit.collider.tag != "Hole" && hit.collider.tag != "Border")
        {
            return Vector3.Distance(hit.transform.position, transform.position);
        }
        else
        {
            return -1;
        }
    }

}
