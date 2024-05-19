using Unity.VisualScripting;
using UnityEngine;

namespace Bots
{
    #region Interfaces, abstract clases and another stuff what we need for work

    public interface IBotSlave
    {
        void BotSubscribtorOnDelegate();
        void BotDescriptorOnDelegate();
        void BotBehaviorController(IBotMaster master, BotComands comand, params object[] someValueForScript);
        void BotPrepperForWork();
        void SetMasterAndNumberOfThisBotInList(IBotMaster newMaster, int numberOfThisBotInList);
    }
    public interface IBotMaster
    {
        void SetComandToBots(IBotMaster botMaster, BotComands comand, params object[] someValueForScript);
        void BotPrepperForWork();
    }

    //����������� ������, �� ������� ������ ���� ������������ ������� ���������� ��������� ��� �������.
    //
    //Abstract classes from which Scripts containing bot behavior for Master.

    public abstract class BotMasterAIBrain : MonoBehaviour
    {
        public abstract void SetComandToIBotMaster(IBotMaster thisBotMaster,  BotComands comand);
    }


    //����������� ������, �� ������� ������ ���� ������������ ������� ���������� ��������� ��� ������.
    //
    //Abstract classes from which Scripts containing bot behavior for Slave.

    public abstract class BotSlaveAIBrain : MonoBehaviour
    {
        public abstract void PrepperForWork();
    }
    public abstract class BotComandFollow : MonoBehaviour
    {
        /// <summary>
        /// �������, ������� ������ ���� ������������� ��� ����������� ����.___The function that should be used to move the bot.
        /// </summary>
        /// <param name="waitingForCommand">��� ������� �������� �������.___The bot that executes the command</param>
        /// <param name="gameObjectOrVector3ToFollow">���� ������ ���� ���������� Vector3 ��� GameObject � ������� ����� ���������.__Vector3 or GameObject should be sent here to which you need to move.</param>
        public abstract void FollowComand(BotSlaveAIBrain waitingForCommand, int indexOfThisBot, params object[] gameObjectOrVector3ToFollow);
    }
    public abstract class BotComandInteract : MonoBehaviour
    {
        public abstract void InteractComand(BotSlaveAIBrain waitingForCommand);
    }
    public abstract class BotComandAction : MonoBehaviour
    {
        public abstract void ActionComand(BotSlaveAIBrain waitingForCommand);
    }
    public enum BotComands
    {
        Follow,
        Interact,
        Action
    }
    #endregion

    public class BotHub : MonoBehaviour
    {
        #region Header and values 

        [Header("���, ������ �� ������� � ���� ��������, \n������ ������ �������, ������� �����\n��������� ������� ���� �����.\n\nOn objekt wiht this script should be scripts\nthat will control the work of all bots.")]

        [Space]
        [Space]
        [Tooltip("������, ������� ����� ��������� ������� ����. ��� �� ������ ���� ������� ��� ����, ����������� ��� ������ ����." +
            "\n\n" +
            "A script that will control the logic of the bot. All the fields necessary for the Bot to work should be created here.")]
        [SerializeField] private BotSlaveAIBrain brainOfSlaveBot;
        [SerializeField] private BotMasterAIBrain brainOfMasterBot;

        [Space]
        [Space]
        [Tooltip("���� ������������ �������, � ������� ������ ���� ��������� ��������� ������������ ����, ������� ��������� �� ����." +
            "\n\n" +
            "Scripts are supplied here, and which should specify the behavior of a certain type that is expected from the bot.")]
        [SerializeField] private BotComandFollow followComandScript;
        [SerializeField] private BotComandInteract interactComandScript;
        [SerializeField] private BotComandAction actionComandScript;
        #endregion

        public delegate void BotBehaviorDelegate(IBotMaster master,  BotComands comand, params object[] someValueForScript);
        private static BotBehaviorDelegate delegateForSignedBots;

        void Awake()
        {
            gameObject.name = "BotHub";

            brainOfSlaveBot= gameObject.GetComponent<BotSlaveAIBrain>();
            brainOfMasterBot=gameObject.GetComponent<BotMasterAIBrain>();

            followComandScript=gameObject.GetComponentInChildren<BotComandFollow>();
            interactComandScript = gameObject.GetComponent<BotComandInteract>();
            actionComandScript = gameObject.GetComponent<BotComandAction>();
        }
        public void BotDescriptor(BotBehaviorDelegate delegat)
        {
            delegateForSignedBots -= delegat;
        }

        public void BotSubscribtor(BotBehaviorDelegate delegat)
        {
            delegateForSignedBots += delegat;
        }
        /// <summary>
        /// �������� ���������� ������� � ��������.___Starts executing functions in the delegate.
        /// </summary>
        public void Invoker(IBotMaster master, BotComands comand, params object[] someValueForScript)
        {
            delegateForSignedBots.Invoke(master, comand, someValueForScript);
        }
        public void SlaveGetScripts( GameObject slaveGameobject, ref BotComandFollow followScript,
            ref BotComandInteract interactScript, ref BotComandAction actionScript, ref BotSlaveAIBrain botSlaveBrain)
        {
            //setingsScript= prepperForWork;
            followScript=followComandScript;
            interactScript=interactComandScript;
            actionScript=actionComandScript; 
            botSlaveBrain= (BotSlaveAIBrain)slaveGameobject.AddComponent( brainOfSlaveBot.GetType() );
        }
        public void MasterGetScripts()
        {
            Debug.Log("�� ������ �������� ��-���� � ������ MasterGetScripts() � BotHub");
        }
    }

}