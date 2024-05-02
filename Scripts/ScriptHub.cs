using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using static ScriptHubUpdateFunction;
public interface IScriptHubFunctions
{
    void ScriptHubUpdate();
    void ScriptHubFixUpdate();
    void StartFunction();
}
public enum ScriptHubUpdateFunction
{
    FunctionUpdate,
    FunctionFixedUpdateEnum
}

public class ScriptHub : MonoBehaviour
{
    public List<object> updateScripts = new List<object>();
    public List<object> fixUpdateScripts = new List<object>();

    private void Awake()
    {
        gameObject.name = "ScriptHub";
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
                Debug.Log(this.name + " ERORR WILE TRYING DO ScriptHubUpdate() ON" + obj + "\n\n\n" + e);
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
    public void AddToScriptsList(object script, ScriptHubUpdateFunction updateFunction)
    {
        //Debug.Log(script);

        if (updateFunction == FunctionUpdate)
        {
            if (!updateScripts.Contains(script))
                updateScripts.Add(script);
        }
        else
        {
            if (!fixUpdateScripts.Contains(script))
                fixUpdateScripts.Add(script);
        }
    }
    public void RemoveFromUScriptsList(object script)
    {
        if (updateScripts.Contains(script))
            updateScripts.Remove(script);
        if (fixUpdateScripts.Contains(script))
            fixUpdateScripts.Remove(script);
    }
}
