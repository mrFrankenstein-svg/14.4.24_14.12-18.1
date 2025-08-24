using System.Collections.Generic;
using UnityEngine;
public enum AnimState
{
    None,
    Death,
    CombatIdle,
    GuardIdle,
    Run,
    Shoot
}

[RequireComponent(typeof(Animator))]
public class AnimationEventsHolder : MonoBehaviour, IScriptHubUpdateFunction
{
    [SerializeField] Animator animator;
    AnimatorStateInfo state;


    //словарь для сопоставления хэшей с enum
    [SerializeField] private Dictionary<int, AnimState> stateMap;

    public void OnEnable()
    {
        ScriptHub.OnAddToScriptsList?.Invoke(this);
    }

    public void OnDisable()
    {
        ScriptHub.OnRemoveFromScriptsList?.Invoke(this);
    }
    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>(); 

        state = animator.GetCurrentAnimatorStateInfo(0);
   
        animator = GetComponent<Animator>();

        // Заполняем карту хэш  enum
        stateMap = new Dictionary<int, AnimState>
        {
            { Animator.StringToHash(AnimationsNameHolder.deathAnimationName), AnimState.Death },
            { Animator.StringToHash(AnimationsNameHolder.combatIdleAnimationName), AnimState.CombatIdle },
            { Animator.StringToHash(AnimationsNameHolder.guardIdleAnimationName), AnimState.GuardIdle },
            { Animator.StringToHash(AnimationsNameHolder.runAnimationName), AnimState.Run },
            { Animator.StringToHash(AnimationsNameHolder.shootAnimationName), AnimState.Shoot },
        };
    }

    /// <summary>
    /// Получить текущее состояние анимации как enum
    /// </summary>
    public AnimState GetCurrentState(int layer = 0)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layer);
        int currentHash = stateInfo.shortNameHash;

        if (stateMap.TryGetValue(currentHash, out AnimState state))
        {
            return state;
        }

        return AnimState.None;
    }

    public void ScriptHubUpdate()
    {
        AnimState currentState = GetCurrentState();

        switch (currentState)
        {
            case AnimState.None:
                break;
            case AnimState.Death:
                break;
            case AnimState.CombatIdle:
                break;
            case AnimState.GuardIdle:
                Debug.Log("hf,jnftn");
                break;
            case AnimState.Run:
                break;
            case AnimState.Shoot:
                break;
            default:
                Debug.Log("Неизвестное состояние");
                break;
        }
    }
}
