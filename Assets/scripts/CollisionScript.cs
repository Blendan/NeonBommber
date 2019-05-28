using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    public GameObject funken;
    public Movment movment;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 vector = gameObject.transform.position;
            GameObject temp = Instantiate(funken, vector, new Quaternion(0, 0, 0, 0)) as GameObject;

            if (collision.gameObject.GetComponent<Movment>() != null && movment != null)
            {
                if (collision.gameObject.GetComponent<Movment>().IsInDash())
                {
                    if (!movment.IsInDash())
                    {
                        movment.Hit();
                    }
                }
            }
        }
    }
}
