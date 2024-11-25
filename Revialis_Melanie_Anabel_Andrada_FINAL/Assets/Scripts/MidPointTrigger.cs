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
    public GameObject behaviourOnObject;
    public GameObject animationOn;
    public int planetNb;

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
                //player.gameObject.GetComponent<ItemCollectionManager>().DecrementCount(priceToContinue);

                if(planetNb == 1)
                {
                    //play the animation
                    animationOn.GetComponent<Animator>().SetTrigger("dissapear");

                    //disable the collider
                    behaviourOnObject.GetComponent<Collider2D>().enabled = false;
                }
                if(planetNb == 2)
                {
                    //play the animation
                    animationOn.GetComponent<Animator>().SetTrigger("makeBridgeAppear");

                    //ennable the collider
                    behaviourOnObject.GetComponent<Collider2D>().enabled = true;
                }

            }
        }
    }
}
