using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneConsult : ScriptableObject
{
    public int scene;


    public void setScene(AdminMySQL adminMySQL, string user)
    {
        int stat = 0;
        adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
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
}
