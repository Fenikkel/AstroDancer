using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFoward : MonoBehaviour
{

    Rigidbody m_Rigidbody;
    public float m_Speed;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Speed = 10.0f;
    }

    void Update()
    {
        m_Rigidbody.velocity = transform.forward * m_Speed;
    }
}
