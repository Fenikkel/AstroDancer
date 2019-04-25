using System;
using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text m_GoldText;
    public int m_CurrentGold = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_GoldText.text = "Gold: " + m_CurrentGold;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddGold(int goldToAdd)
    {
        m_CurrentGold += goldToAdd;
        m_GoldText.text = "Gold: " + m_CurrentGold;

    }
}
