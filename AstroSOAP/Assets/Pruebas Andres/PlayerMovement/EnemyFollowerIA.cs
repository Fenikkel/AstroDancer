using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowerIA : MonoBehaviour
{

    private NavMeshAgent m_navMeshAgent; //NAvMeshAgent del enemigo
    private Transform m_player; //Transform del player
    public bool m_isFollowing = false;
    public float m_DistanceEnemyPlayer; // Distancia en que empieza a segir al jugador

    public bool showPath;
    public LineRenderer line; //to hold the line Renderer

    [Header("Path")]
    public Transform m_Start_Path; // Inicio del camino
    public Transform m_End_Path; // Final del camino
    public Vector3 m_Last_Point_Path; // Ultimo punto del camino antes de seguir al personaje
    public float m_distance_Error; // Distancia para detectar si hay un error 

    public Transform m_Current_Destination; // Destino actual del enemigo
    private float m_pathEndThreshold = 0.2f; // Distancia para detectar si ha llegado al final del camino

    // Start is called before the first frame update

    private void Start()
    { 
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        CheckPath();    // Compruebo si tiene Inicio y final, si no da error
        m_Current_Destination = m_End_Path; // Pone el camino actual el final del path
        m_navMeshAgent.SetDestination(m_Current_Destination.position);
        CheckPath();
    }
    

    private void CheckPath()
    {
        if (m_Start_Path == null)
        {
            m_Start_Path = transform;
            Debug.LogError("Error 0.0: No ha sido posible encontrar el inicio del camino del enemigo " + name);
        }
        if (m_End_Path == null)
        {
            Debug.LogError("ERROR 0.1: No ha sido posible encontrar el final del camino del enemigo " + name);
            Application.Quit();
        }
    }


    // Update is called once per frame
    void Update()
    {
        CheckStalePath();
        CheckEndPath();
        //CheckDistancePlayer();
        DrawPath(m_navMeshAgent.path);
    }

    private void CheckStalePath()
    {
        //Mira si esta atascado en algun sitio
        if (m_navMeshAgent.isPathStale)
            Debug.LogError("ERROR 1.0: El enemigo " + name + " esta atascado");
    }

    /*
    private void CheckDistancePath()
    {
        // Mira si la distancia entre el final del camino generado por el navMeshAgent y el final que le hemos puesto supera un cierto error y no llega de verdad
        if (Vector3.Distance(m_navMeshAgent.pathEndPosition, m_Current_Destination.position) > m_distance_Error)
        {
            Debug.LogError("Error 0.2: El final del camino del enemigo " + name + " esta a mas de " + m_distance_Error + " del final desigando");
            m_navMeshAgent.isStopped = true;
            Destroy(this);
        }
    }
    */
    private bool CheckEndPath()
    {
        if (m_navMeshAgent.hasPath && m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance + m_pathEndThreshold)
        {
            Debug.Log("El enemigo " + name + " ha llegado a su objetvio y ha cambiado el objetivo");
            m_Current_Destination = (m_Current_Destination.position == m_Start_Path.position) ? m_End_Path : m_Start_Path;
            m_navMeshAgent.SetDestination(m_Current_Destination.position);
            return true;
        }
        return false;
    }

    private void CheckDistancePlayer()
    {
        if(Vector3.Distance(transform.position, m_player.transform.position) < m_DistanceEnemyPlayer)
        {
            if(!m_isFollowing)
            {
                m_isFollowing = true;
                if(m_Last_Point_Path == Vector3.zero)
                    m_Last_Point_Path = transform.position;
            }
            m_navMeshAgent.SetDestination(m_player.transform.position);
        }
        else if(m_isFollowing)
        {
            m_navMeshAgent.SetDestination(m_Last_Point_Path);
            if(CheckEndPath())
            {
                m_isFollowing = false;
                m_navMeshAgent.SetDestination(m_Current_Destination.position);
                m_Last_Point_Path = Vector3.zero;
            }
        }
        else
        {
            //CheckDistancePath();
        }
    }

    
    private void DrawPath(NavMeshPath path) //Muestre el camino con una linea
    {

        if (path.corners.Length < 2) //if the path has 1 or no corners, there is no need
            return;

        //set the array of positions to the amount of corners
        line.positionCount = path.corners.Length;

        for (var i = 0; i < path.corners.Length; i++)
        {
            Debug.Log(path.corners[i]);
            line.SetPosition(i, path.corners[i]); //go through each corner and set that to the line renderer's position
        }       
    }
    
}
