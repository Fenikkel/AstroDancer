using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform m_CameraTarget;
    public Vector3 m_Offset;
    public Transform m_CameraPivot; // con esto, podemos cambiar facilmente de seguir al jugador, a seguir otras cosas (hacer railes por eje`mplo)

    public bool m_UseOffsetValues;

    public float m_RotateSpeed = 5;

    [Range(1, 60)]
    public float m_MaxViewAngle = 45; //hasta donde podremos subir la camara
    [Range(-85, 0)] //-30 es el minimo valor para la camara orthografica (si baja mas estara con la misma posicion en un rango de valores y el control se hace poco responsive)
    public float m_MinViewAngle = -30; //hasta donde podremos bajar la camara (cuanto mas alto mas se acercara al jugador)

    public bool m_InvertY = false;

    void Start()
    {
        if (!m_UseOffsetValues) //si no queremos usar el offset sino donde este la camare en la escena...
        {
            m_Offset = m_CameraTarget.position - transform.position;
        }

        m_CameraPivot.transform.position = m_CameraTarget.transform.position; //pivot coje la posicion del jugador
        //m_CameraPivot.transform.parent = m_CameraTarget.transform; //pivot se hace hijo del jugador para poder seguirlo y que la camara gire cuando gire el jugador

        //con esto sacamos al pivot de ser hijo de la camara
        m_CameraPivot.transform.parent = null; //hacemos esto asi para poder dejar inicialmente(antes de que se de al play) el pivot como hijo de la camara y asi poder hacer un prefab con casi todo configurado.

        Cursor.lockState = CursorLockMode.Locked; //esconde el cursor y lo bloquea al centro de la pantalla

    }

    void LateUpdate() //LateUpdate es lo ultimo que hace en cada frame, por eso renderizamos las cosas al final de todo (Si pones FixedUpdate se ve el porque se pone la camara en LateUpdate)
    {

        m_CameraPivot.transform.position = m_CameraTarget.transform.position; //la camara sigue al jugador (pero sin que el pivot sea el hijo del jugador, asi podemos configurar que la camara no rote cuando el jugador rota)

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
            m_CameraPivot.rotation = Quaternion.Euler(m_MaxViewAngle, 0, 0);
        }
        
        if (m_CameraPivot.rotation.eulerAngles.x > 180 && m_CameraPivot.rotation.eulerAngles.x < 360f + m_MinViewAngle) //360 se posa per si min view te valors negatius
        {
            m_CameraPivot.rotation = Quaternion.Euler(360f + m_MinViewAngle, 0, 0);
        }


        //Move the camera based on the current rotation of the target & the original offset
        float desiredYAngle = m_CameraPivot.eulerAngles.y; //euler angles hace que el angulo del quaternion sea el que nosotros entendemos (Osea no tiene en cuenta la W)
        float desiredXAngle = m_CameraPivot.eulerAngles.x;  //este realmente es el vertical (Mouse Y)

        //desiredXAngle --> nos sobra porque no queremos movimientos de camara verticales
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = m_CameraTarget.position - (rotation * m_Offset); //Asi no funciona --> m_Offset * rotation  (Es al reves)


        //transform.position = m_CameraTarget.position - m_Offset; //esto es sin rotacion

        // si la camara va por debajo de donde esta andando el jugador
        if (transform.position.y < m_CameraTarget.position.y + 0.1f) //el  +0.1f aqui y en la nueva posicion es para que en la camara orthografica no se oculte el terreno (y ademas en perspectiva le da un margen de seguridad)
        {
            //si la camara fuera perspectiva esto, envez de bajar por debajo del jugador, lo que hace es acercarse a el
            transform.position = new Vector3(transform.position.x, m_CameraTarget.position.y + 0.1f, transform.position.z);
        }
        transform.LookAt(m_CameraTarget);
    }
}
