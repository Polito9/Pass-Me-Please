using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class selectButton : MonoBehaviour
{   
    [SerializeField] private AudioClip _hoverClip;
    [SerializeField] private AudioClip _SelectedClip;
    [SerializeField] private AudioClip _Selected2Clip;
    [SerializeField] private AudioSource _audioSource;
    void Start()
    {
        if (_audioSource == null)
        {
            Debug.Log("The audio is null");
        }
    }

    public void hoverSound()
    {
        _audioSource.PlayOneShot(_hoverClip);
    }
    public void selectedSound()
    {
        _audioSource.PlayOneShot(_SelectedClip);
    }
    public void selectedImportantSound()
    {
        _audioSource.PlayOneShot(_Selected2Clip);
    }
}