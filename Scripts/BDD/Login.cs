using MySql.Data.MySqlClient;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Login : MonoBehaviour
{
    public InputField usuarioTXT;
    public InputField passTXT;
    public InputField nameTXT;
    private GameObject pantallaError;
    private GameObject canvas;
    private string[] configs_names = {"audioBGM", "audioSFX", "preguntas" };
    private string[] configs_values = {"50", "50", "5"};
    private string[] configs_guardadas = new string [3];
    public string user;
    // Create a Regex object with the pattern
    Regex regex = new Regex("^[a-zA-Z0-9]{3,10}$");
    private void Start()
    {
        pantallaError = GameObject.Find("MessagePanel");
        canvas = GameObject.Find("Canvas");
        try
        {
            pantallaError.SetActive(false);
        }catch(Exception e)
        {
            Debug.LogError(e);
        }
    }
    public void logear()
    {
        if (regex.IsMatch(usuarioTXT.text) && regex.IsMatch(passTXT.text))
        {
            try
            {
                string _log = "SELECT * FROM `jugador` WHERE `Id_jgdr` LIKE '" + usuarioTXT.text + "' AND `contra_jgdr` LIKE '" + passTXT.text + "'";
                AdminMySQL adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
                MySqlDataReader Resultado = adminMySQL.Select(_log);

                if (Resultado.HasRows)
                {
                    user = usuarioTXT.text;
                    Debug.Log("Login correcto");
                    Resultado.Close();
                    //Para la parte de que se guarden en las PlayerPrefs los valores de la tabla configuraciones ya guardados
                    for (int i = 0; i < configs_names.Length; i++)
                    {

                        string _configs = "SELECT config FROM `configuraciones` WHERE `Id_jgdr` LIKE '" + usuarioTXT.text + "' AND `indicador` LIKE '" + configs_names[i] + "'";
                        MySqlDataReader Resultado2 = adminMySQL.Select(_configs);
                        while (Resultado2.Read())
                        {
                            configs_guardadas[i] = Resultado2.GetString(0);
                        }
                        Resultado2.Close();
                    }
                    SceneManager.LoadScene("Menú");
                }
                else
                {
                    canvas.SetActive(false);
                    pantallaError.SetActive(true);
                    insert_inText("message", "Usuario o contraseña incorrectos");
                    Resultado.Close();
                }
            }
            catch(MySqlException e)
            {
                Debug.LogException(e);
            }
        }
        else
        {
            canvas.SetActive(false);
            pantallaError.SetActive(true);
            insert_inText("message", "Ingresa un mínimo de 3 caracteres y un máximo de 10, sin usar caracteres especiales");
        }

    }
    public void registrar()
    {
        try
        {
            if (regex.IsMatch(usuarioTXT.text) && regex.IsMatch(passTXT.text) && regex.IsMatch(nameTXT.text))
            {
                string _reg = "INSERT INTO `jugador` (`id_jgdr`, `contra_jgdr`, `nombre`) VALUES ('" + usuarioTXT.text + "', '" + passTXT.text + "', '" + nameTXT.text + "');";
                for (int i = 0; i < configs_names.Length; i++)
                {
                    _reg += "INSERT INTO `configuraciones` (`Id_config`, `indicador` , `config`, `Id_jgdr`) VALUES ('default','" + configs_names[i] + "', '" + configs_values[i] + "', '" + usuarioTXT.text + "');";
                }
                _reg += "INSERT INTO `partida` (`num_partida`, `d1`, `batHist`, `batBio`, `batMate`, `batFisic`, `batMusic`,`batQuim`, `id_jgdr`) VALUES ('default', b'0', b'0', b'0', b'0', b'0', b'0', b'0', '" + usuarioTXT.text + "' );";
                _reg += "INSERT INTO `mejora` (`mejora_pista`, `mejora_saltarP`, `mejora_velocidad`, `Id_jgdr`) VALUES (b'0', b'0', b'0', '" + usuarioTXT.text + "' );";
                _reg += "INSERT INTO `Posicion` (`id_mapa` ,`num_mapa`, `x`, `y`, `Id_jgdr`) VALUES ('default' ,'1', '0', '0', '" + usuarioTXT.text + "' );";
                AdminMySQL adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
                MySqlDataReader cm1 = adminMySQL.Select("SELECT id_jgdr FROM `jugador` WHERE `Id_jgdr` LIKE '"+usuarioTXT.text+"'");
                if (cm1.Read())
                {
                    canvas.SetActive(false);
                    pantallaError.SetActive(true);
                    insert_inText("message", "Usuario existente");
                    cm1.Close();
                    adminMySQL.closeConnection();
                }
                else
                {
                    cm1.Close();
                    adminMySQL.closeConnection();
                    adminMySQL.justOpenConnection();
                    MySqlDataReader resultado = adminMySQL.Select(_reg);
                    Debug.Log("Registro exitoso");
                    resultado.Close();
                    SceneManager.LoadScene("IniciarSesion");
                }
            }
            else
            {
                canvas.SetActive(false);
                pantallaError.SetActive(true);
                insert_inText("message", "Ingresa un mínimo de 3 caracteres y un máximo de 10, sin usar caracteres especiales");
            }
        }
        catch(MySqlException e)
        {
            Debug.Log(e);
        }
        
    }
    void OnDisable()
    {
        PlayerPrefs.SetString("user", user);
        PlayerPrefs.SetString("volumeBGM", configs_guardadas[0]);
        PlayerPrefs.SetString("volumeSFX", configs_guardadas[1]);
        PlayerPrefs.SetString("max_preg", configs_guardadas[2]);
        PlayerPrefs.SetFloat("xm1", 0.0f);
        PlayerPrefs.SetFloat("ym1", 0.0f);
        PlayerPrefs.SetFloat("speed", 0.8f);
        PlayerPrefs.SetInt("mejoraSalta", 0);
        PlayerPrefs.SetInt("mejoraPista", 0);
    }
    public void insert_inText(String valor, String text)
    {
        GameObject algo_go = GameObject.Find(valor);
        Text algo_in = algo_go.GetComponent<Text>();
        algo_in.text = text;
    }
}