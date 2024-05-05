using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using static ScriptHubUpdateFunction;
public interface IScriptHubFunctions
{
    void ScriptHubUpdate();
    void ScriptHubFixUpdate();
    void ScriptHubOneSecondUpdate();
    void StartFunction();
}
public enum ScriptHubUpdateFunction
{
    FunctionUpdate,
    FunctionFixedUpdate,
    FunctionOneSecondUpdate
}

public class ScriptHub : MonoBehaviour
{
    [SerializeField] List<object> updateScripts = new List<object>();
    [SerializeField] List<object> fixUpdateScripts = new List<object>();
    [SerializeField] List<object> oneSecondUpdate=new List<object>();

    private void Awake()
    {
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
                IScriptHubFunctions script = (IScriptHubFunctions)obj;
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
                IScriptHubFunctions script = (IScriptHubFunctions)obj;
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
                    IScriptHubFunctions script = (IScriptHubFunctions)obj;
                    script.ScriptHubOneSecondUpdate();
                }
                catch (Exception e)
                {
                    Debug.Log(this.name + " ERORR WILE TRYING DO OncePerSecond() ON " + obj + "\n\n\n" + e);
                }
            }
        }
    }
    public void AddToScriptsList(object script, ScriptHubUpdateFunction updateFunction)
    {
        switch (updateFunction)
        {
            case FunctionUpdate:
                if (!updateScripts.Contains(script))
                    updateScripts.Add(script);
                break;

            case FunctionFixedUpdate:
                if (!fixUpdateScripts.Contains(script))
                    fixUpdateScripts.Add(script);
                break;

            case FunctionOneSecondUpdate:
                if(!oneSecondUpdate.Contains(script))
                    oneSecondUpdate.Add(script);
                break;

            default:
                Debug.LogError("ScriptHub AddToScriptsList() error.");
                break;
        }

    }
    public void RemoveFromUScriptsList(object script)
    {
        if (updateScripts.Contains(script))
            updateScripts.Remove(script);
        if (fixUpdateScripts.Contains(script))
            fixUpdateScripts.Remove(script);
        if (oneSecondUpdate.Contains(script))
            oneSecondUpdate.Remove(script);
    }
}
