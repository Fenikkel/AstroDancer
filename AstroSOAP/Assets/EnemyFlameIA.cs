using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFlameIA : MonoBehaviour
{

    private NavMeshAgent m_navMeshAgent; //NAvMeshAgent del enemigo

    public LineRenderer m_line; //to hold the line Renderer
    public Transform[] m_corners;

    private int m_index;



    // Start is called before the first frame update
    void Start()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_line = GetComponent<LineRenderer>();
        if (m_corners == null || m_corners.Length == 0)
            Debug.LogError("ERROR 0.1: m_corners no esta declarado o no tiene vertices");
        DrawPath();
        startPath();

    }

    private void startPath()
    {
        m_navMeshAgent.SetDestination(m_corners[0].position);
    }

    private void Update()
    {
        CheckStalePath();
        if (m_navMeshAgent.hasPath && m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance)
        {
            m_index++;
            if (m_index == m_corners.Length)
                m_index = 0;
            m_navMeshAgent.SetDestination(m_corners[m_index].position);
        }
    }

    private void CheckStalePath()
    {
        //Mira si esta atascado en algun sitio
        if (m_navMeshAgent.isPathStale)
            Debug.LogError("ERROR 1.0: El enemigo " + name + " esta atascado");
    }

    private void DrawPath() //Muestre el camino con una linea
    {

        if (m_corners.Length < 2) //if the path has 1 or no corners, there is no need
            return;

        //set the array of positions to the amount of corners
        m_line.loop = true;
        m_line.positionCount = m_corners.Length;

        for (var i = 0; i < m_corners.Length; i++)
            m_line.SetPosition(i, m_corners[i].position); //go through each corner and set that to the line renderer's position
    }

}
