using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{

    public int m_HealthToGive = 1;
    private HealthManager m_HealthManager;


    // Start is called before the first frame update
    void Start()
    {
        m_HealthManager = FindObjectOfType<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        print("Heal");
        if (other.gameObject.tag == "Player")
        {

            m_HealthManager.HealPlayer(m_HealthToGive);
            Destroy(gameObject);
 
        }
    }
}
