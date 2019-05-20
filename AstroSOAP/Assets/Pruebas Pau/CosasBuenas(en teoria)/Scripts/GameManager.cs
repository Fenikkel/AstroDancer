using System;
using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Text m_DebuggGoldText;
    public Text m_GoldText;
    public TMP_Text m_CollectibleText;


    public int m_CurrentGold = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_DebuggGoldText.text = "Gold: " + m_CurrentGold;
        m_CollectibleText.text = m_CurrentGold + "/?";

        m_GoldText.text = m_CurrentGold.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddGold(int goldToAdd)
    {
        m_CurrentGold += goldToAdd;
        m_DebuggGoldText.text = "Gold: " + m_CurrentGold;
        m_CollectibleText.text = m_CurrentGold + "/?";

        m_GoldText.text = m_CurrentGold.ToString();


    }
}
