using Unity.VisualScripting;
using UnityEngine;

namespace Bots
{
    public interface IBotSlave
    {
        void BotSubscribtorOnDelegate();
        void BotDescriptorOnDelegate();
        void BotBehaviorController(IBotMaster master, BotComands comand);
        void BotPrepperForWork();
        void SetMaster(IBotMaster newMaster);
    }
    public interface IBotMaster
    {
        void SetComandToBots(IBotMaster BotMaster, BotComands comand);
    }

    //Абстрактные классы, от которых должны быть унаследованы Скрипты содержащие поведение ботов.
    //
    //Abstract classes from which Scripts containing bot behavior should be inherited.

    public abstract class BotPrepperForWork : MonoBehaviour
    {
        public abstract void PrepperForWork(IBotSlave needsToBePrepared);
    }
    public abstract class BotComandFollow : MonoBehaviour
    {
        public abstract void FollowComand(IBotSlave waitingForCommand);
    }
    public abstract class BotComandInteract : MonoBehaviour
    {
        public abstract void InteractComand(IBotSlave waitingForCommand);
    }
    public abstract class BotComandAction : MonoBehaviour
    {
        public abstract void ActionComand(IBotSlave waitingForCommand);
    }
    public enum BotComands
    {
        Follow,
        Interact,
        Action
    }

    public class BotHub: MonoBehaviour
    {
        public delegate void BotBehaviorDelegate(IBotMaster master, BotComands comand);
        private static BotBehaviorDelegate delegateForSignedBots;

        void Awake()
        {
            gameObject.name = "BotHub";
        }
        public void BotDescriptor(BotBehaviorDelegate delegat)
        {
            delegateForSignedBots -= delegat;
        }

        public void BotSubscribtor(BotBehaviorDelegate delegat)
        {
            delegateForSignedBots += delegat;
        }
        public void Invoker(IBotMaster master, BotComands comand)
        {
            delegateForSignedBots.Invoke(master, comand);
        }
        public void SlaveGetSettings(ref BotPrepperForWork setingsScript, ref BotComandFollow followScript,
            ref BotComandInteract interactScript, ref BotComandAction actionScript)
        {
            setingsScript=GetComponent<BotPrepperForWork>();
            followScript=GetComponent<BotComandFollow>();
            interactScript=GetComponent<BotComandInteract>();
            actionScript=GetComponent<BotComandAction>();
        }
    }
}