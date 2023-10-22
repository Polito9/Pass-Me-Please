using System.Collections;
using UnityEngine;
using TMPro;
using MySql.Data.MySqlClient;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static JSONMap;
using System;

public class Dialogo : MonoBehaviour
{
    [SerializeField] private GameObject PanelDeDialogo;
    [SerializeField] private TMP_Text Texto;
    [SerializeField] private GameObject textSalir;
    [SerializeField] private GameObject textInfo;
    private Animator animator;
    public TextAsset textJSON;
    public string sqlRow;
    public DialogList dialogList = new DialogList();
    private float typingTime = 0.01f;
    private bool isPlayerInRange;
    private bool didDialogueStart = false;
    private bool endDialogue = false;
    private int lineIndex;
    public bool mejora_speed;
    public bool mejora_salto;
    public bool mejora_pista;
    public bool mejorar = false;
    public bool npc_type = false;
    private string user;
    private int scene = 0;
    public GameObject player;
    public string next_scene;
    public GameObject villain;
    public int num_mapa;
    private bool aMorir = false;
    private bool muerto = false;
    private bool animado1 = false;
    private bool animado2 = false;
    private float audio;
    public sceneConsult cScene;
    void Start()
    {
        audio = float.Parse(PlayerPrefs.GetString("volumeSFX"));
        animator = GetComponent<Animator>();
        user = PlayerPrefs.GetString("user");
        dialogList = JsonUtility.FromJson<DialogList>(textJSON.text);
        if (!mejorar && !npc_type)
        {
            try
            {
                AdminMySQL adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
                string selecter = "SELECT " + sqlRow + " FROM `partida` WHERE `Id_jgdr` LIKE '" + user + "';";
                MySqlDataReader ResultadoSQL = adminMySQL.Select(selecter);
                ResultadoSQL.Read();
                scene = ResultadoSQL.GetInt32(0);
                ResultadoSQL.Close();
            }
            catch (MySqlException e)
            {
                Debug.Log(e);
            }
            if (scene == 2) //Desaparecemos el villano
            {
                muerto = true;
            }
        }
        else if (mejorar)
        {
            try
            {
                AdminMySQL adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
                if (adminMySQL != null)
                {
                    string selecter = "SELECT " + sqlRow + " FROM `mejora` WHERE `Id_jgdr` LIKE '" + user + "';";
                    MySqlDataReader ResultadoSQL = adminMySQL.Select(selecter);
                    ResultadoSQL.Read();
                    scene = ResultadoSQL.GetInt32(0);
                    ResultadoSQL.Close();
                }
                else
                {
                    Debug.Log("No jaló");
                }
            }
            catch (MySqlException e)
            {
                Debug.Log(e);
            }
            if (scene == 1) //Desaparecemos la mejora
            {
                villain.SetActive(false);
            }
        }
        else //Para los demás NPC 
        {
            int stat = 0;
            try
            {
                AdminMySQL adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
                
                string selecter = "SELECT * FROM `partida` WHERE `Id_jgdr` LIKE '" + user + "';";
                MySqlDataReader ResultadoSQL = adminMySQL.Select(selecter);
                ResultadoSQL.Read();
                for (int i = 0; i < 7; i++)
                {
                    stat = ResultadoSQL.GetInt32(i + 1);
                    if (stat == 2)
                    {
                        scene = i + 1;
                    }
                }
                ResultadoSQL.Close();
            }
            catch (MySqlException e)
            {
                Debug.Log(e);
            }
            //Guardamos la escena en las player prefs
            PlayerPrefs.SetInt("sceneDoors", scene);
        }
    }
    private void guardaryCargarBatalla(string scene)
    {
        try
        {
            string update = "";
            update += "UPDATE `Posicion` SET `x` = '" + player.transform.position.x + "', `y` = '" + player.transform.position.y + "' WHERE `id_jgdr` LIKE '" + user + "' AND `num_mapa` LIKE '" + num_mapa + "';";
            AdminMySQL adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
            MySqlDataReader Resultado = adminMySQL.Select(update);
            Resultado.Close();
            Transitions transition = GameObject.Find("TransitionScript").GetComponent<Transitions>();
            string inputData = "0.0/" + scene;
            transition.LoadScenewithTransitionMusic(inputData);
            Time.timeScale = 1f;
        }
        catch (MySqlException error)
        {
            Debug.Log(error.ToString());
        }
    }

