using Bots;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static ScriptHubUpdateFunction;

namespace Bots
{
    public class Bot_Master : MonoBehaviour, IBotMaster, IScriptHubFunctions
    {
        //�������� ������ �  �������� �������
        [SerializeField] List<GameObject> testSlave;

        BotHub botHub;
        void Start()
        {
            //��� ���� �� �������
            botHub = GameObject.Find("BotHub").GetComponent<BotHub>();
            testSlave[0].GetComponent<Bot_Slave>().SetMaster(this);

            StartFunction();

            BotPrepperForWork();
        }
        public void SetComandToBots(IBotMaster botMaster, object someValueForScript, BotComands comand)
        {
            botHub.Invoker(botMaster, someValueForScript,comand);
        }

        public void BotPrepperForWork()
        {
            Debug.Log("�������� ��-����");
        }

        public void ScriptHubUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void ScriptHubFixUpdate()
        {
            throw new System.NotImplementedException();
        }
        public void StartFunction()
        {
            FindObjectOfType<ScriptHub>().AddToScriptsList(this, FunctionOneSecondUpdate);
        }

        public void ScriptHubOneSecondUpdate()
        {
            SetComandToBots(this, gameObject, BotComands.Follow);
        }
    }
}
