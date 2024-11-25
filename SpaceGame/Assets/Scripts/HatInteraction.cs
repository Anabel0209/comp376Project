using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatInteraction : MonoBehaviour
{
    public GameObject turnip;
    public GameObject mayor;
    public GameObject piggy;
    public GameObject hat;

    public GameObject newTurnip;
    public GameObject newMayor;
    public GameObject newPiggy;
    public GameObject newHat;

    public GameObject npcDialoguePanel;

    private bool hasInteracted = false;

    private void OnMouseDown()
    {
        if (hasInteracted) return; 
        hasInteracted = true;

        DialogueManager.instance.StartDialogue(
         new string[0], 
         0.05f,         
         new string[] { "This must be Turnip's hat!", "I should get this back to him." }, 
         "Turnip hat");
    


     StartCoroutine(HandleHatInteraction());

     


    }

    private IEnumerator HandleHatInteraction()
    {
        
        while (DialogueManager.instance.IsInputLocked() || DialogueManager.instance.IsDialogueActive())
        {
            yield return null;
        }

      
        if (hat != null) hat.SetActive(false);

      
        if (turnip != null) turnip.SetActive(false);
        if (mayor != null) mayor.SetActive(false);
        if (piggy != null) piggy.SetActive(false);

 
        if (newTurnip != null) newTurnip.SetActive(true);
        if (newMayor != null) newMayor.SetActive(true);
        if (newPiggy != null) newPiggy.SetActive(true);
        if (newHat != null) newHat.SetActive(true);

        teleporter teleportScript = FindObjectOfType<teleporter>();
        if (teleportScript != null)
        {
            teleportScript.UnlockPlanet2();
        }
    }

}




