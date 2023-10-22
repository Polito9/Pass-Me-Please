using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static FadeAudioSource;
public class Transitions : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private GameObject panelTransition;
    [SerializeField] private AudioSource _audioSrc;
    private void Start()
    {
        _animator = panelTransition.GetComponent<Animator>();
    }

    public void LoadScenewithTransition(string sceneName)
    {
        StartCoroutine(transitionTo(sceneName));
    }

    public void LoadScenewithTransitionMusic(string data)
    {
        //Input .66/Mapita
        char separator = '/';
        int separatorIndex = data.IndexOf(separator);
        float targetVolume = float.Parse(data.Substring(0, separatorIndex));
        string sceneName = data.Substring(separatorIndex + 1);
        //Hacemos las corrutinas
        StartCoroutine(FadeAudioSource.StartFade(_audioSrc, 1.0f, targetVolume));
        StartCoroutine(transitionTo(sceneName));
    }

    IEnumerator transitionTo(string scene)
    {
        _animator.SetTrigger("salida");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
    }
}
