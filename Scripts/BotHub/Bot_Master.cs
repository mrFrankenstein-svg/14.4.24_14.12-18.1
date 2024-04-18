using Bots;
using UnityEngine;
using System.Collections;

namespace Bots
{
    public class Bot_Master : MonoBehaviour, IBotMaster
    {
        [SerializeField] GameObject testSlave;

        [SerializeField] BotHub botHub;
        void Start()
        {
            botHub = GameObject.Find("BotHub").GetComponent<BotHub>();
            testSlave.GetComponent<Bot_Slave>().SetMaster(this);
            StartCoroutine(time());
        }
        IEnumerator time()
        { 
            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < 3; i++)
            {
                SetComandToBots(this, (BotComands)i);
            }
        }
        public void SetComandToBots(IBotMaster botMaster, BotComands comand)
        {
            botHub.Invoker(botMaster,comand);
        }
    }
}
