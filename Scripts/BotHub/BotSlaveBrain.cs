using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bots;
using UnityEngine.AI;
using AudioClipHubNamespace;

using static ScriptHubUpdateFunction;
using TMPro;

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
    public bool NavMeshAgentMove(Vector3 moveTo)
    {
        NavMeshPath path= new NavMeshPath();
        navMeshAgent.CalculatePath(moveTo, path);
        if (PathStatus(path) == true)
        {
            StartCoroutine(AddRandomDelay(() => 
            {
                navMeshAgent.SetPath(path);
            }));
            return true;
        }
        else
        {
            return false;
        }
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
    public bool PathStatus(NavMeshPath pathThatNeedetStatus)
    {
        if (pathThatNeedetStatus.status == NavMeshPathStatus.PathComplete)
            return true;
        else
            return false;
    }
    //добавляет задержку в любой скрипт если сделать следующую конструкцию:
    //  StartCoroutine(AddRandomDelay(() => {
    //      Действие, которое будет выполнено после задержки
    //      Debug.Log("Задержка завершена");
    //  }));

private IEnumerator AddRandomDelay(System.Action action)
    {
        // Генерация случайной задержки от 50 миллисекунд до 1 секунды
        float delay = Random.Range(0.03f, 1.0f);

        // Ожидание указанное количество секунд
        yield return new WaitForSeconds(delay);

        // Выполнение переданного действия после задержки
        action();
    }
}
