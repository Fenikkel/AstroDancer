using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject m_GeneratedPlatform;
    public float m_StartDelay = 0;
    public float m_TimeBetweenPlatforms = 2;


    private float m_GenerateCounter = 0;

    private float m_GeneratorSize;
    private float m_GeneratedPlatformSize;
    private float m_PlatformOffset;

    public bool m_GeneratePlatforms = false;
    void Start()
    {
        m_GeneratorSize = transform.lossyScale.y;
        m_GeneratedPlatformSize = m_GeneratedPlatform.transform.lossyScale.y;
        m_PlatformOffset = (m_GeneratorSize / 2) + (m_GeneratedPlatformSize / 2);

        m_GenerateCounter = m_StartDelay;
    }


    void Update()
    {
        if (m_GenerateCounter <= 0 && m_GeneratePlatforms){ //si podemos generar una plataforma
            Instantiate(m_GeneratedPlatform, new Vector3(transform.position.x, transform.position.y - m_PlatformOffset, transform.position.z), Quaternion.identity);
            m_GenerateCounter = m_TimeBetweenPlatforms;

        }
        else
        {
            m_GenerateCounter -= Time.deltaTime;
        }
    }

    public void GeneratePlatforms(bool bolea)
    {
        m_GeneratePlatforms = bolea; 
    }
}
