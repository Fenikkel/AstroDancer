using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpinController : MonoBehaviour
{
    //El pivot tiene que tener la Z mirando hacia adelante

    public float m_Distance = 2f;
    public float m_StartDelay = 0f;
    public float m_SpinDelay = 1f;
    public float m_RotateSpeed = 10f;
    public bool m_TwoWaters = true;


    private float m_SpinCounter = 0;

    private Transform m_WaterParticles;
    private Transform m_WaterParticles2;
    private Vector3 m_InitialRotation;


    private float m_CircunferenceCounter=0;


    void Start()
    {
        GetComponent<Renderer>().enabled = false;
        m_InitialRotation = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        m_WaterParticles = transform.GetChild(0);
        m_WaterParticles2 = transform.GetChild(1);

        //m_WaterParticles.transform.position = new Vector3(transform.position.x ,transform.position.y, transform.position.z - m_Distance); //Esto es WORLD position

        if (m_TwoWaters)
        {
            m_WaterParticles.transform.localPosition = new Vector3(0f, 0f, -m_Distance);
            m_WaterParticles2.transform.localPosition = new Vector3(0f, 0f, m_Distance);
        }
        else
        {
            m_WaterParticles.transform.localPosition = new Vector3(0f, -m_Distance, 0f); //Asi es LOCAL position
        }

        m_SpinCounter = m_StartDelay; //para empezar mas tarde y dar un offsed de diferencia con los siguientes
    }

    
    void Update()
    {
        ThreeSixty();
    }

    private void ThreeSixty()
    {
        //Debug.Log(m_SpinCounter);
        //Debug.Log(m_CircunferenceCounter);

        if (m_SpinCounter <= 0) //si podemos girar
        {

            //Lerp mejor?
            transform.Rotate(Time.deltaTime * m_RotateSpeed, 0, 0);

            m_CircunferenceCounter += Time.deltaTime * m_RotateSpeed;
            if (m_CircunferenceCounter > 360f) //Si ha pegado una vuelta
            {
                //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.x);
                transform.rotation = Quaternion.Euler(m_InitialRotation.x, m_InitialRotation.y, m_InitialRotation.z);

                m_CircunferenceCounter = 0;
                m_SpinCounter = m_SpinDelay;
            }

        }
        else //sino tiene que esperar
        {
            m_SpinCounter -= Time.deltaTime;
        }
    }
}
