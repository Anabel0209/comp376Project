using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public string[] lines;
    public float textSpeed;
    public GameObject mayorImage;
    public GameObject turnipImage;
    public GameObject presidentImage;
    public GameObject pigImage;
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

        // Only trigger on mouse down for non-President NPCs
        if (gameObject.name != "President")
        {

            // Check if input is locked; if so, do nothing
            if (DialogueManager.instance != null && DialogueManager.instance.IsInputLocked()) return;


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
                else if (gameObject.name == "Pig")
                {
                    npcName = "Piggy";
                }
                else
                {
                    npcName = "Unknown NPC"; // Fallback name
                }
            }

            ShowRelevantImage();
            hasDialogueStarted = true;
            DialogueManager.instance.StartDialogue(lines, textSpeed, playerLines, npcName);
        }
    }

    void ShowRelevantImage()
    {
        // Hide all images first
        if (mayorImage != null) mayorImage.SetActive(false);
        if (turnipImage != null) turnipImage.SetActive(false);
        if (presidentImage != null) presidentImage.SetActive(false);
        if (pigImage != null) pigImage.SetActive(false);

        // Show the specific image for the NPC clicked
        if (gameObject.name == "Mayor" && mayorImage != null)
        {
            mayorImage.SetActive(true);
        }
        else if (gameObject.name == "Villager" && turnipImage != null)
        {
            turnipImage.SetActive(true);
        }
        else if (gameObject.name == "President" && presidentImage != null)
        {
            presidentImage.SetActive(true);
        }
        else if (gameObject.name == "Pig" && pigImage != null)
        {
            pigImage.SetActive(true);
        }
    }

}
