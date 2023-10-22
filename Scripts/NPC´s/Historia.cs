using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Historia : MonoBehaviour
{
    public GameObject player;
    private float x1, x2, y1, y2;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        
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
                SceneManager.LoadScene("Batalla_hist");
            }
        }
    }
}
