using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bots
{
    public class Bot_Slave : MonoBehaviour, IBotSlave
    {
        #region volume
        //«десь показываетс€ на кого сейчас ориентируетс€ в поведении этот бот.
        //
        //It shows who this bot is currently targeting in behavior.")]

        BotHub botHub;
        IBotMaster myMaster; 

        //[Space]
        //[Space]
        //[Tooltip("—крипт, в который нужно будет написать всЄ, что должно быть подготовлено дл€ работы бота." +
        //    "\n\n" +
        //    "A script in which you will need to write everything that needs to be prepared for the bot to work.")]

        //[SerializeField] private BotPrepperForWork prepperForWork; 
        
        [Space]
        [Space]
        [Tooltip("—крипт, который будет управл€ть логикой бота. “ут же должны быть созданы все пол€, необходимые дл€ работы бота." +
            "\n\n" +
            "A script that will control the logic of the bot. All the fields necessary for the Bot to work should be created here.")]
        [SerializeField] private BotSlaveAIBrain brainOfBot;
        [Space]
        [Space]
        [Tooltip("—юда поставл€ютс€ скрипты, а которых должно быть прописано поведение определЄнного типа, которое ожидаетс€ от бота." +
            "\n\n" +
            "Scripts are supplied here, and which should specify the behavior of a certain type that is expected from the bot.")]

        [SerializeField] private BotComandFollow followComandScript;
        [SerializeField] private BotComandInteract interactComandScript;
        [SerializeField] private BotComandAction actionComandScript;
        #endregion

        private void Start()
        {
            botHub = GameObject.Find("BotHub").GetComponent<BotHub>();
            BotSubscribtorOnDelegate();
            BotPrepperForWork();
        }
        public void BotPrepperForWork()
        {
            botHub.SlaveGetSettings( ref followComandScript, ref interactComandScript,ref actionComandScript, ref brainOfBot);
            brainOfBot.PrepperForWork();
        }

        public void BotBehaviorController(IBotMaster master, BotComands comand)
        {
            if (master == myMaster)
            {
                switch (comand)
                {
                    case BotComands.Follow:
                        followComandScript.FollowComand(this);
                        break;
                    case BotComands.Interact:
                        interactComandScript.InteractComand(this);
                        break;
                    case BotComands.Action:
                        actionComandScript.ActionComand(this);
                        break;
                    default:
                        Debug.LogError(this + " the bot cannot execute or execute the command " + comand);
                        break;
                }
            }
        }

        public void BotDescriptorOnDelegate()
        {
            botHub.BotSubscribtor(BotBehaviorController);
        }

        public void BotSubscribtorOnDelegate()
        {
            botHub.BotSubscribtor(BotBehaviorController);
        }

        public void SetMaster(IBotMaster newMaster)
        {
            myMaster = newMaster;
        }
        
    }
}
