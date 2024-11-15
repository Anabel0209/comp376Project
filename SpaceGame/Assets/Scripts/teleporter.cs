using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class teleporter : MonoBehaviour
{
    public GameObject myCharacter;
    public Transform mainPlanet; // Reference to the main planet's Transform
    public Transform planet1; // Reference to planet1's Transform
    public Transform planet2; //Reference to planet 2's Transform
    private Camera myCamera;

    // UI Elements
    public GameObject teleportPanel; // Reference to the UI panel
    public Button goToPlanet1Button;
    public Button goToMainPlanetButton;
    public Button goToPlanet2Button;

    public float interactionRadius = 5f; // Radius within which the player can interact

    private bool isTeleporting = false;
    private bool panelIsOpen = false; // Add this line to declare panelIsOpen


    private void Awake()
    {
        myCamera = Camera.main;

        // Hide the teleport panel initially
        teleportPanel.SetActive(false);

        // Add button listeners for teleporting
        goToPlanet1Button.onClick.AddListener(() => StartCoroutine(TeleportToPlanet1()));
        goToMainPlanetButton.onClick.AddListener(() => StartCoroutine(TeleportToMainPlanet()));
        goToPlanet2Button.onClick.AddListener(() => StartCoroutine(TeleportToPlanet2()));
    }

    void Update()
    {
        // Check distance between player and spaceship
        float distanceToPlayer = Vector2.Distance(transform.position, myCharacter.transform.position);

        // If panel is open, only hide it when the player moves out of range
        if (panelIsOpen && distanceToPlayer > interactionRadius)
        {
            teleportPanel.SetActive(false);
            panelIsOpen = false; // Reset the flag
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Show the teleport panel when a spaceship is clicked
                teleportPanel.SetActive(true);
                panelIsOpen = true; // Set the flag

                // Determine the appropriate button based on the spaceship's current position
                if (Vector2.Distance(transform.position, planet1.position) < 0.1f) // Close to Planet1
                {
                    goToPlanet1Button.gameObject.SetActive(false);
                    goToMainPlanetButton.gameObject.SetActive(true);
                    goToPlanet2Button.gameObject.SetActive(false);
                }
                else if (Vector2.Distance(transform.position, mainPlanet.position) < 0.1f) // Close to Main Planet
                {
                    goToPlanet1Button.gameObject.SetActive(true);
                    goToMainPlanetButton.gameObject.SetActive(false);
                    goToPlanet2Button.gameObject.SetActive(false);
                }
                else if (Vector2.Distance(transform.position, planet2.position) < 0.1f) // Close to Planet2
                {
                    goToPlanet1Button.gameObject.SetActive(false);
                    goToMainPlanetButton.gameObject.SetActive(false);
                    goToPlanet2Button.gameObject.SetActive(true);
                }
            }
        }
    }

    public void SetCameraPlanet(int planetNumber)
    {
        myCamera.GetComponent<cameraController>().planetnb = planetNumber;
    }



    public int CurrentPlanet { get; private set; } = 0; // 0 for Main Planet, 1 for Planet1


    public void SetCurrentPlanet(int planetNumber)
    {
        CurrentPlanet = planetNumber;
    }



    private IEnumerator TeleportToPlanet1()
    {
        Debug.Log("Teleporting to Planet 1");
        myCamera.GetComponent<cameraController>().planetnb = 1;

        CurrentPlanet = 1;

        // Set the character's position with Z set to 0
        myCharacter.transform.position = new Vector3(planet1.position.x, planet1.position.y, 0);

        // Ensure Sprite Renderer is enabled
        myCharacter.GetComponent<SpriteRenderer>().enabled = true;

        // Adjust camera position if necessary
        myCamera.transform.position = new Vector3(planet1.position.x, planet1.position.y, myCamera.transform.position.z);

        yield return new WaitForSeconds(0.1f);
        teleportPanel.SetActive(false);
        isTeleporting = true; // Set teleporting state to true
        panelIsOpen = false; // Reset the panel state
    }

    private IEnumerator TeleportToMainPlanet()
    {
        Debug.Log("Teleporting to Main Planet");
        myCamera.GetComponent<cameraController>().planetnb = 0;

        CurrentPlanet = 0;

        // Set the character's position with Z set to 0
        myCharacter.transform.position = new Vector3(mainPlanet.position.x, mainPlanet.position.y, 0);

        // Ensure Sprite Renderer is enabled
        myCharacter.GetComponent<SpriteRenderer>().enabled = true;

        // Adjust camera position if necessary
        myCamera.transform.position = new Vector3(mainPlanet.position.x, mainPlanet.position.y, myCamera.transform.position.z);

        yield return new WaitForSeconds(0.1f);
        teleportPanel.SetActive(false);
        isTeleporting = true; // Set teleporting state to true
        panelIsOpen = false; // Reset the panel state
    }

    private IEnumerator TeleportToPlanet2()
    {
        Debug.Log("Teleporting to planet 2");
        myCamera.GetComponent<cameraController>().planetnb = 2;

        CurrentPlanet = 2;

        // Set the character's position with Z set to 0
        myCharacter.transform.position = new Vector3(planet2.position.x, planet2.position.y, 0);

        // Ensure Sprite Renderer is enabled
        myCharacter.GetComponent<SpriteRenderer>().enabled = true;

        // Adjust camera position if necessary
        myCamera.transform.position = new Vector3(planet2.position.x, planet2.position.y, myCamera.transform.position.z);

        yield return new WaitForSeconds(0.1f);
        teleportPanel.SetActive(false);
        isTeleporting = true; // Set teleporting state to true
        panelIsOpen = false; // Reset the panel state
    }

}
