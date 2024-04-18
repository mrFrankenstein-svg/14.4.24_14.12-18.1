using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bots;
using UnityEngine.AI;

public class BotBrain : BotSlaveAIBrain
{
    [SerializeField] NavMeshAgent navMeshAgent;
    public override void PrepperForWork()
    {
        throw new System.NotImplementedException();
    }
    public override void ActionComand(BotComandAction ActionComand)
    {
        throw new System.NotImplementedException();
    }

    public override void FollowComand(BotComandFollow followComand)
    {
        throw new System.NotImplementedException();
    }

    public override void InteractComand(BotComandInteract interactComand)
    {
        throw new System.NotImplementedException();
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
