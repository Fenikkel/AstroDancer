using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickUp : MonoBehaviour
{

    public int m_GoldValue;

    private GameManager m_GameManager;

    // Start is called before the first frame update
    void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) //ElCollider del pick up ha de ser trigger
    {
        if (other.tag == "Player")
        {
            m_GameManager.AddGold(m_GoldValue);

            Destroy(gameObject);
        }
    }

}
