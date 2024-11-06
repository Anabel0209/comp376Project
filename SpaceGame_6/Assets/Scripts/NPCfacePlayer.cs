using UnityEngine;

public class NPCFacing : MonoBehaviour
{
    public Transform player; // Assign the player's transform here

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }


    void Update()
    {
        // Check if player is to the left or right of the NPC
        if (player.position.x > transform.position.x)
        {
            // Player is to the right, face right
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            // Player is to the left, face left
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
