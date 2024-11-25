using UnityEngine;

public class NPCFacing : MonoBehaviour
{
    public Transform player; 
    Vector3 currentScale;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
        currentScale = transform.localScale;
    }


    void Update()
    {
        
     
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-currentScale.x, currentScale.y, currentScale.z);
            
        }
    }
}
