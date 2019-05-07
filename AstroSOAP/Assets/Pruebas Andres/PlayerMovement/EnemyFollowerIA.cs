using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowerIA : MonoBehaviour
{

    private NavMeshAgent m_navMeshAgent; //NAvMeshAgent del enemigo
    private Transform m_player; //Transform del player

    public LineRenderer m_line; //to hold the line Renderer

    [Header("Path")]
    public Transform m_Start_Path; // Inicio del camino
    public Transform m_End_Path; // Final del camino
    public Vector3 m_Last_Point_Path; // Ultimo punto del camino antes de seguir al personaje
    public Transform m_Current_Destination; // Destino actual del enemigo
    [Header("Enemy Settings")]
    public float m_distance_Error; // Distancia para detectar si hay un error
    public float m_velocityBase;         // Velocidad base del enemigo
    public float m_DistanceEnemyPlayer; // Distancia en que empieza a segir al jugador
    public bool m_isFollowing = false;

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
        CheckDistancePlayer();
        drawPath(m_navMeshAgent.path.corners);
        UpdateVelocity();
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
    private bool CheckEndPath() // Calcula si ha llegado al final del camino
    {
        if (m_navMeshAgent.hasPath && m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance)
        {
            Debug.Log("El enemigo " + name + " ha llegado a su objetvio y ha cambiado el objetivo");

            if (m_Last_Point_Path == Vector3.zero)
            {
                m_Current_Destination = (m_Current_Destination.position == m_Start_Path.position) ? m_End_Path : m_Start_Path;
                m_navMeshAgent.SetDestination(m_Current_Destination.position);
            }
            return true;
        }
        return false;
    }

    private void CheckDistancePlayer()
    {
        if (Vector3.Distance(transform.position, m_player.transform.position) < m_DistanceEnemyPlayer)
        {
            if (!m_isFollowing)
            {
                m_isFollowing = true;
                if (m_Last_Point_Path == Vector3.zero)
                    m_Last_Point_Path = transform.position;
            }
            m_navMeshAgent.SetDestination(m_player.transform.position);
        }
        else if (m_isFollowing)
        {
            m_navMeshAgent.SetDestination(m_Last_Point_Path);
            if (CheckEndPath())
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


    private void UpdateVelocity() // Modifica la velocidad del enemigo segun la pendiente que este subiendo
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit ray))
        {
            float angle = Vector3.Angle(Vector3.up, ray.normal);
            if (angle == 0)
            {
                m_navMeshAgent.speed = m_velocityBase;
                return;
            }

            float increment = (angle / 80) + 1;
            m_navMeshAgent.speed = m_velocityBase * increment;
        }

    }

    private void OnDrawGizmos()
    {
        if (m_Start_Path == null || m_End_Path == null) //if the path has 1 or no corners, there is no need
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(m_Start_Path.position, m_End_Path.position);

    }

    private void drawPath(Vector3[] path) //Muestre el camino con una linea
    {
        if (path.Length < 2) //if the path has 1 or no corners, there is no need
            return;

        m_line.positionCount = path.Length;
        m_line.loop = true;
        m_line.SetPositions(path);
    }
}
