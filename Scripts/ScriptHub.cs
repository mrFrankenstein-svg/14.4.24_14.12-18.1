using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//using static ScriptHubUpdateFunction;
public interface IScriptHubTechnicalInterface
{
    void EndFunction();
    void StartFunction();
}
public interface IScriptHubUpdateFunction: IScriptHubTechnicalInterface
{
    void ScriptHubUpdate();
}
public interface IScriptHubFixUpdateFunction : IScriptHubTechnicalInterface
{
    void ScriptHubFixUpdate();
}
public interface IScriptHubOneSecondUpdateFunction : IScriptHubTechnicalInterface
{
    void ScriptHubOneSecondUpdate();
}
//public enum ScriptHubUpdateFunction
//{
//    FunctionUpdate,
//    FunctionFixedUpdate,
//    FunctionOneSecondUpdate
//}

public class ScriptHub : MonoBehaviour
{
    private static ScriptHub scriptHub;
    [SerializeField] List<object> updateScripts = new List<object>();
    [SerializeField] List<object> fixUpdateScripts = new List<object>();
    [SerializeField] List<object> oneSecondUpdate=new List<object>();

    private void Awake()
    {
        scriptHub=this;
        gameObject.name = "ScriptHub";
    }
    private void Start()
    {
        StartCoroutine(OncePerSecond());
    }

    void Update()
    {
        foreach (object obj in updateScripts)
        {
            try
            {
                IScriptHubUpdateFunction script = (IScriptHubUpdateFunction)obj;
                script.ScriptHubUpdate();
            }
            catch (Exception e)
            {
                Debug.Log(this.name + " ERORR WILE TRYING DO ScriptHubUpdate() ON" + obj + "\n\n" + e);
            }
        }
    }
    private void FixedUpdate()
    {
        foreach (object obj in fixUpdateScripts)
        {
            try
            {
                IScriptHubFixUpdateFunction script = (IScriptHubFixUpdateFunction)obj;
                script.ScriptHubFixUpdate();
            }
            catch (Exception e)
            {
                Debug.Log(this.name + " ERORR WILE TRYING DO ScriptHubFixUpdate() ON" + obj + "\n\n\n" + e);
            }
        }
    }
    IEnumerator OncePerSecond()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);

            foreach (object obj in oneSecondUpdate)
            {
                try
                {
                    IScriptHubOneSecondUpdateFunction script = (IScriptHubOneSecondUpdateFunction)obj;
                    script.ScriptHubOneSecondUpdate();
                }
                catch (Exception e)
                {
                    Debug.Log(this.name + " ERORR WILE TRYING DO OncePerSecond() ON " + obj + "\n\n\n" + e);
                }
            }
        }
    }
    #region Первая_Версия_Метода
    //работает хорошо
    //public static void AddToScriptsList(object script, ScriptHubUpdateFunction updateFunction)
    //{
    //    switch (updateFunction)
    //    {
    //        case FunctionUpdate:
    //            if (!scriptHub.updateScripts.Contains(script))
    //                scriptHub.updateScripts.Add(script);
    //            break;

    //        case FunctionFixedUpdate:
    //            if (!scriptHub.fixUpdateScripts.Contains(script))
    //                scriptHub.fixUpdateScripts.Add(script);
    //            break;

    //        case FunctionOneSecondUpdate:
    //            if (!scriptHub.oneSecondUpdate.Contains(script))
    //                scriptHub.oneSecondUpdate.Add(script);
    //            break;

    //        default:
    //            Debug.LogError("ScriptHub AddToScriptsList() error.");
    //            break;
    //    }

    //}
    #endregion
    public static void AddToScriptsList(IScriptHubTechnicalInterface script)
    {
        byte tick=0;
        if (script is IScriptHubUpdateFunction)
        {
            if (!scriptHub.updateScripts.Contains(script))
                scriptHub.updateScripts.Add(script);
            tick++;
        }

        if (script is IScriptHubFixUpdateFunction)
        {
            if (!scriptHub.fixUpdateScripts.Contains(script))
                scriptHub.fixUpdateScripts.Add(script);
            tick++;
        }

        if (script is IScriptHubOneSecondUpdateFunction)
        {
            if (!scriptHub.oneSecondUpdate.Contains(script))
                scriptHub.oneSecondUpdate.Add(script);
            tick++;
        }
        
        if(tick==0)
            Debug.LogError("ScriptHub AddToScriptsList() error.");


    }
    public static void RemoveFromScriptsList(IScriptHubTechnicalInterface script)
    {
        if (scriptHub.updateScripts.Contains(script))
            scriptHub.updateScripts.Remove(script);
        if (scriptHub.fixUpdateScripts.Contains(script))
            scriptHub.fixUpdateScripts.Remove(script);
        if (scriptHub.oneSecondUpdate.Contains(script))
            scriptHub.oneSecondUpdate.Remove(script);
    }
}
