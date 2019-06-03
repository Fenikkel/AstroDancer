using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFX : MonoBehaviour
{
    public AudioSource m_MyFX;
    public AudioClip m_HoverFX;
    public AudioClip m_ClickFX;


    public void HoverSound()
    {
        m_MyFX.PlayOneShot(m_HoverFX);
    }

    public void ClickSound()
    {
        m_MyFX.PlayOneShot(m_ClickFX);
    }

}
