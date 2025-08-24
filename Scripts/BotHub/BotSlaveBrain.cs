using System.Collections;
using UnityEngine;
using Bots;
using UnityEngine.AI;

public class BotSlaveBrain : BotSlaveAIBrain, IScriptHubUpdateFunction
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] private Renderer renderer;
    [SerializeField] bool isHasPath = false;


    public void OnEnable()
    {
        ScriptHub.OnAddToScriptsList?.Invoke(this);
    }

    public void OnDisable()
    {
        ScriptHub.OnRemoveFromScriptsList?.Invoke(this);
    }
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
        if (gameObject.name == "BotHub") return;
        if (renderer.isVisible) 
        {
            if (isHasPath != navMeshAgent.hasPath)
            {
                animator.SetBool("run", navMeshAgent.hasPath);
                isHasPath = navMeshAgent.hasPath;
            }
        }

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
