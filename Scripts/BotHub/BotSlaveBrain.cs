using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bots;
using UnityEngine.AI;
using AudioClipHubNamespace;

using static ScriptHubUpdateFunction;

public class BotSlaveBrain : BotSlaveAIBrain, IScriptHubFunctions
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] AudioClipHubSlayer audioClipHubSlayer;
    [SerializeField] Renderer renderer;
    [SerializeField] bool isStoped=true;
    public override void PrepperForWork()
    {
        animator = GetComponent<Animator>();
        renderer= GetComponent<Renderer>();

        if (gameObject.GetComponent<NavMeshAgent>() == null)
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        else
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        audioClipHubSlayer = gameObject.AddComponent<AudioClipHubSlayer>();

        StartFunction();

        Debug.Log("BotSlaveBrain PrepperForWork закончился");
    }
    public void NavMeshAgentMove(Vector3 moveTo)
    { 
        navMeshAgent.SetDestination(moveTo);
    }

    void Update()
    {
        
    }

    public void ScriptHubUpdate()
    {
        if (renderer.isVisible) 
        {
            if (!navMeshAgent.isStopped && isStoped!= navMeshAgent.isStopped)
            {
                animator.SetBool("run", !navMeshAgent.isStopped);
                isStoped = navMeshAgent.isStopped;
            }
        }
    }

    public void ScriptHubFixUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void StartFunction()
    {
        FindObjectOfType<ScriptHub>().AddToScriptsList(this, FunctionUpdate);
    }
}
