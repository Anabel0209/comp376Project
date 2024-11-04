using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class HealthManagement : MonoBehaviour
{
    public int healthPoints = 5;
    float damageTimer = 0f;
    float timeInBetweenDamage = 2.0f;
    bool inLava = false;

    //ui heart
    public GameObject[] myHearts;

    // Update is called once per frame
    void Update()
    {
        //take damage one uppon entering the collider then every 2 seconds while staying in it
        if (inLava)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= timeInBetweenDamage)
            {
                TakeDamage(1);
                damageTimer = 0f;
                Debug.Log(healthPoints);
            }
        }
    }
    //method to call when the player takes damage
    private void TakeDamage(int damage)
    {
       if(healthPoints > 0)
        {
            healthPoints -= damage;
            myHearts[healthPoints].SetActive(false);
        }
        else
        {
            Debug.Log(healthPoints + ": Player dead");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Lava"))
        {
            TakeDamage(1);
            inLava = true;

        }
        if(collision.collider.CompareTag("Spike"))
        {
            TakeDamage(1);
            ContactPoint2D contact = collision.contacts[0];
            Vector2 forceDirection = contact.normal;
            Debug.Log(forceDirection);
            gameObject.GetComponent<Rigidbody2D>().AddForce(forceDirection * 6f, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Lava"))
        {
            inLava = false;
        }
    }
  
}
