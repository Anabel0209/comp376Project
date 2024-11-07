using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class teleporter : MonoBehaviour
{
    public GameObject myCharacter;
    public Transform mainPlanet; // Reference to the main planet's Transform
    public Transform planet1; // Reference to planet1's Transform
    private Camera myCamera;

    // UI Elements
    public GameObject teleportPanel; // Reference to the UI panel
    public Button goToPlanet1Button;
    public Button goToMainPlanetButton;



    private bool isTeleporting = false;


    private void Awake()
    {
        myCamera = Camera.main;

        // Hide the teleport panel initially
        teleportPanel.SetActive(false);

        // Add button listeners for teleporting
        goToPlanet1Button.onClick.AddListener(() => StartCoroutine(TeleportToPlanet1()));
        goToMainPlanetButton.onClick.AddListener(() => StartCoroutine(TeleportToMainPlanet()));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Show the teleport panel when a spaceship is clicked
                teleportPanel.SetActive(true);

                // Determine the appropriate button based on the spaceship's current position
                if (Vector2.Distance(transform.position, planet1.position) < 0.1f) // Close to Planet1
                {
                    goToPlanet1Button.gameObject.SetActive(false);
                    goToMainPlanetButton.gameObject.SetActive(true);
                }
                else if (Vector2.Distance(transform.position, mainPlanet.position) < 0.1f) // Close to Main Planet
                {
                    goToPlanet1Button.gameObject.SetActive(true);
                    goToMainPlanetButton.gameObject.SetActive(false);
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
    }

}
