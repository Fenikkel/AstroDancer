using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform m_CameraTarget;
    public Transform m_CameraPivot; // con esto, podemos cambiar facilmente de seguir al jugador, a seguir otras cosas (hacer railes por eje`mplo)

    public float m_RotateSpeed = 5;
    public Vector3 m_Offset; //no se porque pero el offset funciona al reves de lo que le indicas (-y es +y, etc)

    [Range(1, 60)]
    public float m_MaxViewAngle = 45; //hasta donde podremos subir la camara
    [Range(-85, 0)] //-30 es el minimo valor para la camara orthografica (si baja mas estara con la misma posicion en un rango de valores y el control se hace poco responsive)
    public float m_MinViewAngle = -30; //hasta donde podremos bajar la camara (cuanto mas alto mas se acercara al jugador)

    public bool m_InvertY = false;
    public bool m_UseOffsetValues = false;
    public bool m_EnableCameraRotation = false;
    public bool m_EnableSemiCameraRotation = false;

    private Quaternion m_TargetAngle = Quaternion.Euler(0, 0, 0);
    public float m_SemiRotateSpeed = 0.1f;




    void Start()
    {
        if (!m_UseOffsetValues) //si no queremos usar el offset sino donde este la camare en la escena... 
        {
            m_Offset = m_CameraTarget.position - transform.position;
        }
        else //con los offset no mola si rotas la camara, mejor modificarla tu pero que en X = 0!
        {
            m_EnableCameraRotation = false;
            m_EnableSemiCameraRotation = false;

        }

        //con esto sacamos al pivot de ser hijo de la camara
        m_CameraPivot.transform.parent = null; //hacemos esto asi para poder dejar inicialmente(antes de que se de al play) el pivot como hijo de la camara y asi poder hacer un prefab con casi todo configurado.

        //m_CameraPivot.Rotate(Vector3.zero); //por si estaba rotada

        m_CameraPivot.transform.position = m_CameraTarget.transform.position; //pivot coje la posicion del jugador
        //m_CameraPivot.transform.parent = m_CameraTarget.transform; //pivot se hace hijo del jugador para poder seguirlo y que la camara gire cuando gire el jugador



        Cursor.lockState = CursorLockMode.Locked; //esconde el cursor y lo bloquea al centro de la pantalla
        //print(transform.rotation.eulerAngles.y);

        transform.position = m_CameraTarget.position - m_Offset; //DePRUEBAS


        if (!m_EnableCameraRotation && !m_EnableSemiCameraRotation) //si la camara no gira, hay que poner manualmente la rotacion en Y de la camara para que el jugador no vaya en direcciones que no toca
        {
            m_CameraPivot.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); //asi tendra la misma direccion que la camara y si el jugador se menea en base al pivote se meneara correctamente
        }
        else if(m_EnableCameraRotation)
        {
            m_EnableSemiCameraRotation = false;
            m_CameraPivot.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else //es camera semirotation
        {
            //m_CameraPivot.transform.rotation = Quaternion.Euler(0, 45, 0);
            //PARA TENER LA CAMERA GIRADA YA
            m_CameraPivot.Rotate(0, -90, 0);

            float desiredYAngle = m_CameraPivot.eulerAngles.y; //euler angles hace que el angulo del quaternion sea el que nosotros entendemos (Osea no tiene en cuenta la W)

            Quaternion rotation = Quaternion.Euler(0, desiredYAngle, 0);
            transform.position = m_CameraTarget.transform.position - (rotation * m_Offset); //Asi no funciona --> m_Offset * rotation  (Es al reves)

            transform.LookAt(m_CameraTarget);

            Debug.Log("SemiRotation");
        }


        m_TargetAngle = Quaternion.Euler(m_CameraPivot.eulerAngles.x, m_CameraPivot.eulerAngles.y, m_CameraPivot.eulerAngles.z);
        transform.LookAt(m_CameraTarget);

    }

    void LateUpdate() //LateUpdate es lo ultimo que hace en cada frame, por eso renderizamos las cosas al final de todo (Si pones FixedUpdate se ve el porque se pone la camara en LateUpdate)
    {

        m_CameraPivot.transform.position = m_CameraTarget.transform.position; //la camara sigue al jugador (pero sin que el pivot sea el hijo del jugador, asi podemos configurar que la camara no rote cuando el jugador rota)

        

        if (m_EnableCameraRotation)
        {
            RotateCamera();
            //Debug.Log(transform.rotation.eulerAngles.y);
        }
        else if (m_EnableSemiCameraRotation)
        {
            SemiRotateCamera();
        }
        else
        {
            MoveCamera();
        }

    }

    private void MoveCamera()
    {
        transform.position = m_CameraTarget.position - m_Offset;
    }


    private void SemiRotateCamera()
    {

        //PROBLEMA CUANDO HACIENDO LA INTERPOLACION, ACABA CON EL MISMO ANGULO PERO CAMERA PIVOT Y TARGETANGLE NO TIENEN LOS MISMOS VALORES
        //Debug.Log(Input.GetAxisRaw("Mouse X"));


        Debug.Log(m_CameraPivot.transform.rotation);
        Debug.Log(m_TargetAngle);


        m_CameraPivot.transform.rotation = Quaternion.Slerp(m_CameraPivot.transform.rotation, m_TargetAngle, m_SemiRotateSpeed);

        if (QuaternionAbs(m_CameraPivot.transform.rotation)  == QuaternionAbs(m_TargetAngle))
        {
            print("yata");

            float mouse = Input.GetAxisRaw("Mouse X");


            if (mouse > 0 || Input.GetAxis("RButton") != 0)
            {

                print("Dreta");

                m_TargetAngle = Quaternion.Euler(m_CameraPivot.eulerAngles.x, (m_CameraPivot.eulerAngles.y - 90), m_CameraPivot.eulerAngles.z);
                //m_CameraPivot.Rotate(0, -90 , 0); 
                //m_CameraPivot.rotation = Quaternion.Lerp(desde.rotation, hasta.rotation, Time.time * 0.1f);
            }
            else if (mouse < 0 || Input.GetAxis("LButton") != 0)
            {
                print("Esquerra");


                /*Transform desde = m_CameraPivot;
                Transform hasta = desde;
                hasta.Rotate(0, 90, 0);
                m_CameraPivot.rotation = Quaternion.Lerp(desde.rotation, hasta.rotation, Time.time * 0.1f);
                */
                //m_CameraPivot.Rotate(0, 90 , 0); 

                m_TargetAngle = Quaternion.Euler(m_CameraPivot.eulerAngles.x, (m_CameraPivot.eulerAngles.y + 90), m_CameraPivot.eulerAngles.z);

            }
        }

        

        //m_CameraPivot.Rotate(0, -1 * (mouse * 90) , 0); //seria bo si donara -1 0 i 1 pero el ratoli no ho dona aixina


        /*Quaternion pivotRotation = Quaternion.Euler(0, -1 * (mouse * 90), 0);
        m_CameraPivot.Rotate(pivotRotation);*/

        //Move the camera based on the current rotation of the target & the original offset
        float desiredYAngle =   m_CameraPivot.eulerAngles.y; //euler angles hace que el angulo del quaternion sea el que nosotros entendemos (Osea no tiene en cuenta la W)

        Quaternion rotation = Quaternion.Euler(0, desiredYAngle, 0);
        transform.position = m_CameraTarget.transform.position - (rotation * m_Offset); //Asi no funciona --> m_Offset * rotation  (Es al reves)

        transform.LookAt(m_CameraTarget);
    }

    private void RotateCamera()
    {
        //Debug.Log(Input.GetAxis("Mouse X"));
        //Get the X position of the mouse & rotate the Target  
        float horizontal = (Input.GetAxis("Mouse X")) * m_RotateSpeed; //comienza siempre con 0)  
        m_CameraPivot.Rotate(0, horizontal, 0);
        

        //Move the camera based on the current rotation of the target & the original offset
        float desiredYAngle = m_CameraPivot.eulerAngles.y; //euler angles hace que el angulo del quaternion sea el que nosotros entendemos (Osea no tiene en cuenta la W)

        Quaternion rotation = Quaternion.Euler(0, desiredYAngle, 0);
        transform.position = m_CameraTarget.transform.position - (rotation * m_Offset); //Asi no funciona --> m_Offset * rotation  (Es al reves)
        

        
        transform.LookAt(m_CameraTarget);

        
    }

    private void MoveNRotateCamera()
    {
        //Get the X position of the mouse & rotate the Target 
        float horizontal = Input.GetAxis("Mouse X") * m_RotateSpeed;
        m_CameraPivot.Rotate(0, horizontal, 0);

        //Get the Y position of the mouse & rotate the PIVOT 
        float vertical = Input.GetAxis("Mouse Y") * m_RotateSpeed;

        if (m_InvertY) //para los raros
        {
            m_CameraPivot.Rotate(vertical, 0, 0); //sin menos para invertir el movimiento de la camara (la camara mira al lado contrario del raton?)
        }
        else //para los normales
        {
            m_CameraPivot.Rotate(-vertical, 0, 0); //menos para el movimiento de la camara normal (la camara mira hacia donde va el raton?)
        }

         if (m_CameraPivot.rotation.eulerAngles.x > m_MaxViewAngle && m_CameraPivot.rotation.eulerAngles.x < 180f) //Lo primero es para que la camera no pase de 45 grados y lo segundo es una movida chunga del editor pero tragate que eso hace que cuando bajes la camara no te pondra la camara a 45 grados (osea entrar en estre if)
         {
             m_CameraPivot.rotation = Quaternion.Euler(m_MaxViewAngle, m_CameraPivot.rotation.eulerAngles.y, m_CameraPivot.rotation.eulerAngles.z);
         }

         if (m_CameraPivot.rotation.eulerAngles.x > 180 && m_CameraPivot.rotation.eulerAngles.x < 360f + m_MinViewAngle) //360 se posa per si min view te valors negatius
         {
             m_CameraPivot.rotation = Quaternion.Euler(360f + m_MinViewAngle, m_CameraPivot.rotation.eulerAngles.y, m_CameraPivot.rotation.eulerAngles.z);
         }


        //Move the camera based on the current rotation of the target & the original offset
        float desiredYAngle = m_CameraPivot.eulerAngles.y; //euler angles hace que el angulo del quaternion sea el que nosotros entendemos (Osea no tiene en cuenta la W)
        float desiredXAngle = m_CameraPivot.eulerAngles.x;  //este realmente es el vertical (Mouse Y)

        //desiredXAngle --> nos sobra porque no queremos movimientos de camara verticales
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = m_CameraTarget.transform.position - (rotation * m_Offset); //Asi no funciona --> m_Offset * rotation  (Es al reves)


        //transform.position = m_CameraTarget.position - m_Offset; //esto es sin rotacion

        // si la camara va por debajo de donde esta andando el jugador
        if (transform.position.y < m_CameraTarget.transform.position.y + 0.1f) //el  +0.1f aqui y en la nueva posicion es para que en la camara orthografica no se oculte el terreno (y ademas en perspectiva le da un margen de seguridad)
        {
            //si la camara fuera perspectiva esto, envez de bajar por debajo del jugador, lo que hace es acercarse a el
            transform.position = new Vector3(transform.position.x, m_CameraTarget.transform.position.y + 0.1f, transform.position.z);
        }
        transform.LookAt(m_CameraTarget);
    }

    private Quaternion QuaternionAbs(Quaternion q)
    {
        return new Quaternion(Mathf.Abs(q.x), Mathf.Abs(q.y), Mathf.Abs(q.z), Mathf.Abs(q.w));
    }


}