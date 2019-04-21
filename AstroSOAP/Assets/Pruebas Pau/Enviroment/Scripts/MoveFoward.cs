using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFoward : MonoBehaviour
{

    Rigidbody m_Rigidbody;
    public float m_Speed;
    private Vector3 m_NewPosition;

    void Start()
    {
        m_NewPosition = transform.position;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Speed = 10.0f;
    }

    void Update()
    {
        //m_Rigidbody.velocity = transform.forward * m_Speed;
        m_NewPosition.z += m_Speed * Time.deltaTime ;
        transform.position = m_NewPosition;
    }
}
