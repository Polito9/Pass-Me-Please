using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JSONreader : MonoBehaviour
{    
    [System.Serializable]
    public class Question
    {
        public string preg;
        public string correcta;
        public string inc1;
        public string inc2;
        public string inc3;
    }
    [System.Serializable]
    public class QuestionList
    {
        public Question[] pregunta;
    }
   
}
