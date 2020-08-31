using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    TopDown_Movement Mov;

    void Start()
    {
        Mov = GetComponent<TopDown_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Mov.up = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Mov.left = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Mov.right = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Mov.down = true;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            Mov.up = false;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Mov.left = false;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            Mov.right = false;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            Mov.down = false;
        }
    }
}
