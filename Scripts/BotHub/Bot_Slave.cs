using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bots
{
    public class Bot_Slave : MonoBehaviour, IBotSlave
    {
        #region volume
        //Здесь показывается на кого сейчас ориентируется в поведении этот бот.
        //
        //It shows who this bot is currently targeting in behavior.")]

        BotHub botHub;
        [SerializeField] IBotMaster myMaster;
        [SerializeField] private int numberOfBotInList;

        //[Space]
        //[Space]
        //[Tooltip("Скрипт, в который нужно будет написать всё, что должно быть подготовлено для работы бота." +
        //    "\n\n" +
        //    "A script in which you will need to write everything that needs to be prepared for the bot to work.")]

        //[SerializeField] private BotPrepperForWork prepperForWork; 

        [Space]
        [Space]
        [Tooltip("Скрипт, который будет управлять логикой бота. Тут же должны быть созданы все поля, необходимые для работы бота." +
            "\n\n" +
            "A script that will control the logic of the bot. All the fields necessary for the Bot to work should be created here.")]
        [SerializeField] private BotSlaveAIBrain brainOfBot;
        [Space]
        [Space]
        [Tooltip("Сюда поставляются скрипты, а которых должно быть прописано поведение определённого типа, которое ожидается от бота." +
            "\n\n" +
            "Scripts are supplied here, and which should specify the behavior of a certain type that is expected from the bot.")]

        [SerializeField] private BotComandFollow followComandScript;
        [SerializeField] private BotComandInteract interactComandScript;
        [SerializeField] private BotComandAction actionComandScript;
        #endregion

        private void Start()
        {
            botHub = GameObject.Find("BotHub").GetComponent<BotHub>();
            BotPrepperForWork();
            BotSubscribtorOnDelegate();
        }
        public void BotPrepperForWork()
        {
            botHub.SlaveGetScripts(gameObject, ref followComandScript, ref interactComandScript, ref actionComandScript, ref brainOfBot);
            brainOfBot.PrepperForWork();
        }

        public void BotBehaviorController(IBotMaster master, BotComands comand, params object[] someValueForScript)
        {
            if (master == myMaster)
            {
                switch (comand)
                {
                    case BotComands.Follow:
                        followComandScript.FollowComand(brainOfBot,numberOfBotInList, someValueForScript);
                        break;
                    case BotComands.Interact:
                        interactComandScript.InteractComand(brainOfBot);
                        break;
                    case BotComands.Action:
                        actionComandScript.ActionComand(brainOfBot);
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

        public void SetMasterAndNumberOfThisBotInList(IBotMaster newMaster, int numberOfThisBotInList)
        {
            myMaster = newMaster;
            numberOfBotInList = numberOfThisBotInList;
        }
        /// <summary>
        /// / its need only for BotHubGui script. Do not ues it. 
        /// он нужен только для скрипта BotHubGui. Не используйте его.
        /// </summary>
        public IBotMaster GetMaster()
        {
            return myMaster;
        }

    }
}
