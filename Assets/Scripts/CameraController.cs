using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 m_cameraStartingPos;

    void Update()
    {
        Vector3 pos = player.position;
        pos.x += m_cameraStartingPos.x;
        pos.y += m_cameraStartingPos.y;
        pos.z += m_cameraStartingPos.z;
        transform.position = pos;

        transform.LookAt(player);
    }
}
