using UnityEngine;
using System.Collections.Generic;

namespace Lib
{
    public class ObjectPool : Singleton<ObjectPool>
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
            public GameObject parent;
        }

        public List<Pool> pools;
        public List<GameObject> enemyList = new List<GameObject>();
        public List<GameObject> bulletList = new List<GameObject>();
        public Dictionary<string, Queue<GameObject>> poolDictionary;

        void Awake()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    if (obj == null)
                    {
                        Debug.LogError("Null ");
                    }

                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                    obj.transform.SetParent(pool.parent.transform);
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
                return null;
            }

            if (poolDictionary[tag].Count == 0)
            {
                GameObject newObj = Instantiate(pools.Find(pool => pool.tag == tag).prefab);
                newObj.SetActive(false);
                poolDictionary[tag].Enqueue(newObj);
                newObj.transform.SetParent(pools.Find(pool => pool.tag == tag).parent.transform);
            }

            GameObject objectToSpawn = poolDictionary[tag].Dequeue();
            if (objectToSpawn == null)
            {
                Debug.LogError($"Object with tag {tag} is null. Removing from pool.");
                return null;
            }

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();
            if (pooledObject != null)
            {
                pooledObject.OnObjectSpawn();
            }

            return objectToSpawn;
        }


        public void ReturnToPool(string tag, GameObject objectToReturn)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
                objectToReturn.SetActive(false);
                poolDictionary[tag].Enqueue(objectToReturn);
                return;
            }

            objectToReturn.SetActive(false);
            poolDictionary[tag].Enqueue(objectToReturn);
        }
    }
}