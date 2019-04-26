using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{

    public int m_HealthToGive = 1;
    private HealthManager m_HealthManager;
    [Header("Sound")]
    public AudioClip m_PickAudio; //el archivo de audio
    [Range(0f, 1.0f)]
    public float m_Volume = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_HealthManager = FindObjectOfType<HealthManager>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {

            m_HealthManager.HealPlayer(m_HealthToGive);
            AudioSource.PlayClipAtPoint(m_PickAudio, 0.9f * Camera.main.transform.position + 0.1f * transform.position, m_Volume);
            Destroy(gameObject);
 
        }
    }
}
