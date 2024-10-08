using System;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    public void Initialize()
    {
        agent.Warp(CustomerManager.Instance.GetSpawnPoint().position);
        agent.SetDestination(CustomerManager.Instance.GetDispawnPoint().position);
    }

    private void Update()
    {
        if (agent.remainingDistance <= 1f)
        {
            CustomerManager.Dispawn(this);
        }
    }
}