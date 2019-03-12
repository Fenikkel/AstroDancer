using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float m_MoveSpeed = 10;
    public float m_JumpForce = 5;
    public float m_GravityScale = 1; //1 es bastante

    private CharacterController m_PlayerController;
    private Vector3 m_MoveDirection; //En que direccion se meneara el Jugador


    void Start()
    {
        m_PlayerController = GetComponent<CharacterController>();
    }


    void Update() //60fps
    {
        SetMovement(); //Configuramos el movimiento

        if (Input.GetButtonDown("Jump"))
        {
            Jump(); //Configuramos el salto
        }

        Move(); //El jugador se mueve con lo configurado
    }

    private void SetMovement()
    {
        //Coje indistintivamente el Input del teclado o el del mando
        float movHorizontal = Input.GetAxis("Horizontal") * m_MoveSpeed; //GetAxis --> entre -1 y 1
        float movVertical = Input.GetAxis("Vertical") * m_MoveSpeed;

        m_MoveDirection = new Vector3(movHorizontal, 0f, movVertical);
    }

    private void Jump()
    {
        m_MoveDirection.y = m_JumpForce;
    }

    private void Move()
    {
        //Physics.gravity  --->  Edit -> Project Settings -> Physics

        //Aplicamos la gravedad a la configuracion:
        m_MoveDirection.y = m_MoveDirection.y + (Physics.gravity.y * m_GravityScale); //* Time.deltaTime);  //con Time.DeltaTime tambien se puede pero es un delayer que no podemos modificar

        //Aplicamos el movimiento al jugador
        m_PlayerController.Move(m_MoveDirection * Time.deltaTime);
    }

}
