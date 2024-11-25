using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BabyGolemFollow : MonoBehaviour
{
    public GameObject attachededTo;
    public GameObject newMamaGolem;
    public GameObject newPapaGolem;
    public GameObject mamaGolem;
    public GameObject papaGolem;
    public GameObject mayor;
    public GameObject newMayor;
    public GameObject yellowTurnip;
    public GameObject newYellowTurnip;

    private Vector3 offsetOnThePlayerLookingRight = new Vector3(-1, 1, 0);
    private Vector3 offsetOnThePlayerLookingLeft = new Vector3(1, 1, 0);
    private Vector3 offsetFromMama = new Vector3(1.0f, 1.25f, 0.0f);
    private bool isAttached = false;
    private float smoothSpeed = 6f;
    private Vector3 currentOffset;
    private bool hasInteracted = false;
    private float distanceFromMama;
    public AudioSource baby;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //calculate the distance from mama
        distanceFromMama = Vector3.Distance(gameObject.transform.position, newMamaGolem.transform.position);

        //if the distance is small enough baby goes to mama
        if(distanceFromMama < 7.0f)
        {
            isAttached = false;
            transform.position = Vector3.MoveTowards(transform.position, newMamaGolem.transform.position + offsetFromMama, (smoothSpeed - 1f) * Time.deltaTime);

        }

        if (isAttached)
        {
            // Determine the player's direction and update the offset once
            if (attachededTo.transform.localScale.x > 0) // Looking right
            {
                currentOffset = offsetOnThePlayerLookingRight;
            }
            else // Looking left
            {
                currentOffset = offsetOnThePlayerLookingLeft;
            }

            // Smoothly follow the player with the current offset
            Vector3 targetPosition = attachededTo.transform.position + currentOffset;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }

    }

    private void OnMouseDown()
    {
        if (hasInteracted) return; // Prevent multiple interactions
        hasInteracted = true;

        baby.Play();

        //player dialogue when finding baby golem
        DialogueManager.instance.StartDialogue(
         new string[0], // No NPC lines
         0.05f,         // Text speed
         new string[] { "This must be baby Golem!", "I will bring him back to his parents." }, // Player's line
         "Turnip hat");

        isAttached = true;

        gameObject.GetComponent<Collider2D>().enabled = false;

        //Setup the new NPC
        if (mamaGolem != null) mamaGolem.SetActive(false);
        if (papaGolem != null) papaGolem.SetActive(false);
        if (newMamaGolem != null) newMamaGolem.SetActive(true);
        if(newPapaGolem != null) newPapaGolem.SetActive(true);
        if (mayor != null) mayor.SetActive(false);
        if(newMayor != null) newMayor.SetActive(true);
        if(yellowTurnip != null) yellowTurnip.SetActive(false);
        if(newYellowTurnip != null) newYellowTurnip.SetActive(true);

    }
}
