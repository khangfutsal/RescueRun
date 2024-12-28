using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Lib;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using Watermelon;
namespace RescueRun
{
    public class LevelController : Lib.Singleton<LevelController>
    {
        [SerializeField] private LevelDatabase levelDatabase;
        [SerializeField] private AnimalDatabase animalDatabase;

        [SerializeField] private List<Animal> animals;
        [SerializeField] private List<GameObject> obstaclesObj;


        [Header("About Level")]
        [SerializeField] private int currentLevel = 0;

        public float raycastDistance = 100f;
        public LayerMask spawnedObjectLayer;

        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float minZ;
        [SerializeField] private float maxZ;

        public List<Animal> Animals
        {
            get { return animals; }
        }

        public void Initialize()
        {
            if (PlayerPrefs.HasKey("Level"))
            {
                currentLevel = PlayerPrefs.GetInt("Level");
            }

            InitAnimal();
            InitMapLevel();

        }

        private void InitMapLevel()
        {
            int prevLevel = 0;
            if (PlayerPrefs.HasKey("PrevLevel"))
            {
                prevLevel = PlayerPrefs.GetInt("PrevLevel");
            }
            if (PlayerPrefs.HasKey("Level"))
            {
                currentLevel = PlayerPrefs.GetInt("Level");
            }
            Debug.Log(currentLevel + " with :" + prevLevel);
            int totalByLevel = levelDatabase.GetTotalSpawnedByLevel(currentLevel);
            Debug.Log("Total level :" + totalByLevel);
            if (prevLevel == currentLevel) // Old level
            {
                Vector3 vOffset = new Vector3(0.5f, 0.5f, 0.5f);
                int seed = GameController.Instance.GetSeed();
                Random.InitState(seed);
                for (int i = 0; i < totalByLevel; i++)
                {
                    Vector3 randPosition = new Vector3(Random.Range(minX, maxX) + Random.Range(-vOffset.x, vOffset.x), 1, Random.Range(minZ, maxZ) + Random.Range(-vOffset.z, vOffset.z));
                    Debug.Log(randPosition);
                    CheckPositionToSpawn(randPosition);
                }

            }
            else if (prevLevel != currentLevel)
            {
                for (int i = 0; i < totalByLevel; i++)
                {
                    Vector3 randPosition = new Vector3(Random.Range(minX, maxX), 1, Random.Range(minZ, maxZ));
                    Debug.Log(randPosition);
                    CheckPositionToSpawn(randPosition);
                }
            }




        }

        public void CheckPositionToSpawn(Vector3 v)
        {
            RaycastHit hit;

            if (Physics.Raycast(v, Vector3.down, out hit, raycastDistance))
            {
                Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                List<GameObject> obstacles = levelDatabase.GetObstaclesByLevel(currentLevel - 1);
                int randomIndex = Random.Range(0, obstacles.Count);

                BoxCollider boxCollider = obstacles[randomIndex].GetComponent<BoxCollider>();
                Vector3 overlapTestBoxScale = new Vector3(boxCollider.bounds.size.x, boxCollider.bounds.size.y, boxCollider.bounds.size.z);
                Collider[] collidersInsideOverlapBox = new Collider[10];
                int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(hit.point, overlapTestBoxScale / 2, collidersInsideOverlapBox, spawnRotation, spawnedObjectLayer);

                if (numberOfCollidersFound == 0)
                {
                    Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                    Quaternion finalRotation = spawnRotation * randomRotation;
                    Spawn(hit.point, finalRotation, obstacles[randomIndex]);
                }
            }
        }

        void Spawn(Vector3 positionToSpawn, Quaternion rotationToSpawn, GameObject obj)
        {
            obstaclesObj.Add(ObjectPool.Instance.SpawnFromPool(obj.tag, positionToSpawn, rotationToSpawn));
        }

        private void InitAnimal()
        {
            var animalsData = animalDatabase.GetAnimalByIndex(currentLevel - 1);

            for (int i = 0; i < animalsData.animals.Count; i++)
            {
                Debug.Log("I " + i);
                var animalData = animalsData.animals[i];
                Vector3 pos = new Vector3(animalData.position.x, 0, animalData.position.y);
                GameObject obj = ObjectPool.Instance.SpawnFromPool(animalsData.animals[i].animalObj.tag, pos, Quaternion.identity);
                if (obj != null)
                {
                    animals.Add(obj.GetComponent<Animal>());
                }
            }
        }

        public void DestroyLevel()
        {
            Debug.Log("Destroy Level");
            for (int i = 0; i < animals.Count; i++)
            {
                if (animals[i].gameObject.activeSelf)
                {
                    Debug.Log("Animalssss");
                    ObjectPool.instance.ReturnToPool(animals[i].gameObject.tag, animals[i].gameObject);
                }
            }

            for (int i = 0; i < obstaclesObj.Count; i++)
            {
                if (obstaclesObj[i].activeSelf)
                {
                    Debug.Log("Obstaclessssk");
                    Debug.Log("Obstaclessssk");
                    ObjectPool.instance.ReturnToPool(obstaclesObj[i].tag, obstaclesObj[i].gameObject);
                }

            }

            animals.Clear();
            obstaclesObj.Clear();
        }

        public Animal GetCurrentAnimalTarget()
        {
            for (int i = 0; i < animals.Count; i++)
            {
                if (!animals[i].isCollected) return animals[i];
            }
            return null;
        }
    }
}