    void Update()
    {
        if (muerto && !animado1)
        {
            animator.SetTrigger("Muerto");
            animado1 = true;
        }
        if (aMorir && !animado2)
        {
            animator.SetTrigger("Morir");
            animado2 = true;
        }


        if ((isPlayerInRange ) && (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Space)))
        {
            if (!didDialogueStart)
            {
                if (!mejorar && !npc_type)
                {
                    try
                    {
                        AdminMySQL adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
                        string selecter = "SELECT " + sqlRow + " FROM `partida` WHERE `id_jgdr` LIKE '" + user + "';";
                        MySqlDataReader Resultado = adminMySQL.Select(selecter);
                        Resultado.Read();
                        scene = Resultado.GetInt32(0);
                        Resultado.Close();
                    }
                    catch (MySqlException e)
                    {
                        Debug.Log(e);
                    }
                }
                StartDialogue();
            }
            else if (Texto.text == dialogList.dialogo[scene].dPersonaje[lineIndex])
            {
                if (dialogList.dialogo[scene].dPersonaje.Length - 1 > lineIndex)
                {
                    NextDialogueLine();
                }
                else
                {
                    textSalir.SetActive(true);
                    endDialogue = true;
                }
            }
        }
        if (endDialogue)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                string update = "";
                if (mejora_speed)
                {
                    mejorar = true;
                    update = "UPDATE `mejora` SET `mejora_velocidad` = '1' WHERE `id_jgdr` LIKE '" + user + "';";

                }
                else if (mejora_salto)
                {
                    mejorar = true;
                    update = "UPDATE `mejora` SET `mejora_saltarP` = '1' WHERE `id_jgdr` LIKE '" + user + "';";
                }
                else if (mejora_pista)
                {
                    mejorar = true;
                    update = "UPDATE `mejora` SET `mejora_pista` = '1' WHERE `id_jgdr` LIKE '" + user + "';";
                }

                if (mejorar)
                {
                    AdminMySQL adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
                    MySqlDataReader Resultado = adminMySQL.Select(update);
                    Resultado.Close();
                    villain.SetActive(false);
                }

                didDialogueStart = false;
                endDialogue = true;
                PanelDeDialogo.SetActive(false);
                textSalir.SetActive(false);
                Time.timeScale = 1f;
                lineIndex = 0;
                if (!mejorar && !npc_type)
                {
                    if (scene == 0)
                    {
                        guardaryCargarBatalla(next_scene);
                    }
                    else
                    {
                        try
                        {
                            aMorir = true;
                            string updates = "UPDATE `partida` SET `" + sqlRow + "` = '2' WHERE `id_jgdr` LIKE '" + user + "';";
                            AdminMySQL adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
                            MySqlDataReader Resultadod = adminMySQL.Select(updates);
                            Resultadod.Close();
                            Time.timeScale = 1f;
                        }
                        catch (MySqlException error)
                        {
                            Debug.Log(error.ToString());
                        }
                    }
                }
            }
        }
    }
    public void insert_inText(String valor, String text)
    {
        GameObject algo_go = GameObject.Find(valor);
        Text algo_in = algo_go.GetComponent<Text>();
        algo_in.text = text;
    }
    private void StartDialogue()
    {
        textInfo.SetActive(false);
        didDialogueStart = true;
        PanelDeDialogo.SetActive(true);
        lineIndex = 0;
        Time.timeScale = 0f;
        StartCoroutine(ShowLine());
    }
    private void NextDialogueLine()
    {
        lineIndex++;
        if (lineIndex < dialogList.dialogo[scene].dPersonaje.Length)
        {
            StartCoroutine(ShowLine());
        }
    }
    private IEnumerator ShowLine()
    {
        Texto.text = string.Empty;
        foreach (char ch in dialogList.dialogo[scene].dPersonaje[lineIndex])
        {
            Texto.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Jugador"))
        {
            isPlayerInRange = true;
            textInfo.SetActive(true);
            insert_inText("TextInfo", "Presiona R para hablar");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Jugador"))
        {
            isPlayerInRange = false;
            textInfo.SetActive(false);
        }
    }
}