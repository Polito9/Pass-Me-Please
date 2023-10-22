using MySql.Data.MySqlClient;
using System;
using UnityEngine;

using UnityEngine.UI;
public class Conexiones_escenas : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private bool isPlayerInRange;
    [SerializeField] private GameObject textInfo;
    [SerializeField] private string nextScene;
    [SerializeField] private int num_mapa;
    private string user;
    // Start is called before the first frame update
    void Start()
    {
        user = PlayerPrefs.GetString("user");
    }

    public void insert_inText(String valor, String text)
    {
        GameObject algo_go = GameObject.Find(valor);
        Text algo_in = algo_go.GetComponent<Text>();
        algo_in.text = text;
    }
    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange && Input.GetKey(KeyCode.R))
        {
            guardaryCargarMapa(nextScene);
        }
    }
    private void guardaryCargarMapa(string scene)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Jugador"))
        {
            isPlayerInRange = true;
            textInfo.SetActive(true);
            insert_inText("TextInfo", "Presiona R para salir del lugar");
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