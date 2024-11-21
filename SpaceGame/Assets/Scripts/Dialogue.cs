using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public string[] lines;
    public float textSpeed;
    public GameObject mayorImage;
    public GameObject turnipImage;
    public GameObject presidentImage;
    public GameObject pigImage;
    public GameObject mamaImage;
    public GameObject papaImage;
    public GameObject npcDialoguePanel; // Reference to NPC dialogue panel
    public string npcName; // New field for NPC name
    public string[] playerLines; // New field for player lines

    private float lastInputTime = 0f;
    private float inputCooldown = 2f;

    private bool hasDialogueStarted = false;

    private void Start()
    {
        // Automatically start dialogue if this is the President
        if (gameObject.name == "President")
        {
            // Set the NPC name
            npcName = "President";
            hasDialogueStarted = true;
            DialogueManager.instance.StartDialogue(lines, textSpeed, playerLines, npcName);
        }
    }

    private void OnMouseDown()
    {
        if (DialogueManager.instance == null || DialogueManager.instance.IsInputLocked()) return;

        if (Time.time - lastInputTime < inputCooldown) return;
        lastInputTime = Time.time;

        // Special handling for "Turnip Hat"
        if (gameObject.name == "Turnip hat")
        {
            hasDialogueStarted = true;
            DialogueManager.instance.StartDialogue(lines, textSpeed, playerLines, npcName);
            return; // Exit after handling "Turnip Hat"
        }

        // Regular dialogue behavior for other NPCs
        if (string.IsNullOrEmpty(npcName))
        {
            AssignDefaultNPCName();
        }

        ShowRelevantImage();
        hasDialogueStarted = true;
        DialogueManager.instance.StartDialogue(lines, textSpeed, playerLines, npcName);
    }

    void ShowRelevantImage()
    {
        // Hide all images first
        if (mayorImage != null) mayorImage.SetActive(false);
        if (turnipImage != null) turnipImage.SetActive(false);
        if (presidentImage != null) presidentImage.SetActive(false);
        if (pigImage != null) pigImage.SetActive(false);

        // Show the specific image for the NPC clicked
        if (gameObject.name == "Mayor" && mayorImage != null || gameObject.name == "Mayor (1)" && mayorImage != null)
        {
            mayorImage.SetActive(true);
        }
        else if (gameObject.name == "Villager" && turnipImage != null || gameObject.name == "Villager (1)" && turnipImage != null)
        {
            turnipImage.SetActive(true);
        }
        else if (gameObject.name == "President" && presidentImage != null)
        {
            presidentImage.SetActive(true);
        }
        else if (gameObject.name == "Pig" && pigImage != null || gameObject.name == "Pig (1)" && pigImage != null)
        {
            pigImage.SetActive(true);
        }
        else if (gameObject.name == "MamaGolem" && mamaImage != null || gameObject.name == "MamaGolem (1)" && mamaImage != null)
        {
            mamaImage.SetActive(true);
        }

        else if (gameObject.name == "PapaGolem" && mamaImage != null || gameObject.name == "PapaGolem (1)" && papaImage != null)
        {
            papaImage.SetActive(true);
        }

    }

    void AssignDefaultNPCName()
    {
        if (gameObject.name == "Mayor")
        {
            npcName = "Mayor";
        }
        else if (gameObject.name == "Villager")
        {
            npcName = "Turnip";
        }
        else if (gameObject.name == "Pig")
        {
            npcName = "Piggy";
        }
        else if (gameObject.name == "MamaGolem")
        {
            npcName = "Mama Golem";
        }

        else if (gameObject.name == "PapaGolem")
        {
            npcName = "Papa Golem";
        }
        else if (gameObject.name == "Turnip hat")
        {
            npcName = "hat";
        }
        else
        {
            npcName = "Unknown NPC"; // Fallback name
        }
    }
}
