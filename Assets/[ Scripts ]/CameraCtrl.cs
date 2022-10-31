using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public float camera_move_speed = 10f;
    public float RTS_screen_offset = 10;

    public float minPosX = -5;
    public float maxPosX = 5;


    public float minPosY = -5;
    public float maxPosY = 5;


    public float min_max_PosY = 52;
    public float min_max_PosZ = -20;


    Vector3 cameraPos;
    Vector3 mousePos;

    void Start()
    {
        min_max_PosY = transform.position.y;
        min_max_PosZ = transform.position.z;
    }

    void Update()
    {
        cameraPos = transform.position;


        // X Axis
        if (Input.mousePosition.x >= Screen.width - RTS_screen_offset)
        {
            cameraPos.x += camera_move_speed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= RTS_screen_offset)
        {
            cameraPos.x -= camera_move_speed * Time.deltaTime;
        }

        // Y Axis
        if (Input.mousePosition.y >= Screen.height - RTS_screen_offset)
        {
            cameraPos.y += camera_move_speed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= RTS_screen_offset)
        {
            cameraPos.y -= camera_move_speed * Time.deltaTime;
        }

        cameraPos.x = Mathf.Clamp(cameraPos.x, minPosX, maxPosX);
        cameraPos.y = Mathf.Clamp(cameraPos.y, minPosY, maxPosY);

        //cameraPos.y = Mathf.Clamp(cameraPos.y, min_max_PosY, min_max_PosY);
        //cameraPos.z = Mathf.Clamp(cameraPos.z, min_max_PosZ, min_max_PosZ);

        transform.position = Vector3.Lerp(transform.position, cameraPos, camera_move_speed);
    }
}