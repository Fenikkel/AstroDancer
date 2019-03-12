using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float m_MoveSpeed = 10;
    public float m_JumpForce = 15;
    public float m_GravityScale = 4; //Controlador de la gravedad

    private CharacterController m_PlayerController;
    private Vector3 m_MoveDirection; //En que direccion se meneara el Jugador


    void Start()
    {
        m_PlayerController = GetComponent<CharacterController>();
    }


    void Update() //60fps
    {
        Debug.Log(m_PlayerController.isGrounded);

        SetMovement(); //Configuramos el movimiento

        if (m_PlayerController.isGrounded) //Si esta tocando el suelo
        {
            m_MoveDirection.y = 0; //porque sino estamos aplicando la fuerza de la gravedad todo el rato y el cuerpo pesara un cojon
            if (Input.GetButtonDown("Jump"))
            {
                Jump(); //Configuramos el salto
            }
        }
        Move(); //El jugador se mueve con lo configurado
    }

    private void SetMovement()
    {
        //Coje indistintivamente el Input del teclado o el del mando
        float movHorizontal = Input.GetAxis("Horizontal") * m_MoveSpeed; //GetAxis --> entre -1 y 1
        float movVertical = Input.GetAxis("Vertical") * m_MoveSpeed;
        
        m_MoveDirection = new Vector3(movHorizontal, m_MoveDirection.y, movVertical);
        
    }

    private void Jump()
    {
        m_MoveDirection.y = m_JumpForce;
    }

    private void Move()
    {
        //Physics.gravity  --->  Edit -> Project Settings -> Physics

        /*if (!m_PlayerController.isGrounded) //Aplicamos la gravedad a la configuracion si no estamos tocando el suelo
        {
            m_MoveDirection.y = m_MoveDirection.y + (Physics.gravity.y * m_GravityScale * Time.deltaTime); //con Time.DeltaTime hecemos que sea mas smooth y con m_GravityScale podemos controlar la fuerza
        }*/

        m_MoveDirection.y = m_MoveDirection.y + (Physics.gravity.y * m_GravityScale * Time.deltaTime); //con Time.DeltaTime hecemos que sea mas smooth y con m_GravityScale podemos controlar la fuerza

        //Aplicamos el movimiento al jugador
        //HAY QUE HACER UNITARIO EL VECTOR PARA QUE SEA LA MISMA VELOCIDAD PARA TODAS LAS DIRECCIONES??
        m_PlayerController.Move(m_MoveDirection * Time.deltaTime); //Se mueve hacia donde indica el vector (Supongo que le estamos indicando cuantas unidades se ha de mover en esa direccion, no al punto donde debe ir)
    }

}
