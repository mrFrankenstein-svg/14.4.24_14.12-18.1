using Bots;
using UnityEngine;

public class test4 : BotComandAction
{
    public override void ActionComand(IBotSlave waitingForCommand)
    {
        Debug.Log("  ОН действует");
    }
}