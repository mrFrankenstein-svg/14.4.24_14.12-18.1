using Bots;
using UnityEngine;
using AudioClipHubNamespace;
using UnityEngine.AI;

public class BotPrepperForWorkClass : BotPrepperForWork
{
    public override void PrepperForWork(BotAIBrain script, GameObject gamebject)
    {
        gamebject.AddComponent<AudioClipHubSlayer>();
        gamebject.AddComponent<NavMeshAgent>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
