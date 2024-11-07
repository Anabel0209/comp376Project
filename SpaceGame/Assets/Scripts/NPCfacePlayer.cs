using UnityEngine;

public class NPCFacing : MonoBehaviour
{
    public Transform player; // Assign the player's transform here
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
        
        // Check if player is to the left or right of the NPC
        if (player.position.x > transform.position.x)
        {
            // Player is to the right, face right
            transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);
        }
        else
        {
            // Player is to the left, face left
            transform.localScale = new Vector3(-currentScale.x, currentScale.y, currentScale.z);
            
        }
    }
}
