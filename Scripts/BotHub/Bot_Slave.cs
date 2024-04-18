using UnityEngine;

namespace Bots
{
    public class Bot_Slave : MonoBehaviour, IBotSlave
    {
        //«десь показываетс€ на кого сейчас ориентируетс€ в поведении этот бот.
        //
        //It shows who this bot is currently targeting in behavior.")]

        [SerializeField] IBotMaster myMaster;

        [Space]
        [Tooltip("—крипт, в который нужно будет написать всЄ, что должно быть подготовлено дл€ работы бота." +
            "\n" +
            "A script in which you will need to write everything that needs to be prepared for the bot to work.")]

        [SerializeField] private BotPrepperForWork prepperForWork;

        [Space]
        [Tooltip("—юда поставл€ютс€ скрипты, а которых должно быть прописано поведение определЄнного типа, которое ожидаетс€ от бота." +
            "\n" +
            "Scripts are supplied here, and which should specify the behavior of a certain type that is expected from the bot.")]

        [SerializeField] private BotComandFollow followComandScript;
        [SerializeField] private BotComandInteract interactComandScript;
        [SerializeField] private BotComandAction actionComandScript;


        [Space]
        [SerializeField] BotHub botHub;


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
