using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONMap : MonoBehaviour
{
    [System.Serializable]
    public class DialogoClass
    {
        public string[] dPersonaje;
        public string[] dVillano;
        public string[] dSystem;
        public string[] dNarrador;
        public string[] dNPC1;
    }
    [System.Serializable]
    public class DialogList
    {
        public DialogoClass[] dialogo;
    }
}
