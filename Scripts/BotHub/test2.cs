using Bots;
using UnityEngine;
public class test2 : BotComandFollow
{
    public override void FollowComand(IBotSlave waitingForCommand)
    {
        Debug.Log("  ОН следует"); 
    }
}