using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bots;
using UnityEngine.AI;
using AudioClipHubNamespace;

public class BotSlaveBrain : BotSlaveAIBrain
{
    [SerializeField] public NavMeshAgent navMeshAgent;
    [SerializeField] AudioClipHubSlayer audioClipHubSlayer;
    public override void PrepperForWork()
    {
        navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        audioClipHubSlayer = gameObject.AddComponent<AudioClipHubSlayer>();
        Debug.Log("BotSlaveBrain  PrepperForWork закончился");
    }
    public void NavMeshAgentMove(Vector3 moveTo)
    { 
        navMeshAgent.Move(moveTo);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
