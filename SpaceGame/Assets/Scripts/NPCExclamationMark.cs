using System.Collections;
using UnityEngine;

public class NPCExclamationMark : MonoBehaviour
{
    public GameObject exclamationMarkPrefab; 
    public Vector3 offset = new Vector3(0, 1.8f, 0); 
    public float flashInterval = 0.5f; 
    public Vector3 fixedScale = new Vector3(0.45f, 0.45f, 0.45f);

    private GameObject exclamationMarkInstance;
    private bool isFlashing = false;

    void Start()
    {
        Debug.Log("NPCExclamationMark Start() called."); 
        if (exclamationMarkPrefab != null)
        {
       
            exclamationMarkInstance = Instantiate(exclamationMarkPrefab, transform.position + offset, Quaternion.identity);
            exclamationMarkInstance.transform.localScale = Vector3.one; 

            Debug.Log("Exclamation mark instantiated."); 
            exclamationMarkInstance.transform.localScale = fixedScale;

            exclamationMarkInstance.SetActive(true);
            StartCoroutine(FlashExclamationMark());
        }
        else
        {
            Debug.LogError("Exclamation mark prefab is not assigned!");
        }
    }


    private IEnumerator FlashExclamationMark()
    {
        isFlashing = true;
        while (isFlashing)
        {
            if (exclamationMarkInstance != null)
            {
               
                exclamationMarkInstance.SetActive(!exclamationMarkInstance.activeSelf);
            }
            yield return new WaitForSeconds(flashInterval);
        }
    }

    public void StopFlashing()
    {
     
        StopAllCoroutines();
        isFlashing = false;

      
        if (exclamationMarkInstance != null)
        {
            exclamationMarkInstance.SetActive(true); 
        }
    }



}
