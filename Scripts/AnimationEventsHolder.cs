using UnityEngine;

public class AnimationEventsHolder : MonoBehaviour, IScriptHubUpdateFunction
{
    [SerializeField] Animator animator;
    private void Start()
    {
        if(animator == null)
            animator = GetComponent<Animator>();
    }
    public void StartFunction()
    {
        ScriptHub.AddToScriptsList(this);
    }
    public void ScriptHubUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void EndFunction()
    {ScriptHub.RemoveFromScriptsList(this);
    }
}
