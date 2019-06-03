using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [HideInInspector]public HealthManager m_TheHealthManager;
    [HideInInspector]public Renderer m_TheMeshRenderer;
    public Material m_CheckPointOffMaterial;
    public Material m_CheckPointOnMaterial;

    [HideInInspector]public CheckPoint[] m_AllCheckPoints;

    private AudioSource m_CheckPointSound;

    private bool m_IsOff = true;

    void Start()
    {
        m_CheckPointSound = GetComponent<AudioSource>();

        m_TheHealthManager = FindObjectOfType<HealthManager>();
        m_TheMeshRenderer = GetComponent<MeshRenderer>();
        m_AllCheckPoints = FindObjectsOfType<CheckPoint>();
    }


    public void CheckPointOn()
    {
        //asi es menos efficiente creo
        /*CheckPoint[] allCheckPoints = FindObjectsOfType<CheckPoint>(); //pilla los checkpoints que estan actualmente ACTIVOS en la escena

        foreach (CheckPoint cp in allCheckPoints)
        {
            cp.CheckPointOff(); 
        }*/

        m_CheckPointSound.Play();

        foreach (CheckPoint cp in m_AllCheckPoints)
        {
            cp.CheckPointOff();
        }
        
        m_TheMeshRenderer.material = m_CheckPointOnMaterial;
        m_IsOff = false;
    }

    public void CheckPointOff()
    {
        m_TheMeshRenderer.material = m_CheckPointOffMaterial;
        m_IsOff = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && m_IsOff) //Equals tiene menor coste que == (mas efficient)
        {
            m_TheHealthManager.SetSpawnPoint(transform.position); //el nuevo spawn point sera el del objeto que tenga este script y haya entrado en trigger collision con el el jugador
            CheckPointOn();
        }
    }
}
