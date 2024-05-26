using Bots;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static ScriptHubUpdateFunction;

namespace Bots
{
    public class Bot_Master : MonoBehaviour, IBotMaster, IScriptHubFunctions
    {
        //два поля вынесенные сюда для теста из фкнкции CalculateRadiusForFollowingPoints2TEST()
        [SerializeField] int maxSlavesInThisCircle = 6;
        [SerializeField] float distanceFromMaster = 3f;


        //тестовый обьект с  тестовым слейвом
        [SerializeField] List<GameObject> slaveList;

        BotHub botHub;

        [SerializeField] GameObject followPointsGameobjektsParent;
        [SerializeField] List<GameObject> followPointsGameobjekts;

        //херня для визуализации. В игре не нужно.
        [SerializeField] Mesh mesh;
        void Start()
        {
            StartFunction();

            BotPrepperForWork();
        }
        public void SetComandToBots(IBotMaster botMaster, BotComands comand, params object[] someValueForScript)
        {
            botHub.Invoker(botMaster, comand, someValueForScript);
        }

        public void BotPrepperForWork()
        {
            //Вставляю эту функцию только для того, чтоб проверить работу. Мне просто надоело в ручную их ставить.
            TESTFindeAllSlaves();
            //вычисление начального положения точек на начало существования бота
            //calculating the initial position of the points at the beginning of the bot's existence
            CalculateRadiusForFollowingPoints();

            foreach (var item in slaveList)
            {
                item.GetComponent<Bot_Slave>().SetMasterAndNumberOfThisBotInList(this, slaveList.IndexOf(item));
            }

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
            botHub = GameObject.Find("BotHub").GetComponent<BotHub>();

            FindObjectOfType<ScriptHub>().AddToScriptsList(this, FunctionOneSecondUpdate);
        }

        public void ScriptHubOneSecondUpdate()
        {
            followPointsGameobjektsParent.transform.position = gameObject.transform.position;
            SetComandToBots(this, BotComands.Follow, followPointsGameobjekts);

        }

        /// <summary>
        /// Эта функция будет вычислять точки следования за объектом.___
        /// This function will calculate the points of following the object.
        /// </summary>
        private void CalculateRadiusForFollowingPoints()
        {
            List<Vector3> followPointsVector3 = new List<Vector3>();

            int maxSlavesInThisCircle_test;
            float distanceFromMaster_test;
            int slavesCount = slaveList.Count;

            for (int i = 1; ; i++)
            {
                if (slavesCount == 0)
                    break;

                maxSlavesInThisCircle_test = maxSlavesInThisCircle * i;
                distanceFromMaster_test = distanceFromMaster * i;

                if (slavesCount >= maxSlavesInThisCircle_test)
                {
                    float angleStep = 360f / maxSlavesInThisCircle_test;
                    CalculatingPointOnRadius(angleStep, maxSlavesInThisCircle_test, distanceFromMaster_test, followPointsVector3);
                    slavesCount = slavesCount - maxSlavesInThisCircle_test;
                }
                else
                {
                    float angleStep = 360f / slavesCount;
                    CalculatingPointOnRadius(angleStep, slavesCount, distanceFromMaster_test, followPointsVector3);
                    slavesCount = 0;
                }
            }
            if (followPointsGameobjektsParent == null)
            {
                followPointsGameobjektsParent = new GameObject();
                followPointsGameobjektsParent.name = "FollowPointsGameObjectsParent " + gameObject.name;
                followPointsGameobjektsParent.transform.position = gameObject.transform.position;
                followPointsGameobjektsParent.transform.rotation = gameObject.transform.rotation;
            }


            foreach (var item in followPointsVector3)
            {
                GameObject pointObj = new GameObject();
                pointObj.transform.position = item;
                pointObj.transform.parent = followPointsGameobjektsParent.transform;
                //pointObj.AddComponent<MeshFilter>().mesh = mesh;
                //pointObj.AddComponent<MeshRenderer>();
                followPointsGameobjekts.Add(pointObj);
            }
            followPointsGameobjekts.Add(followPointsGameobjektsParent);

        }
        private void CalculatingPointOnRadius(float angleStep, int numberOfpointOnRadius, float distanceFromCenter, List<Vector3> listVector3)
        {
            for (int i = 0; i < numberOfpointOnRadius; i++)
            {
                float angle = i * angleStep;
                listVector3.Add(new Vector3(gameObject.transform.position.x + distanceFromCenter * Mathf.Cos(angle * Mathf.Deg2Rad),
                                               gameObject.transform.position.y,
                                               gameObject.transform.position.z + distanceFromCenter * Mathf.Sin(angle * Mathf.Deg2Rad)));
            }
        }
        private void TESTFindeAllSlaves()
        {
            GameObject[] slaves = GameObject.FindGameObjectsWithTag("Finish");
            foreach (GameObject slave in slaves)
            {
                slaveList.Add(slave);
            }
        }
    }
}
