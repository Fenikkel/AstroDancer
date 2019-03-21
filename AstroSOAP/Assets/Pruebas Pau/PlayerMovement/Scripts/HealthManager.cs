using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int m_CurrentHealth;
    public int m_MaxHealth = 100;

    public float m_InvincibilityLength = 1f;
    private float m_InvincibilityCounter;

    private PlayerController m_ThePlayer;

    public Renderer m_PlayerRenderer; //para poder hacer parpadear el jugador

    private float m_FlashCounter;
    public float m_FlashLength = 0.1f; //tiempo entre parpadeo
   
    void Start()
    {
        m_CurrentHealth = m_MaxHealth;

        m_ThePlayer = FindObjectOfType<PlayerController>();
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
    }

    public void HurtPlayer(int damage, Vector3 knockBackDirection)
    {

        if (m_InvincibilityCounter <= 0) { //si no estamos en modo invencibilidad

            m_CurrentHealth -= damage;

            m_ThePlayer.KnockBack(knockBackDirection);

            m_InvincibilityCounter = m_InvincibilityLength;

            m_PlayerRenderer.enabled = false;

            m_FlashCounter = m_FlashLength;

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

}
