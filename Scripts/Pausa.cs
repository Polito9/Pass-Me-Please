using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausa : MonoBehaviour
{
    [SerializeField] private GameObject PantallaPausa;
    private bool isPaused;
    [SerializeField] private AudioSource _audioSource;
    public static Pausa Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UpdateGameState();
            ShowPausePanel();
            checkAudio();
        }
    }
    public void UpdateGameState()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    private void ShowPausePanel()
    {
        if (isPaused)
        {
            PantallaPausa.SetActive(true);
        }
        else
        {
            PantallaPausa.SetActive(false);
        }
    }
    private void checkAudio()
    {
        if (isPaused)
        {
            _audioSource.Stop();
        }
        else
        {
            _audioSource.Play();
        }
    }
    public bool IsGamePaused()
    {
        return isPaused;
    }
    public void Reanudar()
    {
        UpdateGameState();
        ShowPausePanel();
        checkAudio();
    }
    public void HaciaConfig()
    {
        SceneManager.LoadScene("Configuracion");
        UpdateGameState();
        ShowPausePanel();
    }
    public void HaciaMenu()
    {
        SceneManager.LoadScene("Menú");
        UpdateGameState();
        ShowPausePanel();
    }
}
