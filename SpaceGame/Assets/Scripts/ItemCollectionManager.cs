using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemCollectionManager : MonoBehaviour
{
    public TMP_Text counterItems;
    public int goal;
    public int count;
    public Sprite itemToCollect;
    public bool shouldCollectItem = true;
    public GameObject itemCollectionDisplay;
    public GameObject inGameImageOfItem;
    private bool hasReachedGoal;



    private void Update()
    {
        inGameImageOfItem.GetComponent<Image>().sprite = itemToCollect;
        if (shouldCollectItem)
        {
            counterItems.SetText(count.ToString());
            itemCollectionDisplay.SetActive(true);
            if (goal == count)
            {
                hasReachedGoal = true;
                //itemCollectionDisplay.SetActive(false);
                //goal = 0;
                //count = 0;
            }
        }
        else
        {
            itemCollectionDisplay.SetActive(false);
        }
    }
    public void DecrementCount(int amountToDecrement)
    {
        if (count >= amountToDecrement)
        {
            count = count - amountToDecrement;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {
            count++;
            Destroy(collision.gameObject);

        }
    }
}