using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//https://answers.unity.com/questions/8094/what-are-all-the-joystick-buttons-for-an-xbox-360p.html

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public CharacterController m_PlayerController; //fisicas que usara el jugador
    [HideInInspector]
    public Vector3 m_MoveDirection; //En que direccion se meneara el Jugador
    public Animator m_PlayerAnimator; //m_Player Animator (que este contiene dentro Animation Controller)
    public Transform m_CameraPivot; //Si sabemos el pivot de la camara podemos hacer que el jugador mire hacia el
    public GameObject m_PlayerModel; //referencia al modelo del personaje
    private Vector3 m_ExternalForces;


    [Header("Inputs")]
    public string m_VerticalAxis = "Vertical";
    public string m_HorizontalAxis = "Horizontal";
    public string m_JumpInput = "Jump";

    [Header("Control Variable")]
    public float m_MoveSpeed = 10;
    public float m_JumpForce = 15;
    public float m_GravityScale = 4; //Controlador de la gravedad
    public float m_RotateSpeed = 3;
    public float m_KnockBackForce = 20;
    public float m_KnockBackTime;
    private float m_KnockBackCounter;

    [Header("Sound")]
    public AudioSource m_JumpAudio;
    [Range(0f, 2f)]
    public float m_PitchRange = 0.2f;
    private float m_OriginalPitch; //variamos el pitch alrededor del pitch original


    void Start()
    {
        m_OriginalPitch = m_JumpAudio.pitch;
        m_ExternalForces = Vector3.zero;
        m_PlayerController = GetComponent<CharacterController>();

    }


    void Update() //60fps
    {
        //Debug.Log(m_PlayerController.isGrounded);
        

        if (m_KnockBackCounter <= 0) //si estamos fuera del tiempo de knockback, podemos controlar el jugador
        {

            SetMovement(); //Configuramos el movimiento

            if (m_PlayerController.isGrounded) //Si esta tocando el suelo
            {
                m_MoveDirection.y = 0f; //porque sino estamos aplicando la fuerza de la gravedad todo el rato y el cuerpo pesara un cojon
                if (Input.GetButtonDown(m_JumpInput))
                {
                    Jump(); //Configuramos el salto
                }
            }

        }
        else //sino ve descontando tiempo al tiempo del knockback
        {
            m_KnockBackCounter -= Time.deltaTime;
        }
        Move(); //El jugador se mueve con lo configurado


        //Move the player in different directions based on camera look direction
        if (Input.GetAxis(m_HorizontalAxis) !=0 || Input.GetAxis(m_VerticalAxis) !=0) //si nos meneamos de alguna forma
        {
            RotatePlayerModel();
        }

        SetAnimations(); //ponemos valores a las variables de las animaciones segun como este configurado el movimiento
     }

    private void RotatePlayerModel()
    {
        transform.rotation = Quaternion.Euler(0f, m_CameraPivot.rotation.eulerAngles.y, 0f);
        Vector3 desiredPosition = new Vector3(m_MoveDirection.x, 0f, m_MoveDirection.z); //punto hacia donde quiere ir el jugador (Nada en la Y porque no queremos que mire hacia arriba)
        Quaternion newRotation = Quaternion.LookRotation(desiredPosition); //lookrotation le das un punto en el espacio de unity y le dices que mire hacia ese 
        m_PlayerModel.transform.rotation = Quaternion.Slerp(m_PlayerModel.transform.rotation, newRotation, m_RotateSpeed * Time.deltaTime); //Es una interpolacion pero de rotaciones //Rotamos el modelo del personaje (hijo del personaje) pero no el objeto personaje

    }

    private void SetMovement()
    {
        /*transform.position += m_ExternalForces;
        m_ExternalForces = Vector3.zero; //ponemos a cero las fuerzas que ya se han aplicado
        */

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
        
        m_MoveDirection = movHorizontal + movVertical;// + m_ExternalForces; //external forces son fuerzas como las de las plataformas o otros elementos que mueven al jugador


        m_MoveDirection = m_MoveDirection.normalized * m_MoveSpeed; //noramlizamos el vector para que si puslas dos teclas a la vez no vaya mas rapido el jugador. Osea, no tiene que ser un vector (1,1,0), ha de ser (0.75, 0.75, 0) o algo asi

        m_MoveDirection = m_MoveDirection + m_ExternalForces;
        m_ExternalForces = Vector3.zero; //ponemos a cero las fuerzas que ya se han aplicado
        

        m_MoveDirection.y = yStore; //ponemos la Y guardada que es la buena y no la que se ha generado con todo el pifostio de antes

    }

    private void Jump()
    {
        m_MoveDirection.y = m_JumpForce;

        m_JumpAudio.pitch = UnityEngine.Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
        m_JumpAudio.Play();
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

    private void SetAnimations() //ponemos los valores adecuados a las variables que deciden quue animacion se hara
    {
        m_PlayerAnimator.SetBool("isGrounded", m_PlayerController.isGrounded); //pone el valor booleano que comprueba si esta tocando suelo a la variable del animator
        m_PlayerAnimator.SetFloat("Speed", (Mathf.Abs(Input.GetAxis(m_VerticalAxis)) + Mathf.Abs(Input.GetAxis(m_HorizontalAxis)))); //miramos el valor absoluto de la velocidad que llevan los controles. Si la suma de los dos es mayor a 0.1, el animator controller hara la animacion de run
    }


    public void KnockBack(Vector3 direction)
    {
        m_KnockBackCounter = m_KnockBackTime;

        //direction = new Vector3(1f, 1f, 1f); //for debug //hace de trampolin impulsor

        m_MoveDirection = direction * m_KnockBackForce;
        m_MoveDirection.y = m_KnockBackForce;

    }

    public void ApplyExternalForces(Vector3 externalForces)
    {
        m_ExternalForces += externalForces; //+= por si se le aplican diversas fuerzas
    }

}
