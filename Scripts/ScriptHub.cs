using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using static ScriptHubUpdateFunction;


public class ScriptHub : MonoBehaviour
{
    public List<object> updateScripts = new List<object>();
    public List<object> fixUpdateScripts = new List<object>();


    void Update()
    {
        foreach (object obj in updateScripts)
        {
            try
            {
                IScriptHubFunctions script = (IScriptHubFunctions)obj;
                script.ScriptHubUpdate();
            }
            catch (Exception)
            {
                Debug.Log(this.name + " ERORR WILE TRYING DO ScriptHubUpdate() ON" + obj);
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
            catch (Exception)
            {
                Debug.Log(this.name + " ERORR WILE TRYING DO ScriptHubFixUpdate() ON" + obj);
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
