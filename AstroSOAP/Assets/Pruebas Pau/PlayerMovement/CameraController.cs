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

    void Start()
    {
        if (!m_UseOffsetValues) //si no queremos usar el offset sino donde este la camare en la escena...
        {
            m_Offset = m_CameraTarget.position - transform.position;
        }

        m_CameraPivot.transform.position = m_CameraTarget.transform.position; //pivot coje la posicion del jugador
        m_CameraPivot.transform.parent = m_CameraTarget.transform; //pivot se hace hijo del jugador para poder seguirlo

        Cursor.lockState = CursorLockMode.Locked; //esconde el cursor y lo bloquea al centro de la pantalla

    }

    void LateUpdate() //LateUpdate es lo ultimo que hace en cada frame, por eso renderizamos las cosas al final de todo (Si pones FixedUpdate se ve el porque se pone la camara en LateUpdate)
    {
        //Get the X position of the mouse & rotate the Target 
        float horizontal = Input.GetAxis("Mouse X") * m_RotateSpeed;
        m_CameraTarget.Rotate(0, horizontal, 0);

        //Get the X position of the mouse & rotate the PIVOT 
        float vertical = Input.GetAxis("Mouse Y") * m_RotateSpeed;
        m_CameraPivot.Rotate(-vertical, 0, 0); //menos para invertir el movimiento de la camara


        //Move the camera based on the current rotation of the target & the original offset
        float desiredYAngle = m_CameraTarget.eulerAngles.y; //euler angles hace que el angulo del quaternion sea el que nosotros entendemos (Osea no tiene en cuenta la W)
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
