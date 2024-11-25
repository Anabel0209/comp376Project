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
    public GameObject yellowTurnipImage;
    public GameObject npcDialoguePanel; 
    public string npcName; 
    public string[] playerLines; 
    public AudioSource npcAudioSource; 
    public GameObject gem; 
    public BoxCollider2D billboardCollider;


    private float lastInputTime = 0f;
    private float inputCooldown = 2f;

    private bool hasDialogueStarted = false;

    private void Start()
    {
      
        if (gameObject.name == "President")
        {
            npcName = "President";
            hasDialogueStarted = true;
            DialogueManager.instance.StartDialogue(lines, textSpeed, playerLines, npcName, npcAudioSource);
        }
    }

    private void OnMouseDown()
    {

        if (gameObject.name == "Mayor_fix")
        {
            GameObject mayor = GameObject.Find("Mayor_hide");
            if (mayor != null) 
            {
                mayor.SetActive(false); 
            }
        }

        if (gameObject.name == "Mayor_end" && gem != null)
        {
            gem.SetActive(true);
        }

        if (gameObject.name == "Mayor_endGame")
        {
            EnableBillboardCollider();
        }

        if (gameObject.name == "YellowTurnip")
        {
            
            FindObjectOfType<HealthManagement>().SetCheckpoint(transform.position);
        }


        if (DialogueManager.instance == null || DialogueManager.instance.IsInputLocked()) return;

        if (Time.time - lastInputTime < inputCooldown) return;
        lastInputTime = Time.time;

       
        if (gameObject.name == "Turnip hat")
        {
            hasDialogueStarted = true;
            DialogueManager.instance.StartDialogue(lines, textSpeed, playerLines, npcName, npcAudioSource);
            return; 
        }

      
        if (string.IsNullOrEmpty(npcName))
        {
            AssignDefaultNPCName();
        }

        ShowRelevantImage();
        hasDialogueStarted = true;
        DialogueManager.instance.StartDialogue(lines, textSpeed, playerLines, npcName, npcAudioSource);
    }

    void ShowRelevantImage()
    {
     
        if (mayorImage != null) mayorImage.SetActive(false);
        if (turnipImage != null) turnipImage.SetActive(false);
        if (presidentImage != null) presidentImage.SetActive(false);
        if (pigImage != null) pigImage.SetActive(false);
        if (mamaImage != null) mamaImage.SetActive(false);
        if (papaImage != null) papaImage.SetActive(false);
        if (yellowTurnipImage != null) yellowTurnipImage.SetActive(false);

        
        if (gameObject.name == "Mayor" && mayorImage != null || gameObject.name == "Mayor_end" && mayorImage != null || gameObject.name == "Mayor_fix" && mayorImage != null || gameObject.name == "Mayor_hide" && mayorImage != null || gameObject.name == "Mayor_endGame" && mayorImage != null)
        {
            mayorImage.SetActive(true);
        }
        else if (gameObject.name == "YellowTurnip" && yellowTurnipImage != null || gameObject.name == "YellowTurnip (1)" && yellowTurnipImage != null || gameObject.name == "YellowTurnip (2)" && yellowTurnipImage != null)
        {
            yellowTurnipImage.SetActive(true);
        }
        else if (gameObject.name == "Villager" && turnipImage != null || gameObject.name == "Villager (1)" && turnipImage != null)
        {
            turnipImage.SetActive(true);
        }
        else if (gameObject.name == "President" && presidentImage != null )
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

        else if (gameObject.name == "PapaGolem" && papaImage != null || gameObject.name == "PapaGolem (1)" && papaImage != null)
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
        else if (gameObject.name == "YellowTurnip")
        {
            npcName = "Chomp";
        }
        else
        {
            npcName = "Unknown NPC"; 
        }

      
    }

    private void EnableBillboardCollider()
    {
        if (billboardCollider != null)
        {
            billboardCollider.enabled = true;
        }
        else
        {
            Debug.LogWarning("Billboard BoxCollider2D is not assigned.");
        }
    }
}
