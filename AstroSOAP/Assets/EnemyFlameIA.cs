using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFlameIA : MonoBehaviour
{

    private NavMeshAgent m_navMeshAgent; //NAvMeshAgent del enemigo

    private LineRenderer m_line;
    public Transform[] m_corners;

    private int m_index;
    private object gizmos;



    // Start is called before the first frame update
    void Start()
    {
        m_index = 0;
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_line = GetComponent<LineRenderer>();
        if (m_corners == null || m_corners.Length == 0)
            Debug.LogError("ERROR 0.1: m_corners no esta declarado o no tiene vertices");
        startPath();

    }

    private void startPath()
    {
        m_navMeshAgent.SetDestination(m_corners[0].position);
    }

    private void Update()
    {
        Debug.Log(m_navMeshAgent.hasPath && m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance);
        drawPath(m_navMeshAgent.path);
        CheckStalePath();
        if (m_navMeshAgent.hasPath && m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance)
            nextCorner();
    }

    private void CheckStalePath()
    {
        //Mira si esta atascado en algun sitio
        if (m_navMeshAgent.isPathStale)
            Debug.LogError("ERROR 1.0: El enemigo " + name + " esta atascado");
    }

    private void nextCorner()
    {
        Debug.Log("Esta en el corner numero " + m_index);
        m_index++;
        if (m_index == m_corners.Length)
            m_index = 0;
        m_navMeshAgent.SetDestination(m_corners[m_index].position);
    }

    private void OnDrawGizmos()
    {
        if (m_corners.Length < 2) //if the path has 1 or no corners, there is no need
            return;

        //set the array of positions to the amount of corners
        for (var i = 0; i < m_corners.Length; i++)
        {
            Gizmos.color = Color.blue;
            if (i == m_corners.Length - 1)
                Gizmos.DrawLine(m_corners[i].position, m_corners[0].position);
            else
                Gizmos.DrawLine(m_corners[i].position, m_corners[i + 1].position);
        }
    }

    private void drawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2) //if the path has 1 or no corners, there is no need
            return;

        m_line.positionCount = path.corners.Length;
        m_line.loop = true;
        m_line.SetPositions(path.corners);
    }
}
