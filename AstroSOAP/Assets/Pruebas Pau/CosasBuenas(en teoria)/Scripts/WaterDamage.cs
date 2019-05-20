using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDamage : MonoBehaviour
{
    private HealthManager m_HealthManager;

    private void Awake()
    {
        m_HealthManager = FindObjectOfType<HealthManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_HealthManager.KillPlayer();
        }
    }
}
