using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class MusicController : MonoBehaviour
{
    private float audio = 0.0f;
    [SerializeField] private AudioClip back1;
    [SerializeField] private AudioClip back2;
    [SerializeField] private AudioClip back3;
    [SerializeField] private AudioClip back4;
    [SerializeField] private AudioClip back5;
    [SerializeField] private AudioClip back6;
    [SerializeField] private AudioClip back7;
    [SerializeField] private AudioClip back8;
    [SerializeField] private AudioClip back9;
    [SerializeField] private AudioClip back10;
    [SerializeField] private AudioClip back11;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private bool exploracion;
    void Start()
    {
        audio = float.Parse(PlayerPrefs.GetString("volumeBGM"));
        if (_audioSource == null)
        {
            Debug.Log("The audio is null");
        }
        if (exploracion)
        {
            var randomGenerator = new Random();
            var random1 = randomGenerator.Next(1, 12);
            AudioClip[] backClips = { back1, back2, back3, back4, back5, back6, back7, back8, back9, back10, back11};
            if (random1 >= 1 && random1 <= backClips.Length)
            {
                _audioSource.clip = backClips[random1-1];
                Debug.Log("Es " + random1);
            }
            else
            {
                Debug.Log("Index out of range");
            }
        }
        else
        {
            _audioSource.clip = back1;
        }
        _audioSource.Play();
        _audioSource.volume = audio;
        Debug.Log("El audio es "+ _audioSource.volume);
    }
    void Update()
    {
        
    }
}
