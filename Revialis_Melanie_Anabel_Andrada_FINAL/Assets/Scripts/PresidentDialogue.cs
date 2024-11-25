using UnityEngine;

public class PresidentDialogue : MonoBehaviour
{
    public string[] lines; 
    public float textSpeed; 
    public string npcName = "President"; 
    public string[] playerLines; 
    private bool hasStartedDialogue = false;


    private void Start()
    {
        if (PresidentDialogueManager.instance != null)
        {
            hasStartedDialogue = true;
            PresidentDialogueManager.instance.StartDialogue(lines, textSpeed, playerLines, npcName);
        }
    }
}
