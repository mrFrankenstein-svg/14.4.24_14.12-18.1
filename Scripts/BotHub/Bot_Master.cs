using Bots;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static ScriptHubUpdateFunction;
using Unity.VisualScripting;
using TMPro;
using System.Runtime.InteropServices;

namespace Bots
{
    public class Bot_Master : MonoBehaviour, IBotMaster, IScriptHubFunctions
    {
        //тестовый обьект с  тестовым слейвом
        [SerializeField] List<GameObject> slaveList;
        [SerializeField] List<Vector3> followPointsPoints;

        BotHub botHub;
        void Start()
        {
            botHub = GameObject.Find("BotHub").GetComponent<BotHub>();

            foreach (var item in slaveList)
            {

                item.GetComponent<Bot_Slave>().SetMaster(this);
            }
            //testSlave[0].GetComponent<Bot_Slave>().SetMaster(this);

            StartFunction();

            BotPrepperForWork(); 
            //поставил сюда чисто для теста
            CalculateRadiusForFollowingPoints();
        }
        public void SetComandToBots(IBotMaster botMaster, object someValueForScript, BotComands comand,IBotSlave definiteBot = null)
        {
            botHub.Invoker(botMaster, someValueForScript, comand, definiteBot);
        }

        public void BotPrepperForWork()
        {
            //followPointsPoints = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), gameObject.transform);
            ////followPointsObject = new GameObject();
            ////followPointsObject.transform.position = gameObject.transform.position;
            ////followPointsObject.transform.rotation = gameObject.transform.rotation;
            //followPointsPoints.name = "FollowPointsObject";
            //followPointsPoints.transform.SetParent(gameObject.transform);
            Debug.Log("Написать ещё чё-нить");
        }

        public void ScriptHubUpdate()
        {
            //не нужно
            throw new System.NotImplementedException();
        }

        public void ScriptHubFixUpdate()
        {
            //не нужно
            throw new System.NotImplementedException();
        }
        public void StartFunction()
        {
            FindObjectOfType<ScriptHub>().AddToScriptsList(this, FunctionOneSecondUpdate);
        }

        public void ScriptHubOneSecondUpdate()
        {
            //это я сейчас быстро накатяю чтоб проверить как работает цикл отправкикоманды отдельному боту
            //и вычисления его вектора 3
            CalculateRadiusForFollowingPoints();
            for (int i = 0; i < slaveList.Count; i++)
            {
                SetComandToBots(this, followPointsPoints[i], BotComands.Follow, slaveList[i].GetComponent<Bot_Slave>());
            }
            
        }
        /// <summary>
        /// Эта функция будет вычислять точки следования за объектом.___
        /// This function will calculate the points of following the object.
        /// </summary>
        private void CalculateRadiusForFollowingPoints()
        {
            followPointsPoints.Clear();
            float angleStep = 360f / slaveList.Count;
            for (int i = 0; i < slaveList.Count; i++)
            {
                float angle = i * angleStep;
                followPointsPoints.Add(new Vector3(gameObject.transform.position.x + 3 * Mathf.Cos(angle * Mathf.Deg2Rad),
                                               gameObject.transform.position.y,
                                               gameObject.transform.position.z + 3 * Mathf.Sin(angle * Mathf.Deg2Rad)));
            }
            //if(slaveList.Count <= 24)
        }
        private void CalculateRadiusForFollowingPoints2TEST()
        {
            followPointsPoints.Clear();
            int slavesCount= slaveList.Count;
            for (int i = 0; ; i++)
            {
                float distanceFromMaster = i * 3;
                if (slavesCount > 6 && i==0)
                {
                    ;KerningPairKey;LayoutKind;l
                    float angleStep = 360f / 6;
                    sss(angleStep, 6, distanceFromMaster);
                    slavesCount= slavesCount - 6;
                    continue;
                }
            }
        }
        private void sss(float angleStep, int tackts,float distanceFromCenter)
        {
            for (int i = 0; i < slaveList.Count; i++)
            {
                float angle = i * angleStep;
                followPointsPoints.Add(new Vector3(gameObject.transform.position.x + distanceFromCenter * Mathf.Cos(angle * Mathf.Deg2Rad),
                                               gameObject.transform.position.y,
                                               gameObject.transform.position.z + distanceFromCenter * Mathf.Sin(angle * Mathf.Deg2Rad)));
            }
        }
    }
}
