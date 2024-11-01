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
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            Debug.Log("mouse pressed");

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("clicked on" + hit.collider.gameObject.name);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.CompareTag("Spaceship"))
                    {
                        Debug.Log("clicked on spaceship");
                        myCharacter.transform.position = new Vector2(destination.position.x, destination.position.y);
                    }
                }
            }
        }
    }
}
