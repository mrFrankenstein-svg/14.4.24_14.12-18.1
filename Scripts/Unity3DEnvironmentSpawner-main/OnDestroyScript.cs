using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnvironmentSpawnerNamespace
{
    public class OnDestroyScript : MonoBehaviour
    {
        [SerializeField] EnvironmentSpawner spawner;
        void OnDestroy()
        {
            Debug.Log("�������� � ���� ���-������");
            //spawner.ThisObjectHasBeenDestroyed(gameObject);
        }
        public void SetSpawner(EnvironmentSpawner spawn)
        { 
            spawner = spawn;
        }
    }
}