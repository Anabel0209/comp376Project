using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporter : MonoBehaviour
{
    public GameObject myCharacter;

    public Transform destination;
    Camera myCamera;

    private void Awake()
    {
        myCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);


            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (destination.gameObject.CompareTag("Planet1"))
                {
                    myCamera.GetComponent<cameraController>().planetnb = 1;
                }
                else if (destination.gameObject.CompareTag("MainPlanet"))
                {
                    myCamera.GetComponent<cameraController>().planetnb = 0;
                }

                myCharacter.transform.position = new Vector2(destination.position.x, destination.position.y);

            }
            
        }
    }
}
