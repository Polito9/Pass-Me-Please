using MySql.Data.MySqlClient;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Configs : MonoBehaviour
{
    private GameObject pantallaError;
    private GameObject canvas;
    public InputField preguntas;
    private string[] configs_names = { "preguntas", "audioBGM", "audioSFX"};
    public Slider sliderBGM;
    public Slider sliderSFX;
    private float sliderValue;
    private string user;
    private string[] player_prefs_config_names = {"max_preg", "volume" };
    void OnEnable()
    {
        user = PlayerPrefs.GetString("user");
    }

    public void insertText(String valor)
    {
        GameObject algo_go = GameObject.Find(valor);
        InputField algo_in = algo_go.GetComponent<InputField>();
        algo_in.text = PlayerPrefs.GetString(valor);
    }
    void Start()
    {
        pantallaError = GameObject.Find("MessagePanel");
        canvas = GameObject.Find("Canvas");
        try
        {
            pantallaError.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        insertText(player_prefs_config_names[0]);
        sliderBGM.value = float.Parse(PlayerPrefs.GetString("volumeBGM"));
        sliderSFX.value = float.Parse(PlayerPrefs.GetString("volumeSFX"));
    }

    public void insert_inText(String valor, String text)
    {
        GameObject algo_go = GameObject.Find(valor);
        Text algo_in = algo_go.GetComponent<Text>();
        algo_in.text = text;
    }
    public void actualizaConfigs()
    {
        string update = "";
        int num_preg = int.Parse(preguntas.text);
        if (num_preg <= 10)
        {
            string[] configs_values = { preguntas.text, sliderBGM.value.ToString(), sliderSFX.value.ToString() };
            for (int i = 0; i < configs_names.Length; i++)
            {
                update += "UPDATE `configuraciones` SET `config` = '" + configs_values[i] + "' WHERE `indicador` LIKE '" + configs_names[i] + "' AND `Id_jgdr` LIKE '" + user + "';";
            }
            AdminMySQL adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
            MySqlDataReader Resultado = adminMySQL.Select(update);
            Debug.Log("Guardado exitoso");
            Resultado.Close();
            SceneManager.LoadScene("Menú");
        }
        else
        {
            canvas.SetActive(false);
            pantallaError.SetActive(true);
            insert_inText("message", "El valor máximo de preguntas es 10");
        }
    }
    // Update is called once per frame
    void Update()
    {
    }

    void OnDisable()
    {
        string[] configs_values = {preguntas.text, sliderBGM.value.ToString(), sliderSFX.value.ToString() };
        PlayerPrefs.SetString("max_preg", configs_values[0]);
        PlayerPrefs.SetString("volumeBGM", configs_values[1]);
        PlayerPrefs.SetString("volumeSFX", configs_values[2]);
    }
}