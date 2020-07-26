using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    float m_idleTime;
    
    Animator m_animatorComponent;
    GameObject[] m_waypoints;
    int m_currentWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        m_animatorComponent = GetComponent<Animator>();
        m_waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        Debug.Log("Enemy has " + m_waypoints.Length + " waypoints");
        m_currentWaypoint = 0;

        m_animatorComponent.SetFloat("IdleTime", m_idleTime);
    }

    // Update is called once per frame
    void Update()
    {
        

        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        m_animatorComponent.SetFloat("DistanceFromPlayer", Vector3.Distance(transform.position,playerPosition));

        if (m_animatorComponent.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            float currentTime = m_animatorComponent.GetFloat("IdleTime");
            m_animatorComponent.SetFloat("IdleTime", currentTime - Time.deltaTime);
        }
        
        else if (m_animatorComponent.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
        {
            GetComponent<NavMeshAgent>().SetDestination(playerPosition);
        } 
        else if (m_animatorComponent.GetCurrentAnimatorStateInfo(0).IsName("Patrol"))
        {
            GetComponent<NavMeshAgent>().SetDestination(m_waypoints[m_currentWaypoint].transform.position);
        }
        else
        {
            GetComponent<NavMeshAgent>().SetDestination(transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Animator>().SetTrigger("PlayerIsNear");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Animator>().SetTrigger("PlayerIsFar");
        }
    }

    public void SetNextWaypoint()
    {
        m_currentWaypoint++;
        if (m_currentWaypoint > m_waypoints.Length - 1)
        {
            m_currentWaypoint = 0;
        }
    }
}
