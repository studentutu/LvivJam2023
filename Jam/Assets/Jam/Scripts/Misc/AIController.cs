using System;
using System.Collections;
using System.Collections.Generic;
using Jam.Scripts.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent; //  Nav mesh agent component
    public float startWaitTime = 4; //  Wait time of every action
    public float speedWalk = 6; //  Walking speed, speed in the nav mesh agent

    int m_CurrentWaypointIndex; //  Current waypoint where the enemy is going to

    float m_WaitTime; //  Variable of the wait time that makes the delay
    bool m_IsPatrol; //  If the enemy is patrol, state of patroling

    void Start()
    {
        m_IsPatrol = true;
        m_WaitTime = startWaitTime; //  Set the wait time variable that will change

        m_CurrentWaypointIndex = 0; //  Set the initial waypoint

        navMeshAgent.isStopped = true;
    }

    private Waypoints _waypoints;

    private void OnEnable()
    {
        MessageBroker.Default.Buffered().ReceiveBuffered<Waypoints>(x =>
        {
            _waypoints = x;

            navMeshAgent.isStopped = false;
            navMeshAgent.speed = speedWalk; //  Set the navemesh speed with the normal speed of the enemy
            navMeshAgent.SetDestination(_waypoints.waypoints[m_CurrentWaypointIndex]
                .position); //  Set the destination to the first waypoint
        }).Dispose();
    }

    private void Update()
    {
        if (_waypoints == null)
            return;

        Patroling();
    }

    private void Patroling()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            m_WaitTime -= Time.deltaTime;
            //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
            if (m_WaitTime <= 0)
            {
                NextPoint();
                m_WaitTime = startWaitTime;
            }
        }
    }

    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % _waypoints.waypoints.Length;
        navMeshAgent.SetDestination(_waypoints.waypoints[m_CurrentWaypointIndex].position);
    }
}