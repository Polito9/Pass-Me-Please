using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class aux_NPC : MonoBehaviour
{
    public GameObject player;
    public string next_scene;
    private string user;

    // Start is called before the first frame update
    void Start()
    {
        user = PlayerPrefs.GetString("user");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}