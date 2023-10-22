using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC_camFollowing : MonoBehaviour
{

    public GameObject player;
    private float distance, pendiente, dist_x, dist_y;
    private float x1, x2, y1, y2;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    { 
        animator = GetComponent<Animator>();
        string btn_interact = PlayerPrefs.GetString("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        //Calculamos la distancia 
        x1 = transform.position.x;
        x2 = player.transform.position.x;
        y1 = transform.position.y;
        y2 = player.transform.position.y;
        distance = Mathf.Sqrt((Mathf.Pow((x1 - x2), 2))+(Mathf.Pow((y1 - y2), 2))); //Equation to calculate the distance between 2 points 
        dist_x = x2 - x1;
        dist_y = y2 - y1;
        
        if (distance < 2.0f)
        {
            pendiente = (y2 - y1) / (x2 - x1);
            //Debug.Log(pendiente);
            if (pendiente > -1 && pendiente < 1) //1 lados
            {
                animator.SetInteger("direction", 1);
            }
            else if ((pendiente < -1 || pendiente > 1) && dist_y < 0) //2 abajo
            {
                animator.SetInteger("direction", 2);
            }
            else if ((pendiente < -1 || pendiente > 1) && dist_x > 0) //3 arriba
            {
                animator.SetInteger("direction", 3);
            }
        }
        if (dist_x < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (dist_x > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

    }
}
