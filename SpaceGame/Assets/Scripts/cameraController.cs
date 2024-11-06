using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public float planetnb = 0;
    public Transform target;
    public float yOffset = -2;

    //camera boundaries
    public Transform topLeftMainPlanet, bottomRightMainPlanet;
    public Transform topLeftPlanet1, bottomRightPlanet1;

    //camera size
    private float camHalfLength;
    private float camHalfHeight;

    Camera myCamera;
    

    // Start is called before the first frame update
    void Start()
    {
        myCamera = gameObject.GetComponent<Camera>();
        
        //camHalfHeight = myCamera.orthographicSize;
        //camHalfLength = myCamera.aspect * camHalfHeight;
    }

    // Update is called once per frame
    void Update()
    {
        camHalfHeight = myCamera.orthographicSize;
        camHalfLength = myCamera.aspect * camHalfHeight;
        //camera position
        float horizontalPosition, verticalPosition;

        if (planetnb == 0)
        {
            //horizontal and vertical clamping
            horizontalPosition = Mathf.Clamp(target.position.x, topLeftMainPlanet.position.x + camHalfLength, bottomRightMainPlanet.position.x - camHalfLength);
            verticalPosition = Mathf.Clamp(target.position.y, bottomRightMainPlanet.position.y + camHalfHeight, topLeftMainPlanet.position.y - camHalfHeight);

            transform.position = new Vector3(horizontalPosition, verticalPosition, transform.position.z);
        }
        if(planetnb == 1)
        {
            //horizontal and vertical clamping
            horizontalPosition = Mathf.Clamp(target.position.x, topLeftPlanet1.position.x + camHalfLength, bottomRightPlanet1.position.x - camHalfLength);
            verticalPosition = Mathf.Clamp(target.position.y, bottomRightPlanet1.position.y + camHalfHeight, topLeftPlanet1.position.y - camHalfHeight);

            transform.position = new Vector3(horizontalPosition, verticalPosition, transform.position.z);
        }



    }
}
