using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{

    public float cameraSpeed = 2.0f;
    public Transform target;
    public float yOffset = -2;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = new Vector3(target.position.x, target.position.y + yOffset, -10.0f);
        transform.position = Vector3.Slerp(transform.position, newPosition, cameraSpeed * Time.deltaTime);
    }
}
