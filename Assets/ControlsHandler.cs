using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsHandler : MonoBehaviour
{
    PlayerController m_playerController;
    // Start is called before the first frame update
    void Start()
    {
        m_playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            //cast the ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                //if you clicked on an enemy
                if (hitInfo.transform.tag == "Enemy")
                {
                    if (m_playerController)
                    {
                       m_playerController.Attack(hitInfo.transform.gameObject);
                    }
                }
                else
                {
                    if (m_playerController)
                    {
                        m_playerController.Move(hitInfo.point);
                    }
                }
            }
        }
    }
}
