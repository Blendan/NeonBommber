using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float width, height;

    public GameObject bomb,hole, exlposion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetHole()
    {
        if (GameObject.FindGameObjectsWithTag("Hole").Length == 1 || GameObject.FindGameObjectsWithTag("Hole").Length == 0)
        {
            GameObject temp;
            float x = Random.Range(width / 2 * -1, width / 2);
            float y = Random.Range(height / 2 * -1, height / 2);
            temp = Instantiate(hole, new Vector3(x, y), new Quaternion(0, 0, 0, 0)) as GameObject;
        }
    }

    public void Explode(GameObject gameObject)
    {
        Vector3 vector = gameObject.transform.position;
        GameObject temp = Instantiate(exlposion, vector, new Quaternion(0, 0, 0, 0)) as GameObject;
    }

    public void SetBomb()
    {
        GameObject temp;
        float x = Random.Range(width / 2 * -1, width / 2);
        float y = Random.Range(height / 2 * -1, height / 2);
        temp = Instantiate(bomb, new Vector3(x, y), new Quaternion(0, 0, 0, 0)) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
