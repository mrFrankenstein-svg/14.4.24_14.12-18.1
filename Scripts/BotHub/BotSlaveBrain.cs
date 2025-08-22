using System.Collections;
using UnityEngine;
using Bots;
using UnityEngine.AI;
using AudioClipHubNamespace;

public class BotSlaveBrain : BotSlaveAIBrain, IScriptHubUpdateFunction
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent navMeshAgent;
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


        StartFunction();

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


    public void StartFunction()
    {
        ScriptHub.AddToScriptsList(this);
    }

    public bool PathStatus(NavMeshPath pathThatNeedetStatus)
    {
        if (pathThatNeedetStatus.status == NavMeshPathStatus.PathComplete)
            return true;
        else
            return false;
    }
    //��������� �������� � ����� ������ ���� ������� ��������� �����������:
    //  StartCoroutine(AddRandomDelay(() => {
    //      ��������, ������� ����� ��������� ����� ��������
    //      Debug.Log("�������� ���������");
    //  }));

private IEnumerator AddRandomDelay(System.Action action)
    {
        // ��������� ��������� �������� �� 50 ����������� �� 1 �������
        float delay = Random.Range(0.03f, 1.0f);

        // �������� ��������� ���������� ������
        yield return new WaitForSeconds(delay);

        // ���������� ����������� �������� ����� ��������
        action();
    }
}
