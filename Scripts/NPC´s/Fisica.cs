using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fisica : MonoBehaviour
{
    public GameObject player;
    private float x1, x2, y1, y2;
    private float distance;
    private string user;
    Vector2 pos_inicial;

    // Start is called before the first frame update
    void Start()
    {
        user = PlayerPrefs.GetString("user");
    }

    // Update is called once per frame
    void Update()
    {
        x1 = transform.position.x;
        x2 = player.transform.position.x;
        y1 = transform.position.y;
        y2 = player.transform.position.y;
        distance = Mathf.Sqrt((Mathf.Pow((x1 - x2), 2)) + (Mathf.Pow((y1 - y2), 2))); //Equation to calculate the distance between 2 points 
        if (distance < 1.0f)
        {
            //Para la interacción de batalla
            if (Input.GetKey(KeyCode.E))
            {
                guardarPosicion("batalla_mate");
            }
        }
    }
    public void guardarPosicion(string scene)
    {
        string update = "";
        update += "UPDATE `partida` SET `x` = '" + player.transform.position.x + "', `y` = '" + player.transform.position.y + "' WHERE `id_jgdr` LIKE '" + user + "';";
        Debug.Log(update);
        AdminMySQL adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
        MySqlDataReader Resultado = adminMySQL.Select(update);
        Debug.Log("Guardado exitoso de partida");
        Resultado.Close();
        SceneManager.LoadScene(scene);
        Time.timeScale = 1f;
    }
}