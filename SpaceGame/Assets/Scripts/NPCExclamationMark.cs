using System.Collections;
using UnityEngine;

public class NPCExclamationMark : MonoBehaviour
{
    public GameObject exclamationMarkPrefab; // Prefab for the exclamation mark
    public Vector3 offset = new Vector3(0, 1.8f, 0); // Position offset above the NPC
    public float flashInterval = 0.5f; // Interval for flashing the exclamation mark
    public Vector3 fixedScale = new Vector3(0.45f, 0.45f, 0.45f);

    private GameObject exclamationMarkInstance;
    private bool isFlashing = false;

    void Start()
    {
        Debug.Log("NPCExclamationMark Start() called."); // Debugging
        if (exclamationMarkPrefab != null)
        {
            // Instantiate the exclamation mark and place it above the NPC
            exclamationMarkInstance = Instantiate(exclamationMarkPrefab, transform.position + offset, Quaternion.identity);
            exclamationMarkInstance.transform.localScale = Vector3.one; // Reset its scale to default

            Debug.Log("Exclamation mark instantiated."); // Debugging

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
                // Toggle visibility
                exclamationMarkInstance.SetActive(!exclamationMarkInstance.activeSelf);
            }
            yield return new WaitForSeconds(flashInterval);
        }
    }

    public void StopFlashing()
    {
        // Stop the flashing coroutine
        StopAllCoroutines();
        isFlashing = false;

        // Ensure the exclamation mark is visible
        if (exclamationMarkInstance != null)
        {
            exclamationMarkInstance.SetActive(true); // Keep the exclamation mark active
        }
    }



}
