using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    //Sin rigidbody mejor porque sino el jugador puede aplicar fuerzas a la plataforma y desviarla de su ruta
    //PERO NO SE COMO HACERLE UN ATTACH DEL PLAYER SIN BUGS, OSEA QUE SE QUEDA CON RIGIDBODY
    //Bugs que da: las particulas que se mueven con el estan siempre activas aunque este quieto + Las plataformas si chocan con algo o incluso con el personaje se paran y no avanzan (no atraviesan)

    Rigidbody m_Rigidbody;
    public float m_Speed = 10.0f;
    private Vector3 m_NewPosition;
    private Vector3 m_PushForces;
    private GameObject m_Player;
    private PlayerController m_PlayerController;


    void Start()
    {
        m_PushForces = Vector3.zero;
        m_NewPosition = transform.position;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Player = GameObject.FindGameObjectWithTag("Player"); //Esta buscant soles un jugador
        m_PlayerController = m_Player.GetComponent<PlayerController>();

    }

    void Update() //no hace falta fixed update para que calcule antes las push forces
    {
        m_Rigidbody.velocity = transform.forward * m_Speed;

        if (transform.childCount > 0) //SI TIENE UN HIJO ES QUE ES el jugador
        {
            //m_PushForces.z = m_Speed * Time.deltaTime;
            m_PushForces = transform.forward * m_Speed;

            m_PlayerController.ApplyExternalForces(m_PushForces);
            m_PushForces = Vector3.zero;

        }
        /*
        m_NewPosition.z += m_Speed * Time.deltaTime;
        transform.position = m_NewPosition;
        */
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == m_Player)
        {
            Debug.Log("Entry");

            m_Player.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == m_Player)
        {
            Debug.Log("Exit");
            m_Player.transform.parent = null;
        }
    }
    
}
