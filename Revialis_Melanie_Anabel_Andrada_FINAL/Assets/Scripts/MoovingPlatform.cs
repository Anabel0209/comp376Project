using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoovingPlatform : MonoBehaviour
{
    public Transform target1;
    public Transform target2;
    public float speed;

    Vector2 direction;

    void Start()
    {
        direction = target1.position;
    }

    void Update()
    {
        if(Vector2.Distance(transform.position, target1.position) < 0.1f)
        {
            direction = target2.position;
        }
        if (Vector2.Distance(transform.position, target2.position) < 0.1f)
        {
            direction = target1.position;
        }
        transform.position = Vector2.MoveTowards(transform.position, direction, speed * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
