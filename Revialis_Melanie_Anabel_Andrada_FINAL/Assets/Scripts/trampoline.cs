using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trampoline : MonoBehaviour
{

    public float bounce = 50.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce((Vector2.up * bounce)*1.5f, ForceMode2D.Impulse);
        }
    }
}
