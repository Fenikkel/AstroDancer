using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform m_CameraTarget;
    public Vector3 m_Offset;

    public bool m_UseOffsetValues;

    public float m_RotateSpeed = 5;

    void Start()
    {
        if (!m_UseOffsetValues) //si no queremos usar el offset sino donde este la camare en la escena...
        {
            m_Offset = m_CameraTarget.position - transform.position;
        }
    }

    void Update()
    {
        //Get the X position of the mouse & rotate the target
        float horizontal = Input.GetAxis("Mouse X") * m_RotateSpeed;
        m_CameraTarget.Rotate(0, horizontal, 0);

        float vertical = Input.GetAxis("Mouse Y") * m_RotateSpeed;
        m_CameraTarget.Rotate(-vertical, 0, 0); //menos para invertir el movimiento de la camara


        //Move the camera based on the current rotation of the target & the original offset
        float desiredYAngle = m_CameraTarget.eulerAngles.y; //euler angles hace que el angulo del quaternion sea el que nosotros entendemos (Osea no tiene en cuenta la W)
        float desiredXAngle = m_CameraTarget.eulerAngles.x;  //este realmente es el vertical (Mouse Y)

        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = m_CameraTarget.position - (rotation * m_Offset); //Asi no funciona --> m_Offset * rotation  (Es al reves)


        //transform.position = m_CameraTarget.position - m_Offset; //esto es sin rotacion

        transform.LookAt(m_CameraTarget);
    }
}
