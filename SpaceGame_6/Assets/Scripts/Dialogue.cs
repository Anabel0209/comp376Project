using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public string[] lines;
    public float textSpeed;
    public GameObject mayorImage;
    public GameObject turnipImage;
    public string npcName; // New field for NPC name
    public string[] playerLines; // New field for player lines

    private void OnMouseDown()
    {
        // Assign default names based on GameObject name if not set
        if (string.IsNullOrEmpty(npcName))
        {
            if (gameObject.name == "Mayor")
            {
                npcName = "Mayor";
            }
            else if (gameObject.name == "Villager")
            {
                npcName = "Villager";
            }
            else
            {
                npcName = "Unknown NPC"; // Fallback name
            }
        }

        ShowRelevantImage();
        DialogueManager.instance.StartDialogue(lines, textSpeed, playerLines, npcName);
    }

    void ShowRelevantImage()
    {
        // Hide all images first
        if (mayorImage != null) mayorImage.SetActive(false);
        if (turnipImage != null) turnipImage.SetActive(false);

        // Show the specific image for the NPC clicked
        if (gameObject.name == "Mayor" && mayorImage != null)
        {
            mayorImage.SetActive(true);
        }
        else if (gameObject.name == "Villager" && turnipImage != null)
        {
            turnipImage.SetActive(true);
        }
    }

}
