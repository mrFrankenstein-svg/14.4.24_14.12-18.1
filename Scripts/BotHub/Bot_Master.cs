using Bots;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static ScriptHubUpdateFunction;

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
        public void SetComandToBots(IBotMaster botMaster, object someValueForScript, BotComands comand)
        {
            botHub.Invoker(botMaster, someValueForScript, comand);
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
            SetComandToBots(this, gameObject, BotComands.Follow);
            //CalculateRadiusForFollowingPoints();
        }
        /// <summary>
        /// Эта функция будет вычислять точки следования за объектом.___This function will calculate the points of following the object.
        /// </summary>
        private void CalculateRadiusForFollowingPoints()
        {
            float angleStep = 360f / slaveList.Count;

            for (int i = 0; i < slaveList.Count; i++)
            {
                float angle = i * angleStep;
                followPointsPoints.Add( new Vector3(gameObject.transform.position.x + 3 * Mathf.Cos(angle * Mathf.Deg2Rad),
                                               gameObject.transform.position.y,
                                               gameObject.transform.position.z + 3 * Mathf.Sin(angle * Mathf.Deg2Rad)));

                //GameObject obj= Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), position, Quaternion.identity);
            }
        }
        public void GetPointForeSlaveFolowing()
        {
            Я добававил в скрипт возможность конкретного укзания бота. Надо этим воспользоватся для указания точки для следования
        }
    }
}
