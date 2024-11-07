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
                Destroy(itemToDestroyToOpenPath);
                hasBeenDestroyed = true;
                player.gameObject.GetComponent<ItemCollectionManager>().DecrementCount(priceToContinue);
            }

        }
    }
}