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
    [SerializeField] bool isHasPath = false;
    public override void PrepperForWork()
    {
        animator = GetComponent<Animator>();

        if(GetComponent<Renderer>() != null)
            renderer= GetComponent<SkinnedMeshRenderer>();
        else
            renderer= GetComponentInChildren<SkinnedMeshRenderer>();

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

    public void ScriptHubUpdate()
    {
        if (renderer.isVisible) 
        {
            if (isHasPath != navMeshAgent.hasPath)
            {
                animator.SetBool("run", navMeshAgent.hasPath);
                isHasPath = navMeshAgent.hasPath;
            }
        }

    }

    public void ScriptHubFixUpdate()
    {
        //это не нужно но обязано тут быть
        throw new System.NotImplementedException();
    }

    public void StartFunction()
    {
        FindObjectOfType<ScriptHub>().AddToScriptsList(this, FunctionUpdate);
    }

    public void ScriptHubOneSecondUpdate()
    {
        //это не нужно но обязано тут быть
        throw new System.NotImplementedException();
    }
}
