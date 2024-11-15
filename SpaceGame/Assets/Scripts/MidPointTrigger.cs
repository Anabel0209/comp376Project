using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GraphicRaycaster;

public class MidPointTrigger : MonoBehaviour
{
    int amountOfItem;
    public GameObject player;
    public int priceToContinue;
    public GameObject blockingObject;

    void Update()
    {
        amountOfItem = player.gameObject.GetComponent<ItemCollectionManager>().count;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (amountOfItem >= priceToContinue)
            {
                Debug.Log("in trigger");
                
                //decrement the nb of items
                player.gameObject.GetComponent<ItemCollectionManager>().DecrementCount(priceToContinue);

                //play the animation
                blockingObject.GetComponent<Animator>().SetTrigger("dissapear");

                //disable the collider
                blockingObject.GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
