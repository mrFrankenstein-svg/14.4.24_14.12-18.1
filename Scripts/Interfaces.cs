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
public enum AudioClipHubFunction
{
    StepAudio,
    FireAudio
}