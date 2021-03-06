﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulsePlayer : MonoBehaviour
{

    //El script sirve para acelerar en una dirreccion el jugador y ralentizarlo en la otra direccion. Eso si, esta velocidad ha de ser menor de la que tenga el jugador

    public float m_Speed = 10.0f; //la velocidad ha de ser menor de la que tenga el jugador, sino da bug de revote
    private Vector3 m_PushForces;
    private GameObject m_Player;
    private PlayerController m_PlayerController;
    private AudioSource m_SpeedUpSound;

    private float m_speedOffset;
    private Renderer m_rend;
    private float m_offset;

    void Start()
    {
        m_SpeedUpSound = GetComponent<AudioSource>();
        m_PushForces = Vector3.zero;
        m_Player = GameObject.FindGameObjectWithTag("Player"); //Esta buscant soles un jugador
        m_PlayerController = m_Player.GetComponent<PlayerController>();
        m_rend = GetComponent<Renderer>();
        m_speedOffset = m_Speed * 0.1f;
    }
    private void Update()
    {
        m_offset = Time.time * m_Speed * 0.1f;
        m_rend.material.SetTextureOffset("_MainTex", new Vector2(0, m_offset));
    }

    private void OnTriggerEnter(Collider other)
    {
        m_SpeedUpSound.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == m_Player)
        {
            Debug.Log("Speed Up");
            m_PushForces = transform.forward * m_Speed;

            m_PlayerController.ApplyExternalForces(m_PushForces);
            m_PushForces = Vector3.zero;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == m_Player)
        {
            Debug.Log("ExitSpeedUp");
        }
    }

}
