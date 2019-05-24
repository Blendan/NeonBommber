using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomObjects : MonoBehaviour
{
    public GameObject[] objects;
    public float spawnRate = 20f;

    public float minX, minY;

    private float lastSpoan = 0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        lastSpoan -= Time.deltaTime;
        if(lastSpoan<=0)
        {
            lastSpoan = spawnRate;
            if ((int)Random.Range(0,5)==3)
            {
                int index = Random.Range(0, objects.Length);
                bool isX = false;

                float x = minX, y = minY;

                if((int)Random.Range(1f,2f)==2)
                {
                    x *= -1;
                }

                if ((int)Random.Range(1f, 2f) == 2)
                {
                    y *= -1;
                }

                if((int)Random.Range(1f,2f)==2)
                {
                    isX = true;
                    x *= Random.Range(-1f, 1f);
                }
                else
                {
                    y *= Random.Range(-1f, 1f);
                }

                GameObject temp = Instantiate(objects[index], new Vector3(x, y), new Quaternion(0, 0, 0, 0)) as GameObject;

                RandomObjectMovemnt movment = temp.GetComponent<RandomObjectMovemnt>();

                if(isX)
                {
                    if (y < 0)
                    {
                        movment.y = Random.Range(2, 10);
                    }
                    else
                    {
                        movment.y = Random.Range(2, 10) *-1f;
                    }
                }
                else
                {
                    if (x < 0)
                    {
                        movment.x = Random.Range(5, 15);
                    }
                    else
                    {
                        movment.x = Random.Range(5, 15) * -1f;
                    }
                }

                movment.liveTime = Random.Range(10f, 60f);

            }
        }
    }
}
