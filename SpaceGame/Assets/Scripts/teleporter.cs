using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class teleporter : MonoBehaviour
{
    public GameObject myCharacter;
    public Transform mainPlanet;  
    public Transform planet1;  
    public Transform planet2; 
    private Camera myCamera;

  
    public GameObject teleportanimationPanel;
    public Image spaceshipImage;
    public RectTransform mainPlanetImage;
    public RectTransform planet1Image;
    public RectTransform planet2Image;
    public float animationDuration = 3f;



    public GameObject teleportPanel; 
    public Button goToPlanet1Button;
    public Button goToMainPlanetButton;
    public Button goToPlanet2Button;
    public GameObject planet2Text;


    public float interactionRadius = 5f; 

    private bool isTeleporting = false;
    private bool panelIsOpen = false; 
    private bool hasInteractedWithHat = false;


    private void Awake()
    {
        myCamera = Camera.main;

       
        teleportPanel.SetActive(false);

        goToPlanet2Button.gameObject.SetActive(false);
        planet2Image.gameObject.SetActive(false); 
        if (planet2Text != null) planet2Text.SetActive(false); 

        goToPlanet1Button.onClick.AddListener(() => StartCoroutine(TeleportWithAnimation(mainPlanetImage, planet1Image, 1)));
        goToMainPlanetButton.onClick.AddListener(() => StartCoroutine(TeleportWithAnimation(planet1Image, mainPlanetImage, 0)));
        goToPlanet2Button.onClick.AddListener(() => StartCoroutine(TeleportWithAnimation(mainPlanetImage, planet2Image, 2)));
    }



    public void UnlockPlanet2()
    {
        hasInteractedWithHat = true;

        goToPlanet2Button.gameObject.SetActive(true); 
        planet2Image.gameObject.SetActive(true); 
        if (planet2Text != null) planet2Text.SetActive(true); 
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, myCharacter.transform.position);

     
        if (panelIsOpen && distanceToPlayer > interactionRadius)
        {
            teleportPanel.SetActive(false);
            panelIsOpen = false; 
            return;
        }

        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
         
                teleportPanel.SetActive(true);
                panelIsOpen = true; 

                goToPlanet1Button.onClick.AddListener(() =>
                {
                    RectTransform startPlanetImage = GetCurrentPlanetImage();
                    StartCoroutine(TeleportWithAnimation(startPlanetImage, planet1Image, 1));
                });

                goToMainPlanetButton.onClick.AddListener(() =>
                {
                    RectTransform startPlanetImage = GetCurrentPlanetImage();
                    StartCoroutine(TeleportWithAnimation(startPlanetImage, mainPlanetImage, 0));
                });

                goToPlanet2Button.onClick.AddListener(() =>
                {
                    RectTransform startPlanetImage = GetCurrentPlanetImage();
                    StartCoroutine(TeleportWithAnimation(startPlanetImage, planet2Image, 2));
                });
            }
        }
    }

    private RectTransform GetCurrentPlanetImage()
    {
        if (CurrentPlanet == 0)
        {
            return mainPlanetImage;
        }
        else if (CurrentPlanet == 1)
        {
            return planet1Image;
        }
        else if (CurrentPlanet == 2)
        {
            return planet2Image;
        }

        return mainPlanetImage; 
    }


    private IEnumerator TeleportWithAnimation(RectTransform startPlanet, RectTransform targetPlanet, int targetPlanetIndex)
    {
        teleportanimationPanel.SetActive(true);
        spaceshipImage.rectTransform.anchoredPosition = startPlanet.anchoredPosition;

        spaceshipImage.rectTransform
            .DOAnchorPos(targetPlanet.anchoredPosition, animationDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                teleportanimationPanel.SetActive(false);
                CompleteTeleportation(targetPlanetIndex);
            });

        yield return new WaitForSeconds(animationDuration);
    }

    private void CompleteTeleportation(int targetPlanetIndex)
    {

        Transform targetPlanet = null;

        if (targetPlanetIndex == 0)
        {
            targetPlanet = mainPlanet;
            CurrentPlanet = 0;
            myCamera.GetComponent<cameraController>().planetnb = 0;
        }
        else if (targetPlanetIndex == 1)
        {
            targetPlanet = planet1;
            CurrentPlanet = 1;
            myCamera.GetComponent<cameraController>().planetnb = 1;
        }
        else if (targetPlanetIndex == 2)
        {
            targetPlanet = planet2;
            CurrentPlanet = 2;
            myCamera.GetComponent<cameraController>().planetnb = 2;
        }

        if (targetPlanet != null)
        {

            myCharacter.transform.position = new Vector3(targetPlanet.position.x, targetPlanet.position.y, 0);


            myCamera.transform.position = new Vector3(targetPlanet.position.x, targetPlanet.position.y, myCamera.transform.position.z);
        }

   
        teleportanimationPanel.SetActive(false);
    }


    public void SetCameraPlanet(int planetNumber)
    {
        myCamera.GetComponent<cameraController>().planetnb = planetNumber;
    }



    public int CurrentPlanet { get; private set; } = 0; 


    public void SetCurrentPlanet(int planetNumber)
    {
        CurrentPlanet = planetNumber;
    }



    private IEnumerator TeleportToPlanet1()
    {
        Debug.Log("Teleporting to Planet 1");
        myCamera.GetComponent<cameraController>().planetnb = 1;

        CurrentPlanet = 1;

        myCharacter.transform.position = new Vector3(planet1.position.x, planet1.position.y, 0);

        myCharacter.GetComponent<SpriteRenderer>().enabled = true;

        myCamera.transform.position = new Vector3(planet1.position.x, planet1.position.y, myCamera.transform.position.z);

        yield return new WaitForSeconds(0.1f);
        teleportPanel.SetActive(false);
        isTeleporting = true; 
        panelIsOpen = false; 
    }

    private IEnumerator TeleportToMainPlanet()
    {
        Debug.Log("Teleporting to Main Planet");
        myCamera.GetComponent<cameraController>().planetnb = 0;

        CurrentPlanet = 0;

  
        myCharacter.transform.position = new Vector3(mainPlanet.position.x, mainPlanet.position.y, 0);

        myCharacter.GetComponent<SpriteRenderer>().enabled = true;

        myCamera.transform.position = new Vector3(mainPlanet.position.x, mainPlanet.position.y, myCamera.transform.position.z);

        yield return new WaitForSeconds(0.1f);
        teleportPanel.SetActive(false);
        isTeleporting = true; 
        panelIsOpen = false; 
    }

    private IEnumerator TeleportToPlanet2()
    {
        Debug.Log("Teleporting to planet 2");
        myCamera.GetComponent<cameraController>().planetnb = 2;

        CurrentPlanet = 2;

   
        myCharacter.transform.position = new Vector3(planet2.position.x, planet2.position.y, 0);

    
        myCharacter.GetComponent<SpriteRenderer>().enabled = true;


        myCamera.transform.position = new Vector3(planet2.position.x, planet2.position.y, myCamera.transform.position.z);

        yield return new WaitForSeconds(0.1f);
        teleportPanel.SetActive(false);
        isTeleporting = true; 
        panelIsOpen = false;
    }

}
