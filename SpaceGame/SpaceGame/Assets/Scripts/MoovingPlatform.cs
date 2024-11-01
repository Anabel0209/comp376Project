using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoovingPlatform : MonoBehaviour
{
    public Transform target1;
    public Transform target2;
    public float speed;

    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = target1.position;
    }

    // Update is called once per frame
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
