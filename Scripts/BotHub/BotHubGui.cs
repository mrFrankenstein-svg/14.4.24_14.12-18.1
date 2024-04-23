using UnityEngine;
using UnityEditor;
using Bots;
using System.Windows.Input;
using UnityEngine.Timeline;

[CustomEditor(typeof(Bot_Slave))]

public class BotHubGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Bot_Slave targ = (Bot_Slave)target;
        if (GUILayout.Button("Follow button"))
        {
            targ.BotBehaviorController(targ.GetMaster(), new Vector3(0,0,0), BotComands.Follow);
        }
    }
}
