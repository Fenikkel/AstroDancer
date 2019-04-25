using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : MonoBehaviour
{
    public int m_ContainersToGive = 1;
    private HealthManager m_HealthManager;


    // Start is called before the first frame update
    void Start()
    {
        m_HealthManager = FindObjectOfType<HealthManager>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            m_HealthManager.AddHeartContainer(m_ContainersToGive);
            Destroy(gameObject);

        }
    }
}
