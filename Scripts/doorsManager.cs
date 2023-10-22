using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class doorsManager : MonoBehaviour
{
    private int sceneDoors;
    [SerializeField] private GameObject outQ;
    [SerializeField] private GameObject outMu;
    [SerializeField] private GameObject outB;
    [SerializeField] private GameObject outMat;
    [SerializeField] private GameObject outH;
    [SerializeField] private GameObject outF;
    [SerializeField] private GameObject outFinal;
    void Start()
    {
        try
        {
            //Creamos un array de GameObjects para irlos mostrando de acuerdo a el número de escena
            GameObject[] outs = { outFinal, outF, outH, outMat, outB, outMu, outQ };
            sceneDoors = PlayerPrefs.GetInt("sceneDoors");
            if (sceneDoors == 0)
            {
                sceneDoors = 1;
            }
            int aux = 7 - sceneDoors;
            for (int i = 0; i < aux; i++)
            {
                outs[i].SetActive(false);
            }
        }catch(MissingReferenceException e)
        {
            Debug.LogException(e);
        }
    }
}

/*
 0 -->batQ 
 1 -->batMusic
 2
 
 */
