using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float m_dps;
    [SerializeField]
    float m_attackRange;

    Animator m_animatorComponent;
    NavMeshAgent m_navmeshagentComponent;
    
    Vector3 destination;
    private void Start()
    {
        m_animatorComponent = GetComponent<Animator>();
        m_navmeshagentComponent = GetComponent<NavMeshAgent>();
        destination = transform.position;
    }

    void Update()
    {
        Debug.DrawLine(transform.position, destination, Color.red);
        m_navmeshagentComponent.SetDestination(destination);
        m_animatorComponent.SetBool("Quiet", DestinationReached());
    }

    public void Move(Vector3 newDest)
    {
        if (m_animatorComponent.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            m_animatorComponent.SetTrigger("StopAttack");
        }
       destination = newDest;
    }

    public void Attack(GameObject enemy)
    {
        if (Vector3.Distance(transform.position, enemy.transform.position) < m_attackRange)
        {
            m_animatorComponent.SetTrigger("StartAttack");
            destination = transform.position;
        } else
        {
            Move(enemy.transform.position);
        }
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
