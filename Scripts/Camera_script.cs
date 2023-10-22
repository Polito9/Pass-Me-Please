using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_script : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
        Vector3 position = transform.position;
        position.x = player.transform.position.x;
        position.y = player.transform.position.y;
        transform.position = position;
    }
}
