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
    public GameObject darkTurnip;

    private Vector3 offsetOnThePlayerLookingRight = new Vector3(-1, 1, 0);
    private Vector3 offsetOnThePlayerLookingLeft = new Vector3(1, 1, 0);
    private Vector3 offsetFromMama = new Vector3(1.0f, 1.25f, 0.0f);
    private bool isAttached = false;
    private float smoothSpeed = 6f;
    private Vector3 currentOffset;
    private bool hasInteracted = false;
    private float distanceFromMama;
    public AudioSource baby;


    void Start()
    {
        
    }


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
            
            if (attachededTo.transform.localScale.x > 0) 
            {
                currentOffset = offsetOnThePlayerLookingRight;
            }
            else 
            {
                currentOffset = offsetOnThePlayerLookingLeft;
            }

          
            Vector3 targetPosition = attachededTo.transform.position + currentOffset;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }

    }

    private void OnMouseDown()
    {
        if (hasInteracted) return; 
        hasInteracted = true;

        baby.Play();

        //player dialogue when finding baby golem
        DialogueManager.instance.StartDialogue(
         new string[0], 
         0.05f,         
         new string[] { "Baby Golem!", "Let's get you home." }, 
         "Turnip hat");

        isAttached = true;

        gameObject.GetComponent<Collider2D>().enabled = false;

        //setup the new NPCs
        if (mamaGolem != null) mamaGolem.SetActive(false);
        if (papaGolem != null) papaGolem.SetActive(false);
        if (newMamaGolem != null) newMamaGolem.SetActive(true);
        if(newPapaGolem != null) newPapaGolem.SetActive(true);
        if (mayor != null) mayor.SetActive(false);
        if(newMayor != null) newMayor.SetActive(true);
        if(yellowTurnip != null) yellowTurnip.SetActive(false);
        if(newYellowTurnip != null) newYellowTurnip.SetActive(true);
        if (darkTurnip != null) darkTurnip.SetActive(false);

    }
}
