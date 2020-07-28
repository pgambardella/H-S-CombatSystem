using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    float m_idleTime;

    NavMeshAgent m_navmeshagentComponent;
    Animator m_animatorComponent;
    GameObject[] m_waypoints;
    int m_currentWaypoint;
    Transform m_playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        m_animatorComponent = GetComponent<Animator>();
        m_waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        m_currentWaypoint = 0;

        ResetIdleTime();
        m_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        m_navmeshagentComponent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        //update the value for Animator state change
        m_animatorComponent.SetFloat("DistanceFromPlayer", Vector3.Distance(transform.position, m_playerTransform.position));   
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_animatorComponent.SetBool("Attacking", true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_animatorComponent.SetBool("Attacking", false);
        }
    }

    public void CalculateWaypointDistance ()
    {
        if (DestinationReached())
        {
            SetNextWaypoint();
            ResetIdleTime();
            m_animatorComponent.SetTrigger("WaypointReached");
        }
    }

    void SetNextWaypoint()
    {
        m_currentWaypoint++;
        if (m_currentWaypoint > m_waypoints.Length - 1)
        {
            m_currentWaypoint = 0;
        }

        m_navmeshagentComponent.SetDestination(m_waypoints[m_currentWaypoint].transform.position);
    }

    public void ResetIdleTime()
    {
        m_animatorComponent.SetFloat("IdleTime", m_idleTime);
    }

    public void CalculateIdleRemainingTime()
    {
        float remainingTime = m_animatorComponent.GetFloat("IdleTime");
        m_animatorComponent.SetFloat("IdleTime", remainingTime - Time.deltaTime);
    }

    public void ChasePlayer()
    {
        m_navmeshagentComponent.SetDestination(m_playerTransform.position);
    }

    bool DestinationReached()
    {
        bool ret = false;
        if (!m_navmeshagentComponent.pathPending)
        {
            if (m_navmeshagentComponent.remainingDistance <= m_navmeshagentComponent.stoppingDistance)
            {
                if (!m_navmeshagentComponent.hasPath || m_navmeshagentComponent.velocity.sqrMagnitude == 0f)
                {
                    ret = true;
                }
            }
        }
        return ret;
    }
}
