using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace EnvironmentSpawnerNamespace
{
    public class EnvironmentSpawner : MonoBehaviour
    {
        // --- Синглтон ---
        public static EnvironmentSpawner Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        [Header("Зона спавна")]
        [SerializeField] private Vector2 dimensions = new Vector2(5, 5);
        [SerializeField] private LayerMask ignoreLayers;

        [Header("Префабы")]
        [SerializeField] private List<GameObject> prefabs = new List<GameObject>();

        [Header("Родительский объектов")]
        [SerializeField] private Transform parent;

        [Header("Ограничениямест для спавна")]
        [SerializeField] private LayerMask blockedLayers;
        [SerializeField] private List<string> blockedTags;

        [Header("Разрешения мест для спавна")]
        [SerializeField] private LayerMask allowedLayers;
        [SerializeField] private List<string> allowedTags;


        [Header("Настройки объекта")]
        [SerializeField] private Vector2 scaleRange = new Vector2(1f, 1f);
        [SerializeField] private Vector2 rotationRange = new Vector2(0f, 360f);
        [SerializeField] private bool addIdToName = false;

        [Header("Служебное")]
        private byte mistakes=0;
        [SerializeField] public int numberOfObjThatNeedToSpawn=0;
        [SerializeField] private List<GameObject> spawnedObjects = new List<GameObject>();

        // --- Основные публичные методы ---
        public void Spawn(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = SpawnOne();
                if (obj != null)
                    spawnedObjects.Add(obj);
            }
        }

        public void Clear()
        {
            foreach (var obj in spawnedObjects)
            {
                if (obj != null)
                    DestroyImmediate(obj);
            }
            spawnedObjects.Clear();
        }

        // --- Вспомогательные методы ---
        private GameObject SpawnOne()
        {
            mistakes = 0;
            if (prefabs.Count == 0) return null;

            var prefab = prefabs[Random.Range(0, prefabs.Count)];

            // Масштаб
            float scale = Random.Range(scaleRange.x, scaleRange.y);

            Vector3 hit=Vector3.zero;
            for ( ;mistakes<255 ; )
            {
                object i = FindingAFreePlaceForAnObject(prefab, scale);

                if (i is Vector3)
                {
                    hit = (Vector3)i;
                    mistakes = 0;
                    break;
                }
                else
                {
                    mistakes++;
                }
            }

            if (hit != Vector3.zero)
            {

                // Создаём объект
                var obj = Instantiate(prefab, hit, Quaternion.identity, parent != null ? parent : transform);

                // Имя
                obj.name = prefab.name;
                if (addIdToName) obj.name += $"_{obj.GetInstanceID()}";

                // Поворот
                float yRot = Random.Range(rotationRange.x, rotationRange.y);
                obj.transform.Rotate(obj.transform.rotation.x, yRot, obj.transform.rotation.z);

                obj.transform.localScale = Vector3.one * scale;

                return obj;
            }
            else
            {
                Debug.LogError("[EnvironmentSpawner] Не найдено место для объекта.");
                return null;
            }
        }
        private object FindingAFreePlaceForAnObject(GameObject prefab, float scale)
        {
            // Луч вниз
            if (!Physics.Raycast(GetRandomPointAbove(), Vector3.down, out RaycastHit hit, Mathf.Infinity, ~ignoreLayers))
                return null;

            // Проверка слоёв и тегов
            if (((1 << hit.collider.gameObject.layer) & blockedLayers) != 0) return null;
            if (blockedTags.Contains(hit.collider.tag)) return null;

            // Проверка коллизий перед созданием
            Vector3 checkSize = GetPrefabBounds(prefab) * scale * 0.5f;
            Collider[] overlaps = Physics.OverlapBox(hit.point, checkSize, Quaternion.identity);
            //if (overlaps.Length > 0) return null; // место занято
            for (int i = 0; i < overlaps.Length; i++)
            {
                if (blockedTags.Contains(overlaps[i].tag)) return null; 
                if (((1 << overlaps[i].gameObject.layer) & blockedLayers) != 0) return null;
            }
            return hit.point;

        }    

        private Vector3 GetRandomPointAbove()
        {
            Vector3 offset = new Vector3(
                Random.Range(-dimensions.x / 2, dimensions.x / 2),
                10f,
                Random.Range(-dimensions.y / 2, dimensions.y / 2)
            );
            return transform.position + offset;
        }

        private Vector3 GetPrefabBounds(GameObject prefab)
        {
            Renderer rend = prefab.GetComponent<Renderer>();
            Collider col = prefab.GetComponent<Collider>();
            if (rend != null)
                return rend.bounds.size;
            else
                return col.bounds.size;

               // return Vector3.one; // если нет рендера
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 0, 1, 0.25f);
            Gizmos.DrawCube(transform.position, new Vector3(dimensions.x, 0.1f, dimensions.y));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(dimensions.x, 0.1f, dimensions.y));
        }
    }
}
