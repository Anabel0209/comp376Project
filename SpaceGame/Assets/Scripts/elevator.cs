using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class elevator : MonoBehaviour
{
    //controle the speed of the platform
    public float speedX;
    public float speedY;

    //control the scope of the movement
    public float verticalDisplacement;
    public float horizontalDisplacement;

    public float timeStoppedAtTop = 0f;
    public float timeStoppedAtBottom = 0f;
    public float timeStoppedAtLeft = 0f;
    public float timeStoppedAtRight = 0f;

    public float delayBeforeStart = 0f;

    //limits of the movement
    private float horizontalLimitLeft;
    private float horizontalLimitRight;

    private float verticalLimitTop;
    private float verticalLimitBottom;

    private bool isMoving = true;


    void Start()
    {
        //initialize the limits of the movement
        horizontalLimitLeft = transform.position.x - horizontalDisplacement;
        horizontalLimitRight = transform.position.x + horizontalDisplacement;
        verticalLimitBottom = transform.position.y - verticalDisplacement;
        verticalLimitTop = transform.position.y + verticalDisplacement;

        //wait a certain amount of time before starting the movement
        StartCoroutine(waitBeforeStarting());
    }


    void Update()
    {
        if(isMoving)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        //change the direction of the moving objects if it reaches a limit
        if (transform.position.x >= horizontalLimitRight)
        {
            transform.position = new Vector2(horizontalLimitRight, transform.position.y);
            StartCoroutine(stopMovementAtRight());
            
            speedX = -speedX;
        }
        if (transform.position.x <= horizontalLimitLeft)
        {
            transform.position = new Vector2(horizontalLimitLeft, transform.position.y);
            StartCoroutine(stopMovementAtLeft());
            speedX = -speedX;
        }
        if (transform.position.y >= verticalLimitTop)
        {
            transform.position = new Vector2(transform.position.y, verticalLimitTop);
            StartCoroutine(stopMovementAtTop());
            speedY = -speedY;
        }
        if (transform.position.y <= verticalLimitBottom)
        {
            transform.position = new Vector2(transform.position.y, verticalLimitBottom);
            StartCoroutine(stopMovementAtBottom());
            speedY = -speedY;
        }

        //update the position of the object
        Vector2 movement = new Vector2(transform.position.x + speedX * Time.deltaTime, transform.position.y + speedY * Time.deltaTime);
        transform.position = movement;
    }

    private IEnumerator waitBeforeStarting()
    {
        isMoving = false;
        yield return new WaitForSeconds(delayBeforeStart);
        isMoving = true;
    }
    private IEnumerator stopMovementAtTop()
    {
        isMoving = false;
        yield return new WaitForSeconds(timeStoppedAtTop);
        isMoving = true;
    }
    private IEnumerator stopMovementAtBottom()
    {
        isMoving = false;
        yield return new WaitForSeconds(timeStoppedAtBottom);
        isMoving = true;
    }
    private IEnumerator stopMovementAtRight()
    {
        isMoving = false;
        yield return new WaitForSeconds(timeStoppedAtRight);
        isMoving = true;
    }
    private IEnumerator stopMovementAtLeft()
    {
        isMoving = false;
        yield return new WaitForSeconds(timeStoppedAtLeft);
        isMoving = true;
    }
}
