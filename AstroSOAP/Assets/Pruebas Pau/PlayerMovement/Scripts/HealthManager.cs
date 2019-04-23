using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthManager : MonoBehaviour
{
    public int m_CurrentHealth;
    public int m_MaxHealth = 100;

    public float m_InvincibilityLength = 1f;
    private float m_InvincibilityCounter;

    public PlayerController m_ThePlayer;
    public PlatformGenerator m_PlatformGenerator;

    public Renderer m_PlayerRenderer; //para poder hacer parpadear el jugador

    private float m_FlashCounter;
    public float m_FlashLength = 0.1f; //tiempo entre parpadeo

    private bool m_IsRespawning = false;
    public Vector3 m_RespawnPoint;
    public float m_RespawnLength = 2;

    public GameObject m_DeathEffect;

    public Image m_BlackScreen;
    private bool m_IsFadeToBlack;
    private bool m_IsFadeFromBlack;
    public float m_FadeSpeed = 2f; //Para que vaya bien, fade speed tiene que ser mayor que wait for fade
    public float m_WaitForFade = 1f;


    void Start()
    {
        m_CurrentHealth = m_MaxHealth;

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

    public void HurtPlayer(int damage, Vector3 knockBackDirection)
    {

        if (m_InvincibilityCounter <= 0) { //si no estamos en modo invencibilidad

            m_CurrentHealth -= damage;

            if (m_CurrentHealth <= 0)
            {
                Respawn();
                
            }
            else
            {
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

        if(m_CurrentHealth> m_MaxHealth){

            m_CurrentHealth = m_MaxHealth;
        }
    }


    public void Respawn()
    {
        if (!m_IsRespawning) //si no estamos respawneando
        {
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

        m_ThePlayer.gameObject.SetActive(true);
        StopScene(false);
        //respawneamos el jugador
        m_ThePlayer.transform.position = m_RespawnPoint;
        //Debug.Log(m_RespawnPoint);
        //Debug.Log("m_Player " + m_ThePlayer.transform.position);
        m_CurrentHealth = m_MaxHealth;

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
}
