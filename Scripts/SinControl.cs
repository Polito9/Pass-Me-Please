using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SinControl : MonoBehaviour
{ 
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); 
    }
    public void cerrarJuego()
    {
        Application.Quit();
    }
}