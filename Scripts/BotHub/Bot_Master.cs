using Bots;
using UnityEngine;
using System.Collections;

namespace Bots
{
    public class Bot_Master : MonoBehaviour, IBotMaster
    {
        //тестовый обьект с  тестовым слейвом
        [SerializeField] GameObject testSlave;

        BotHub botHub;
        void Start()
        {
            //это ваще всё ненужно
            botHub = GameObject.Find("BotHub").GetComponent<BotHub>();
            testSlave.GetComponent<Bot_Slave>().SetMaster(this);

            BotPrepperForWork();
            //StartCoroutine(time());
        }
        //IEnumerator time()
        //{ 
        //    yield return new WaitForSeconds(0.1f);

        //    for (int i = 0; i < 3; i++)
        //    {
        //        SetComandToBots(this, (BotComands)i);
        //    }
        //}
        public void SetComandToBots(IBotMaster botMaster, object someValueForScript, BotComands comand)
        {
            botHub.Invoker(botMaster, someValueForScript,comand);
        }

        public void BotPrepperForWork()
        {
            Debug.Log("Написать чё-нить");
        }
    }
}
