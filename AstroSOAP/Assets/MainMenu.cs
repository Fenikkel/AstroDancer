using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //activa la escena que tenga el indice siguiente a esta
    }


    public void Quit()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

}
