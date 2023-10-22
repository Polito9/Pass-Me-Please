using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Partida : MonoBehaviour
{
    public GameObject pantallaPausa;
    public GameObject player;
    public int num_mapa;
    private string user;
    private void OnEnable()
    {
        user = PlayerPrefs.GetString("user");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    public void guardarPosicion(string scene)
    {
        string update = "";
        update += "UPDATE `Posicion` SET `x` = '" + player.transform.position.x+ "', `y` = '" + player.transform.position.y + "' WHERE `id_jgdr` LIKE '" + user+ "' AND `num_mapa` LIKE '"+num_mapa+"';";
        AdminMySQL adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
        MySqlDataReader Resultado = adminMySQL.Select(update);
        Debug.Log("Guardado exitoso de partida");
        Resultado.Close();
        SceneManager.LoadScene(scene);
        pantallaPausa.SetActive(false);
        Time.timeScale = 1f;
    }

    public void guardaPosMapasExtra(string pos)
    {
        char separator = ',';
        string[] parts = pos.Split(separator);
        float x_ = float.Parse(parts[0]);
        float y_ = float.Parse(parts[1]);
        string scene = parts[2];

        string update = "";
        update += "UPDATE `Posicion` SET `x` = '" + x_ + "', `y` = '" + y_ + "' WHERE `id_jgdr` LIKE '" + user + "';";
        AdminMySQL adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
        Debug.Log(update);
        MySqlDataReader Resultado = adminMySQL.Select(update);
        Resultado.Close();
        SceneManager.LoadScene(scene);
        pantallaPausa.SetActive(false);
        Time.timeScale = 1f;
    }
    public void borrarPartida()
    {
        string update = "";
        update += "DELETE FROM `partida` WHERE `partida`.`Id_jgdr` LIKE '"+user+"';";
        update += "DELETE FROM `posicion` WHERE `posicion`.`Id_jgdr` LIKE '"+user+"';";
        update += "DELETE FROM `mejora` WHERE `mejora`.`Id_jgdr` LIKE '" + user + "';";
        update += "INSERT INTO `partida` (`num_partida`, `d1`, `batHist`, `batBio`, `batMate`, `batFisic`, `batMusic`,`batQuim`, `id_jgdr`) VALUES ('default', b'0', b'0', b'0', b'0', b'0', b'0', b'0', '" + user + "'); ";
        update += "INSERT INTO `posicion` (`id_mapa` ,`num_mapa`, `x`, `y`, `Id_jgdr`) VALUES ('default' ,'1', '0', '0', '" + user + "' );";
        update += "INSERT INTO `mejora` (`mejora_pista`, `mejora_saltarP`, `mejora_velocidad`, `Id_jgdr`) VALUES (b'0', b'0', b'0', '" + user + "' );";
        AdminMySQL adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
        Debug.Log(update);
        MySqlDataReader Resultado = adminMySQL.Select(update);
        Resultado.Close();
        adminMySQL.closeConnection();
        SceneManager.LoadScene("Map1");
    }
    void OnDisable()
    {

    }
}
