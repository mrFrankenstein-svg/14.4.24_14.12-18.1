using Bots;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SlaveBotComandFollow : BotComandFollow
{
    public override void FollowComand(BotSlaveAIBrain waitingForCommand, object gameObjectOrVector3ToFollow)
    {
        BotSlaveBrain clas = (BotSlaveBrain)waitingForCommand;

        if (gameObjectOrVector3ToFollow. GetType() == typeof(Vector3) )
        {            
            clas.NavMeshAgentMove((Vector3)gameObjectOrVector3ToFollow);
        }
        else if (gameObjectOrVector3ToFollow.GetType() == typeof(GameObject))
        {
            GameObject gamObj = (GameObject)gameObjectOrVector3ToFollow;
            clas.NavMeshAgentMove(gamObj.GetComponent<Transform>().position);
        }
        else
        {
            UnityEngine.Debug.LogError(clas.name + " send incorrect parametrs in " + name + " FollowComand().");
        }
    }
}
