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

    //����������� ������, �� ������� ������ ���� ������������ ������� ���������� ��������� �����.
    //
    //Abstract classes from which Scripts containing bot behavior should be inherited.
    
    //����� ��� ��� ������ �� �����, �� ���������
    //public abstract class BotPrepperForWork : MonoBehaviour
    //{
    //    public abstract void PrepperForWork(BotSlaveAIBrain needsToBePreparedScript, GameObject needsToBePreparedGameobject);
    //}
    public abstract class BotSlaveAIBrain : MonoBehaviour
    {
        public abstract void PrepperForWork();
        public abstract void FollowComand(BotComandFollow followComand);
        public abstract void InteractComand(BotComandInteract interactComand);
        public abstract void ActionComand(BotComandAction ActionComand);
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

    public class BotHub : MonoBehaviour
    {
        [Header("��� ������ ������ �������, ������� ����� ��������� ������� ���� �����.\n\nThere should be scripts that will control the work of all bots.")]

        [Space]
        [Space]
        [Tooltip("������, ������� ����� ��������� ������� ����. ��� �� ������ ���� ������� ��� ����, ����������� ��� ������ ����." +
            "\n\n" +
            "A script that will control the logic of the bot. All the fields necessary for the Bot to work should be created here.")]
        [SerializeField] private BotSlaveAIBrain brainOfSlaveBot;

        //[Space]
        //[Space]
        //[Tooltip("������, � ������� ����� ����� �������� ��, ��� ������ ���� ������������ ��� ������ ����." +
        //    "\n\n" +
        //    "A script in which you will need to write everything that needs to be prepared for the bot to work.")]
        //[SerializeField] private BotPrepperForWork prepperForWork;

        
        [Space]
        [Space]
        [Tooltip("���� ������������ �������, � ������� ������ ���� ��������� ��������� ������������ ����, ������� ��������� �� ����." +
            "\n\n" +
            "Scripts are supplied here, and which should specify the behavior of a certain type that is expected from the bot.")]
        [SerializeField] private BotComandFollow followComandScript;
        [SerializeField] private BotComandInteract interactComandScript;
        [SerializeField] private BotComandAction actionComandScript;

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
        public void SlaveGetSettings( ref BotComandFollow followScript,
            ref BotComandInteract interactScript, ref BotComandAction actionScript, ref BotSlaveAIBrain botBrain)
        {
            //setingsScript= prepperForWork;
            followScript=followComandScript;
            interactScript=interactComandScript;
            actionScript=actionComandScript; 
            botBrain= brainOfSlaveBot;
        }
    }
}