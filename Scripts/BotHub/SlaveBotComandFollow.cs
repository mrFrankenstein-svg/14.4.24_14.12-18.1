using Bots;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SlaveBotComandFollow : BotComandFollow
{
    //BotSlaveBrain botSlaveBrainClas;
    public override void FollowComand(BotSlaveAIBrain waitingForCommand, int indexOfThisBot, params object[] gameObjectOrVector3ToFollow)
    {
        BotSlaveBrain botSlaveBrainClas = (BotSlaveBrain)waitingForCommand;
        List<GameObject> list = (List<GameObject>)gameObjectOrVector3ToFollow[0];
        if (botSlaveBrainClas.NavMeshAgentMove(list[indexOfThisBot].GetComponent<Transform>().position)==false)
        {
            botSlaveBrainClas.NavMeshAgentMove(list.Last().GetComponent<Transform>().position);
        }

        //if (botSlaveBrainClas.PathStatus()==false)
        //{
        //}

    }
    //private void SetTargetToNavmesh(int indexOfThisBot, List<GameObject> gameObjectOrVector3ToFollow)
    //{
    //    //if (gameObjectOrVector3ToFollow[indexOfThisBot].GetType() == typeof(Vector3))
    //    //{
    //    //    //botSlaveBrainClas.NavMeshAgentMove((Vector3)gameObjectOrVector3ToFollow[indexOfThisBot]);
    //    //}
    //    //else if (gameObjectOrVector3ToFollow.GetType() == typeof(GameObject))
    //    //{
    //    //    GameObject gamObj = (GameObject)gameObjectOrVector3ToFollow[indexOfThisBot];
    //    //    botSlaveBrainClas.NavMeshAgentMove(gamObj.GetComponent<Transform>().position);
    //    //}
    //    //botSlaveBrainClas.NavMeshAgentMove(gameObjectOrVector3ToFollow[indexOfThisBot].GetComponent<Transform>().position);
            
    //    //else
    //    //{
    //    //    UnityEngine.Debug.LogError(botSlaveBrainClas.name + " send incorrect parametrs in " + name + " FollowComand().");
    //    //}
    //}
}
