using UnityEngine;

public class PresidentDialogue : MonoBehaviour
{
    public string[] lines; // Lines for the President's dialogue
    public float textSpeed; // Speed of typing effect
    public string npcName = "President"; // Name of the President
    public string[] playerLines; // Lines for the player response (if any)
    private bool hasStartedDialogue = false;


    private void Start()
    {
        // Optionally start the dialogue automatically
        if (PresidentDialogueManager.instance != null)
        {
            hasStartedDialogue = true;
            PresidentDialogueManager.instance.StartDialogue(lines, textSpeed, playerLines, npcName);
        }
    }
}
