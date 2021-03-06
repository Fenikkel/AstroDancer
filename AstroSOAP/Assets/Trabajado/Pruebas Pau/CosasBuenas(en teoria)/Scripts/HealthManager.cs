﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HealthManager : MonoBehaviour
{
    
    public PlayerController m_ThePlayer;
    public PlatformGenerator m_PlatformGenerator;
    public Renderer m_PlayerRenderer; //para poder hacer parpadear el jugador
    public GameObject m_DeathEffect;

    [Header("UI")]
    public Image m_BlackScreen;
    public Text m_HealthText;
    public TMP_Text m_LivesText;


    public bool m_OldUI = false; 
    public Sprite m_FullHeart;
    public Sprite m_EmptyHeart;
    public Image[] m_HeartArray;

    //CON LA NUEVA UI SOLO PUEDEN HABER 3 HEART CONTAINERS
    public Image m_WaveImage;
    public Sprite[] m_WaveSpriteArray;


    [Header("Sound")]
    public AudioSource m_DeathAudio;
    public AudioSource m_HurtAudio;
    [Range(0f, 2f)]
    public float m_PitchRange = 0.2f;
    private float m_OriginalPitch; //variamos el pitch alrededor del pitch original

    [Header("Health")]
    public int m_CurrentHealth = 2;
    [Range(1, 5)]
    public int m_MaxHealth = 3; //en ese momento, no la cantidad maxima de corazones que podemos tener

    public int m_MaxHeartContainers = 5; //cantidad maxima de containers (no han de superar el heart array length)

    public int m_Lives = 4;



    [Header("Invencibility")]
    public float m_InvincibilityLength = 1f;
    private float m_InvincibilityCounter;
    private float m_FlashCounter;
    public float m_FlashLength = 0.1f; //tiempo entre parpadeo

    [Header("Respawn")]
    private bool m_IsRespawning = false;
    public Vector3 m_RespawnPoint;
    public float m_RespawnLength = 2;
    private bool m_IsFadeToBlack;
    private bool m_IsFadeFromBlack;
    public float m_FadeSpeed = 2f; //Para que vaya bien, fade speed tiene que ser mayor que wait for fade
    public float m_WaitForFade = 1f;



    void Start()
    {
        //m_HurtAudio = GetComponent<AudioSource>();
        m_OriginalPitch = m_HurtAudio.pitch;

        m_CurrentHealth = m_MaxHealth;

        //m_HealthText.text = "Health: " + m_CurrentHealth;

        if (m_OldUI)
        {
            for (int i = 0; i < m_HeartArray.Length; i++)
            {

                if (i < m_CurrentHealth)
                {
                    m_HeartArray[i].sprite = m_FullHeart;
                }
                else
                {
                    m_HeartArray[i].sprite = m_EmptyHeart;
                }

                if (i < m_MaxHealth)
                {
                    //print("one heart");
                    m_HeartArray[i].enabled = true;
                }
                else
                {
                    m_HeartArray[i].enabled = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < m_WaveSpriteArray.Length; i++)
            {

                if (i == m_CurrentHealth)
                {
                    m_WaveImage.sprite =  m_WaveSpriteArray[i];
                }

            }
        }


        


        //m_ThePlayer = FindObjectOfType<PlayerController>();

        //m_RespawnPoint = m_ThePlayer.transform.position;
    }
    private void Update()
    {

        if (m_InvincibilityCounter > 0)
        {
            m_InvincibilityCounter -= Time.deltaTime;

            m_FlashCounter -= Time.deltaTime;

            if (m_FlashCounter <=0) //si el tiempo del parpadeo ha pasado (tiempo entre activado y desactivado o viceversa)
            {
                m_PlayerRenderer.enabled = !m_PlayerRenderer.enabled;
                m_FlashCounter = m_FlashLength; //vuelve a poner el tiempo entre el parpadeo
            }
            if (m_InvincibilityCounter <= 0 )  //esto para el caso en que si cuando se acaba la invencibilidad se ha quedado apagado el render
            {
                m_PlayerRenderer.enabled = true;
            }
        }

        if (m_IsFadeToBlack)
        {
            m_BlackScreen.color = new Color(m_BlackScreen.color.r, m_BlackScreen.color.g, m_BlackScreen.color.b, Mathf.MoveTowards(m_BlackScreen.color.a, 1f, m_FadeSpeed * Time.deltaTime)); //con moive towards ira incrementando m_BlackScreen.color.a hasta 1f sin passarlo sumandole el valor de m_FadeSpeed * Time.deltaTime  (con delta time hacemos que se sume un poca cada frame)
            if (m_BlackScreen.color.a == 1f)
            {
                m_IsFadeToBlack = false;
            }
        }

        if (m_IsFadeFromBlack)
        {
            m_BlackScreen.color = new Color(m_BlackScreen.color.r, m_BlackScreen.color.g, m_BlackScreen.color.b, Mathf.MoveTowards(m_BlackScreen.color.a, 0f, m_FadeSpeed * Time.deltaTime)); //con moive towards ira incrementando m_BlackScreen.color.a hasta 1f sin passarlo sumandole el valor de m_FadeSpeed * Time.deltaTime  (con delta time hacemos que se sume un poca cada frame)
            if (m_BlackScreen.color.a == 0f)
            {
                m_IsFadeFromBlack = false;
            }
        }
    }

    public void KillPlayer()
    {
        m_CurrentHealth = 0;
        //m_HealthText.text = "Health: " + m_CurrentHealth;

        UpdateUI();
        m_DeathAudio.Play();
        Respawn();
    }

    public void HurtPlayer(int damage, Vector3 knockBackDirection)
    {

        if (m_InvincibilityCounter <= 0) { //si no estamos en modo invencibilidad

            m_CurrentHealth -= damage;
            if (m_CurrentHealth<0)
            {
                m_CurrentHealth = 0;
            }
            //m_HealthText.text = "Health: " + m_CurrentHealth;

            UpdateUI();

            if (m_CurrentHealth <= 0)
            {
                m_DeathAudio.Play();
                Respawn();
                
            }
            else
            {
                m_HurtAudio.pitch = UnityEngine.Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_HurtAudio.Play();

                m_ThePlayer.KnockBack(knockBackDirection);

                m_InvincibilityCounter = m_InvincibilityLength;

                m_PlayerRenderer.enabled = false;

                m_FlashCounter = m_FlashLength;
            }
        }
        /*if (m_CurrentHealth < 0) //para que no hago over kill y de problemas
        {
            m_CurrentHealth = 0;
        }*/
    }

    public void HealPlayer(int healAmount)
    {
        m_CurrentHealth += healAmount;
        //m_HealthText.text = "Health: " + m_CurrentHealth;

        UpdateUI();

        if (m_CurrentHealth> m_MaxHealth){

            m_CurrentHealth = m_MaxHealth;
            //m_HealthText.text = "Health: " + m_CurrentHealth;

        }
    }


    public void Respawn()
    {
        if (!m_IsRespawning) //si no estamos respawneando
        {
            print("Lives: = " + m_Lives);
            m_Lives--;
            m_LivesText.text = m_Lives.ToString();
            print("Lives: = " + m_Lives);
            StartCoroutine("RespawnCo");
        }
    }

    public IEnumerator RespawnCo() //respawn coroutine
    {
        m_IsRespawning = true; //mientras respawneamos restringimos el respawn para no hacerlo varias veces

        StopScene(true);
        m_ThePlayer.gameObject.SetActive(false);
        GameObject rubish = Instantiate(m_DeathEffect, m_ThePlayer.transform.position, m_ThePlayer.transform.rotation);

        yield return new WaitForSeconds(m_RespawnLength);

        m_IsFadeToBlack = true;

        yield return new WaitForSeconds(m_WaitForFade);

        m_IsFadeToBlack = false; //Es de seguridad, por si el m_FadeSpeed es menor que m_WaitForFade
        m_IsFadeFromBlack = true;
        Destroy(rubish);

        m_IsRespawning = false; //ya podemos volver a respawnear

        // m_ThePlayer.gameObject.SetActive(true); Si activamos el personaje antes de moverlo entrara en el trigger del agua de matarlo y por eso a veces no respawneaba bien. Bug Fixed.

        StopScene(false);
        //respawneamos el jugador
        m_ThePlayer.transform.position = m_RespawnPoint;
        m_ThePlayer.gameObject.SetActive(true);



        //Debug.Log(m_RespawnPoint);
        //Debug.Log("m_Player " + m_ThePlayer.transform.position);
        m_CurrentHealth = m_MaxHealth;
        UpdateUI();

        //lo hacemos invencible
        m_InvincibilityCounter = m_InvincibilityLength;
        m_PlayerRenderer.enabled = false;
        m_FlashCounter = m_FlashLength;
    }


    public void SetSpawnPoint(Vector3 newRespawnPosition)
    {
        m_RespawnPoint = newRespawnPosition;
    }

    private void StopScene(bool stop)
    {
        if (m_PlatformGenerator != null)
        {
            m_PlatformGenerator.GeneratePlatforms(!stop);
        }
    }

    private void UpdateUI()
    {
        if (m_OldUI)
        {
            for (int i = 0; i < m_HeartArray.Length; i++)
            {

                if (i < m_CurrentHealth)
                {
                    m_HeartArray[i].sprite = m_FullHeart;
                }
                else
                {
                    m_HeartArray[i].sprite = m_EmptyHeart;
                }
            }
        }
        else{

            for (int i = 0; i < m_WaveSpriteArray.Length; i++)
            {

                if (i == m_CurrentHealth)
                {
                    m_WaveImage.sprite = m_WaveSpriteArray[i];
                }

            }
        }

 
    }

    public void AddHeartContainer(int heartsToAdd)
    {
        if ((m_MaxHealth + heartsToAdd) > m_MaxHeartContainers)
        {
            m_MaxHealth = m_MaxHeartContainers; //no queremos pasarnos del maximo
        }
        else
        {
            m_MaxHealth += heartsToAdd; //incrementamos los containers
        }
        HealPlayer(heartsToAdd);

        for (int i = 0; i < m_HeartArray.Length; i++)
        {
             if (i < m_MaxHealth)
             {
                m_HeartArray[i].enabled = true;
             }
             else
             {
                m_HeartArray[i].enabled = false;
             }
        }

            UpdateUI();
        


    }

}
