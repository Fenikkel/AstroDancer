using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowerIA : MonoBehaviour
{

    private NavMeshAgent m_navMeshAgent; //NAvMeshAgent del enemigo
    private Transform m_player; //Transform del player

    public bool showPath;
    public LineRenderer line; //to hold the line Renderer

    [Header("Path")]
    public Transform m_Start_Path; // Inicio del camino
    public Transform m_End_Path; // Final del camino

    public Transform m_Current_Destination; // Destino actual del enemigo
    private float pathEndThreshold = 0.1f;

    // Start is called before the first frame update

    private void Start()
    { 
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        CheckPath();    // Compruebo si tiene Inicio y final, si no da error
        m_Current_Destination = m_End_Path; // Pone el camino actual el final del path
        m_navMeshAgent.SetDestination(m_Current_Destination.position);
    }
    

    private void CheckPath()
    {
        if (m_Start_Path == null)
        {
            m_Start_Path = transform;
            Debug.LogError("Error 0.0: No ha sido posible encontrar el inicio del camino del enemigo " + this.name);
            Application.Quit(); 
        }
        if (m_End_Path == null)
        {
            m_End_Path = transform;
            Debug.LogError("ERROR 0.1: No ha sido posible encontrar el final del camino del enemigo " + this.name);
            Application.Quit();
        }
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log(m_navMeshAgent.hasPath);
        CheckEndPath();
    }

    private void CheckEndPath()
    {
        if (m_navMeshAgent.hasPath && m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance + pathEndThreshold )
        {
            Debug.Log("Ha Llegado a su objetvio y ha cambiado el objetivo que era: " + m_Current_Destination.position);
            m_Current_Destination = (m_Current_Destination.position == m_Start_Path.position) ? m_End_Path : m_Start_Path;
            m_navMeshAgent.SetDestination(m_Current_Destination.position);
        }        
    }

    /*
    private void DrawPath(NavMeshPath path) //Intentando que muestre el camino con una linea para debugeo pero aun no funciona
    { 
        if (path.corners.Length < 2) //if the path has 1 or no corners, there is no need
            return;

        //set the array of positions to the amount of corners
        line.positionCount = path.corners.Length;

        for (var i = 1; i < path.corners.Length; i++)
        {
            Debug.Log(path.corners[i]);
            line.SetPosition(i, path.corners[i]); //go through each corner and set that to the line renderer's position
        }
    }
    */
}
