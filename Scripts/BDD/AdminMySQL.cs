using MySql.Data.MySqlClient;
using System;
using Unity.VisualScripting;
using UnityEngine;
public class AdminMySQL : MonoBehaviour
{
    public string serverBD;
    public string nombreBD;
    public string usuarioBD;
    public string contraseniaBD;
    private string dataConnection;
    private MySqlConnection connection;
    void Start()
    {
        dataConnection = "Server=" + serverBD
                       + ";Database=" + nombreBD
                       + ";Uid=" + usuarioBD
                       + ";Pwd=" + contraseniaBD
                       + ";";
        ConectarServerBD();
    }
    private void ConectarServerBD()
    {
        connection = new MySqlConnection(dataConnection);
        try
        {
            connection.Open();
        }
        catch (MySqlException error)
        {
            Debug.Log(error.ToString());
        }
    }
    public MySqlDataReader Select(string _select)
    {

        MySqlCommand cmd = connection.CreateCommand();  
        cmd.CommandText = _select;
        MySqlDataReader Resultado = cmd.ExecuteReader();
        return Resultado;
    }

    public void closeConnection()
    {
        connection.Close();
    }

    public void justOpenConnection()
    {
        connection.Open();
    }
}