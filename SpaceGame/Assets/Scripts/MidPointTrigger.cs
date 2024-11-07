using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MidPointTrigger : MonoBehaviour
{
    int amountOfItem;
    public GameObject player;
    public GameObject itemToDestroyToOpenPath;
    private bool hasBeenDestroyed = false;
    public int priceToContinue;

    public float pushForce = 15f; // Force applied to the boxes

    public GameObject[] itemsToMove; // Array to hold the boxes


    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        amountOfItem = player.gameObject.GetComponent<ItemCollectionManager>().count;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(amountOfItem + "   " + hasBeenDestroyed);

        if (collision.CompareTag("Player"))
        {
            if (amountOfItem >= 7 && hasBeenDestroyed == false)
            {
                Debug.Log("in trigger");
                
                hasBeenDestroyed = true;
                player.gameObject.GetComponent<ItemCollectionManager>().DecrementCount(priceToContinue);
                PushItemsAway();
            }

        }
    }

    private void PushItemsAway()
    {
        foreach (GameObject item in itemsToMove)
        {
            if (item != null)
            {
                Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 randomDirection = Random.insideUnitCircle.normalized; // Random direction
                    rb.bodyType = RigidbodyType2D.Dynamic; // Ensure it is dynamic
                    rb.AddForce(randomDirection * pushForce, ForceMode2D.Impulse);
                }
            }
        }
    }

}
