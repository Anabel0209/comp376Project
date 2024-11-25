using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public GameObject EndGameMenu;

    public void OnMouseDown()
    {
        EndGameMenu.SetActive(true);
    }

}
