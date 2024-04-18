using Bots;
using UnityEngine;

public class test3 : BotComandInteract
{
    public override void InteractComand(IBotSlave waitingForCommand)
    {
        Debug.Log("  ОН интерактирует"); 
    }
}
