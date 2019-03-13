using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Inputs")]
    public string m_VerticalAxis = "Vertical";
    public string m_HorizontalAxis = "Horizontal";
    public string m_JumpInput = "Jump";

    [Header("Control Variable")]
    public float m_MoveSpeed = 10;
    public float m_JumpForce = 15;
    public float m_GravityScale = 4; //Controlador de la gravedad
    [HideInInspector]
    public CharacterController m_PlayerController;
    [HideInInspector]
    public Vector3 m_MoveDirection; //En que direccion se meneara el Jugador


    void Start()
    {
        m_PlayerController = GetComponent<CharacterController>();
    }


    void Update() //60fps
    {
        //Debug.Log(m_PlayerController.isGrounded);

        SetMovement(); //Configuramos el movimiento

        if (m_PlayerController.isGrounded) //Si esta tocando el suelo
        {
            m_MoveDirection.y = 0; //porque sino estamos aplicando la fuerza de la gravedad todo el rato y el cuerpo pesara un cojon
            if (Input.GetButtonDown(m_JumpInput))
            {
                Jump(); //Configuramos el salto
            }
        }
        Move(); //El jugador se mueve con lo configurado
    }

    private void SetMovement()
    {
        //Coje indistintivamente el Input del teclado o el del mando

        //Esto va siempre con el mismo eje de coordenadas y no va rotando (son como unos controles de un juego 2D)
        /*
        float movHorizontal = Input.GetAxis(m_HorizontalAxis) * m_MoveSpeed; //GetAxis --> entre -1 y 1
        float movVertical = Input.GetAxis(m_VerticalAxis) * m_MoveSpeed;

        m_MoveDirection = new Vector3(movHorizontal, m_MoveDirection.y, movVertical);
        */

        //transform.right && transform.foward son dos vectores que estan apuntando hacia al lado o hacia delante segun las coordenadas de transform (en nuestro caso el jugador)

        float yStore = m_MoveDirection.y; //guardamos la Y que tenemos
        Vector3 movHorizontal = transform.right * Input.GetAxis(m_HorizontalAxis); 
        Vector3 movVertical = transform.forward * Input.GetAxis(m_VerticalAxis);

        m_MoveDirection = movHorizontal + movVertical;
        m_MoveDirection = m_MoveDirection.normalized * m_MoveSpeed; //noramlizamos el vector para que si puslas dos teclas a la vez no vaya mas rapido el jugador. Osea, no tiene que ser un vector (1,1,0), ha de ser (0.75, 0.75, 0) o algo asi

        m_MoveDirection.y = yStore; //ponemos la Y guardada que es la buena y no la que se ha generado con todo el pifostio de antes

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
