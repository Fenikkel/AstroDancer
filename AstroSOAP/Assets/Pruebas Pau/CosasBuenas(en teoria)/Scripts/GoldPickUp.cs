using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickUp : MonoBehaviour
{

    public int m_GoldValue;

    private GameManager m_GameManager;

    [Header("Sound")]
    public AudioClip m_PickAudio; //el archivo de audio
    /*[Range(0f, 2f)]
    public float m_PitchRange = 0.1f;
    private float m_OriginalPitch; //variamos el pitch alrededor del pitch original
    */

    void Start()
    {
        //m_OriginalPitch = m_PickAudio.pitch;
        m_GameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other) //ElCollider del pick up ha de ser trigger
    {
        if (other.tag == "Player")
        {
            m_GameManager.AddGold(m_GoldValue);
            //m_PickAudio.pitch = UnityEngine.Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
            //m_PickAudio.Play();
            AudioSource.PlayClipAtPoint(m_PickAudio, transform.position);

            Destroy(gameObject);
        }
    }

}
