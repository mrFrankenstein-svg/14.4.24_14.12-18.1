using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bots
{
    public class Bot_Slave : MonoBehaviour, IBotSlave
    {
        //Здесь показывается на кого сейчас ориентируется в поведении этот бот.
        //
        //It shows who this bot is currently targeting in behavior.")]

        BotHub botHub;
        IBotMaster myMaster; 

        [Space]
        [Header("Все эти скрипты должны лежать на GameObject со скриптом BotHub.\n\nAll this scripts should be on the GameObject with the BotHub script.")]
        
        [Space]
        [Space]
        [Tooltip("Скрипт, в который нужно будет написать всё, что должно быть подготовлено для работы бота." +
            "\n" +
            "A script in which you will need to write everything that needs to be prepared for the bot to work.")]

        [SerializeField] private BotPrepperForWork prepperForWork;

        [Space]
        [Tooltip("Сюда поставляются скрипты, а которых должно быть прописано поведение определённого типа, которое ожидается от бота." +
            "\n" +
            "Scripts are supplied here, and which should specify the behavior of a certain type that is expected from the bot.")]

        [SerializeField] private BotComandFollow followComandScript;
        [SerializeField] private BotComandInteract interactComandScript;
        [SerializeField] private BotComandAction actionComandScript;

        [ContextMenu("Reset The Value")]


        private void Start()
        {
            botHub = GameObject.Find("BotHub").GetComponent<BotHub>();
            BotSubscribtorOnDelegate();
            BotPrepperForWork();
        }
        public void BotPrepperForWork()
        {
            botHub.SlaveGetSettings(ref prepperForWork, ref followComandScript, ref interactComandScript,ref actionComandScript);
            prepperForWork.PrepperForWork(this);
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
