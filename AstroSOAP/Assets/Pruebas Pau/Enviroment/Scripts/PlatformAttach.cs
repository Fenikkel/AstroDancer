using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{

    private GameObject Player;
    

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player"); //Esta buscant soles un jugador
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("Entry");

            Player.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("Exit");
            Player.transform.parent = null;
        }
    }
}
