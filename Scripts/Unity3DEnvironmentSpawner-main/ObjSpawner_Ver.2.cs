using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace EnvironmentSpawnerNamespace
{
    public class EnvironmentSpawner2 : MonoBehaviour
    {
        // --- �������� ---
        public static EnvironmentSpawner2 Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        [Header("���� ������ Spawn zone")]
        [SerializeField] private Vector2 dimensions = new Vector2(5, 5);
        //[SerializeField] private LayerMask ignoreLayers;

        [Header("������� Prefabs")]
        [SerializeField] private List<GameObject> prefabs = new List<GameObject>();

        [Header("������������ �������� Parent of the objects")]
        [SerializeField] private Transform parent;

        [Header("����������� ���� ��� ������ Restrictions on spawn locations")]
        [SerializeField] private LayerMask blockedLayers;
        [SerializeField] private List<string> blockedTags;

        [Header("���������� ���� ��� ������ Spawn site permissions")]
        [SerializeField] private LayerMask allowedLayers;
        [SerializeField] private List<string> allowedTags;


        [Header("��������� ������� Object Settings")]
        [SerializeField] private Vector2 scaleRange = new Vector2(1f, 1f);
        [SerializeField] private float xAngle = 0;
        [SerializeField] private Vector2 rotationRange = new Vector2(0f, 360f);
        [SerializeField] private bool addIdToName = false;

        [Header("��������� Official")] 
        //private static readonly RaycastHit[] tempHit = new RaycastHit[1];
        private byte mistakes = 0;
        [SerializeField] public int numberOfObjThatNeedToSpawn = 0;
        [SerializeField] private List<GameObject> spawnedObjects = new List<GameObject>();

        // --- �������� ��������� ������ --- Basic public methods
        public void Spawn(int count)
        {
            StartCoroutine(CheckCollisionsNextFixedUpdate(count));
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
        IEnumerator CheckCollisionsNextFixedUpdate(int needObjects, int unsuccessfulAttempts=0)
        {
            if (prefabs.Count == 0)
            {
                Debug.LogError("There are no recognized prefabs.");
                yield break;
            }

            var prefab = prefabs[Random.Range(0, prefabs.Count)];

            // ������� --- Scale
            float scale = Random.Range(scaleRange.x, scaleRange.y);

            // ������� --- rotation
            Quaternion randomRotation = Quaternion.Euler(xAngle, Random.Range(rotationRange.x, rotationRange.y), 0f);

            //Vector3 point = GetRandomPointAbove();
            bool isThePointFree=false;

            Vector3 point = Vector3.zero;

            yield return new WaitForFixedUpdate();

            for (; mistakes < 255;)
            {
                point = FindingAFreePlaceForAnObject2(prefab, scale, randomRotation);
                if (point != Vector3.zero)
                {
                    isThePointFree = true;
                    break;
                }
                else 
                {
                    mistakes++;
                }

            }

            mistakes = 0;

            if (isThePointFree)
            {
                // ������ ������ --- Creating an object
                var obj = Instantiate(prefab, point, randomRotation, parent != null ? parent : transform);

                obj.name = prefab.name;

                if (addIdToName) obj.name += $"_{obj.GetInstanceID()}";

                obj.transform.localScale = obj.transform.localScale * scale;

                spawnedObjects.Add(obj);
                
                if (needObjects - 1 > 0)
                    StartCoroutine(CheckCollisionsNextFixedUpdate(needObjects-1));
            }
            else
            {
                Debug.LogError("[EnvironmentSpawner] �� ������� ����� ��� �������.");

                if (unsuccessfulAttempts < 254)
                    StartCoroutine(CheckCollisionsNextFixedUpdate(needObjects, unsuccessfulAttempts+1));
                else
                    Debug.Log("There are too many mistakes to continue.");
            }
        }

        private Vector3 FindingAFreePlaceForAnObject2(GameObject obj, float scale, Quaternion rotation)
        {
            // �������� ��������
            Vector3 halfExtents = Vector3.zero;

            GetPrefabBounds(obj, ref halfExtents);

            //Vector3 halfExtents = bounds.extents;

            //��� ����
            if (!Physics.Raycast(GetRandomPointAbove(), Vector3.down, out RaycastHit hit, Mathf.Infinity))
                return Vector3.zero;

            //// �������� ���� � ����� --- Checking layers and tags
            //if (((1 << hit.collider.gameObject.layer) & blockedLayers) != 0) return Vector3.zero;
            //if (blockedTags.Contains(hit.collider.tag)) return Vector3.zero;

            // �������� �������� ����� ��������� --- Checking for collisions before creation
            //Vector3 checkSize = GetPrefabBounds(prefab) * scale * 0.5f;
            Collider[] overlaps = Physics.OverlapBox(hit.point, halfExtents, rotation);

            for (int i = 0; i < overlaps.Length; i++)
            {
                if (blockedTags.Contains(overlaps[i].tag))
                    return Vector3.zero;
                if (((1 << overlaps[i].gameObject.layer) & blockedLayers) != 0)
                    return Vector3.zero;
            }
            return hit.point;


            /*
            if (Physics.CheckBox(center, halfExtents * scale, rotation))
            {
                Debug.Log("���� ����������� � ������ ��������!");
                return false;
            }
            else
            {
                Debug.Log("��������, ����� ������� ������.");
                return true;
            }*/
        }
        /*
         --- ��������������� ������ --- Auxiliary methods
        private void SpawnOne()
        {
            StartCoroutine(CheckCollisionsNextFixedUpdate(prefab, scale, randomRotation));

            /* �������� ���.
            Vector3 hit=Vector3.zero;
            for ( ;mistakes<255 ; )
            {
                object i = FindingAFreePlaceForAnObject(prefab, scale);

                if (i is Vector3)
                {
                    hit = (Vector3)i;
                    break;
                }
                else
                {
                    mistakes++;
                }
            }

            if (hit != Vector3.zero)
            {
                // ������ ������ --- Creating an object
                var obj = Instantiate(prefab, hit, Quaternion.identity, parent != null ? parent : transform);

                obj.name = prefab.name;
                if (addIdToName) obj.name += $"_{obj.GetInstanceID()}";

                obj.transform.Rotate(xAngle, yRot, 0);

                obj.transform.localScale = obj.transform.localScale * scale;

                return obj;
            }
            else
            {
                Debug.LogError("[EnvironmentSpawner] �� ������� ����� ��� �������.");
                return null;
            }
        }
        */

        //private object FindingAFreePlaceForAnObject(GameObject prefab, float scale)
        //{
        //    // ��� ����
        //    if (!Physics.Raycast(GetRandomPointAbove(), Vector3.down, out RaycastHit hit, Mathf.Infinity, ~ignoreLayers))
        //        return null;

        //    // �������� ���� � ����� --- Checking layers and tags
        //    if (((1 << hit.collider.gameObject.layer) & blockedLayers) != 0) return null;
        //    if (blockedTags.Contains(hit.collider.tag)) return null;

        //    // �������� �������� ����� ��������� --- Checking for collisions before creation
        //    Vector3 checkSize = GetPrefabBounds(prefab) * scale * 0.5f;
        //    Collider[] overlaps = Physics.OverlapBox(hit.point, checkSize, Quaternion.identity);

        //    for (int i = 0; i < overlaps.Length; i++)
        //    {
        //        if (blockedTags.Contains(overlaps[i].tag)) return null;
        //        if (((1 << overlaps[i].gameObject.layer) & blockedLayers) != 0) return null;
        //    }
        //    return hit.point;

        //}

        private Vector3 GetRandomPointAbove()
        {
            Vector3 offset = new Vector3(
                Random.Range(-dimensions.x / 2, dimensions.x / 2),
                10f,
                Random.Range(-dimensions.y / 2, dimensions.y / 2)
            );
            //int hitCount = Physics.RaycastNonAlloc(transform.position + offset, Vector3.down, tempHit, Mathf.Infinity);

            return transform.position + offset;
        }

        private void GetPrefabBounds(GameObject prefab, ref  Vector3 halfExtents)
        {
            BoxCollider col = prefab.GetComponent<BoxCollider>();
            if (col != null) 
            {
                halfExtents= Vector3.Scale(col.size, col.transform.lossyScale)* 0.5f;
                ���������� ��� ��� ����� ���� �������� ���������� �������� �������
            }
            else
            {

                //Renderer rend = prefab.GetComponent<Renderer>(); 
                //    //return rend.bounds;
            }

            // return Vector3.one; // ���� ��� �������
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 0, 1, 0.25f);
            Gizmos.DrawCube(transform.position, new Vector3(dimensions.x, 0.1f, dimensions.y));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(dimensions.x, 0.1f, dimensions.y));
        }
#endif
    }

#if UNITY_EDITOR


        [CustomEditor(typeof(EnvironmentSpawner2))]
        [CanEditMultipleObjects]
        public class EnvironmentSpawnerEditor2 : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                EnvironmentSpawner2 script = (EnvironmentSpawner2)target;

                if (GUILayout.Button("Generate"))
                {
                    script.Spawn(script.numberOfObjThatNeedToSpawn);
                }
                if (GUILayout.Button("Destroy all in parent object"))
                {
                    script.Clear();
                }
                EditorUtility.SetDirty(this);
                EditorUtility.SetDirty(script);
            }
            public void OnEditorGUI()
            {
                EnvironmentSpawner scriptg = (EnvironmentSpawner)target;

            }

            public void OnInspectorUpdate()
            {
                Repaint();
            }

        }
#endif
}
