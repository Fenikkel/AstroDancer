using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerWithRigidBody : MonoBehaviour
{

    public float m_MoveSpeed = 10;
    public float m_JumpForce = 5;

    private Rigidbody m_PlayerRigidBody;


    void Start()
    {
        m_PlayerRigidBody = GetComponent<Rigidbody>(); //El objeto tiene que tener el componente RigidBody
    }

    
    void Update()
    {
        Move();

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

    }

    private void Jump()
    {
        float velX = m_PlayerRigidBody.velocity.x; //la velocidad en X que ha calculado el rigidbody del jugador
        float velZ = m_PlayerRigidBody.velocity.z; //mismo en Z

        m_PlayerRigidBody.velocity = new Vector3(velX, m_JumpForce, velZ);
    }

    private void Move()
    {
        //Coje indistintivamente el Input del teclado o el del mando
        float movHorizontal = Input.GetAxis("Horizontal") * m_MoveSpeed; //GetAxis --> entre -1 y 1
        float movVertical = Input.GetAxis("Vertical") * m_MoveSpeed;

        m_PlayerRigidBody.velocity = new Vector3(movHorizontal, m_PlayerRigidBody.velocity.y, movVertical); //m_PlayerRigidBody.velocity.y --> no nos movemos en la y, cojemos la que ya esta

    }

}
