using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static JSONreader;
using MySql.Data.MySqlClient;
public class Manager : MonoBehaviour
{
    private int mejoraSaltar;
    private int mejoraPista;
    private GameObject panelMejSalta;
    private GameObject panelMejPista;
    public Image imageIncorrect;
    public GameObject end;
    public GameObject wrong;
    public TextAsset textJSON;
    public RectTransform rectC1;
    public RectTransform recti1;
    public RectTransform recti2;
    public RectTransform recti3;
    public QuestionList myQuestionList = new QuestionList();
    public string sqlRow;
    Vector2 posIn1 = new Vector3();
    Vector2 posIn2 = new Vector3();
    Vector2 posIn3 = new Vector3();
    Vector2 posIn4 = new Vector3();
    Animacioncitas villan_anim;
    AdminMySQL adminMySQL;
    int num_pregunta = 0;
    private int max_num_pregunta;
    int[] array = new int[] { 0, 1, 2, 3 };
    int[] randomizerArray_preg;
    private string user;
    private bool mostrado = true;
    private float audio;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioSource _audioSource;
    public void Start()
    {
        audio = float.Parse(PlayerPrefs.GetString("volumeSFX"));
        //Para el audio
        if (_audioSource == null)
        {
            Debug.Log("The audio is null");
        }
        else
        {
            _audioSource.volume = audio;
        }
        max_num_pregunta = int.Parse(PlayerPrefs.GetString("max_preg"));
        user = PlayerPrefs.GetString("user");
        adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
        villan_anim = GetComponent<Animacioncitas>();
        myQuestionList = JsonUtility.FromJson<QuestionList>(textJSON.text);
        //Para generar el array que se va a randomizar con el número de preguntas del JSON
        int[] num_pregunta_array = new int[myQuestionList.pregunta.Length];
        for (int i = 0; i < num_pregunta_array.Length; i++)
        {
            num_pregunta_array[i] = i;
        }
        randomizerArray_preg = RandomizeArray(num_pregunta_array);
        posIn1 = rectC1.anchoredPosition;
        posIn2 = recti1.anchoredPosition;
        posIn3 = recti2.anchoredPosition;
        posIn4 = recti3.anchoredPosition;
        //Debug.Log(max_num_pregunta);
        //Para la primera carga
        setValues(randomizerArray_preg[num_pregunta]);
    }
    private void setValues(int i)
    {
        if (max_num_pregunta > num_pregunta)
        {
            Vector3[] positions = new Vector3[] { posIn1, posIn2, posIn3, posIn4 };
            int[] randomizerArray = RandomizeArray(array);
            rectC1.anchoredPosition = positions[randomizerArray[0]];
            recti1.anchoredPosition = positions[randomizerArray[1]];
            recti2.anchoredPosition = positions[randomizerArray[2]];
            recti3.anchoredPosition = positions[randomizerArray[3]];
            //Insertamos la información
            insert_inText("question", myQuestionList.pregunta[i].preg);
            insert_inText("correct", myQuestionList.pregunta[i].correcta);
            insert_inText("i1", myQuestionList.pregunta[i].inc1);
            insert_inText("i2", myQuestionList.pregunta[i].inc2);
            insert_inText("i3", myQuestionList.pregunta[i].inc3);
        }
        else
        {
            //Hacemos el guardado en la base de datos
            adminMySQL.justOpenConnection();
            string update = "UPDATE `partida` SET `" + sqlRow + "` = '1' WHERE `id_jgdr` LIKE '" + user + "';";
            MySqlDataReader reader = adminMySQL.Select(update);
            reader.Close();
            villan_anim.girarYSalir();
            end.SetActive(true);
        }
    }
    public int[] RandomizeArray(int[] array)
    {
        System.Random rnd = new System.Random();

        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            int temp = array[k];
            array[k] = array[n];
            array[n] = temp;
        }
        return array;
    }
    public void insert_inText(String valor, String text)
    {
        GameObject algo_go = GameObject.Find(valor);
        Text algo_in = algo_go.GetComponent<Text>();
        algo_in.text = text;
    }
    private void Update()
    {
        if (mostrado)
        {
            setPanelMejora();
        }
        villan_anim.moverInfinito();
    }

    private void setPanelMejora()
    {
        //Para las mejoras
        panelMejSalta = GameObject.Find("MejoraSalta");
        panelMejPista = GameObject.Find("MejoraPista");
        string selecter = "SELECT * FROM `mejora` WHERE `id_jgdr` LIKE '" + user + "';";
        MySqlDataReader Resultado = adminMySQL.Select(selecter);
        Resultado.Read();
        mejoraPista = Resultado.GetInt32(0);
        mejoraSaltar = Resultado.GetInt32(1);
        Resultado.Close();
        adminMySQL.closeConnection();
        if (mejoraSaltar == 0)
            panelMejSalta.SetActive(false);
        if (mejoraPista == 0)
            panelMejPista.SetActive(false);
        mostrado = false;
    }
    public void correct()
    {
        _audioSource.PlayOneShot(_audioClip);
        villan_anim.damaged_anim();
        //Para las siguientes cargas de preguntas
        num_pregunta++;
        setValues(randomizerArray_preg[num_pregunta]);
        imageIncorrect.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
    public void incorrect()
    {
        wrong.SetActive(true);
    }

    public void saltar()
    {
        correct();
        panelMejSalta.SetActive(false);
    }
    public void pista()
    {
        imageIncorrect.GetComponent<Image>().color = new Color32(255, 136, 136, 255);
        panelMejPista.SetActive(false);
    }
}   