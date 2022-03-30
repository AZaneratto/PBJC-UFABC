using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageBotoes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMundoGame()
    {
        SceneManager.LoadScene("Intro");
        
    }

    public void RestartMundoGame()
    {
        SceneManager.LoadScene("Intro");

    }

    public void CreditoMundoGame()
    {
        SceneManager.LoadScene("Creditos");

    }

    public void MenuMundoGame()
    {
        SceneManager.LoadScene("Pag1");
    }
}
