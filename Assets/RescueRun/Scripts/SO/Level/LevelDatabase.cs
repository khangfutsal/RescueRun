using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watermelon;

namespace RescueRun
{
    [CreateAssetMenu(menuName = "SO/LevelDatabase")]
    public class LevelDatabase : ScriptableObject
    {
        [SerializeField] private List<LevelData> levels;
        [SerializeField] private List<ObstaclesData> obstaclesByLevel;

        public List<LevelData> Levels => levels;

        public int GetTotalSpawnedByLevel(int level)
        {
            for (int i = 0; i < levels.Count; i++)
            {
                if (int.Parse(levels[i].level) == level)
                {
                    return levels[i].totalSpawnObstacles;
                }

            }
            return levels.GetRandomItem().totalSpawnObstacles;
        }

        public List<GameObject> GetObstaclesByLevel(int level) // 4
        {
            int allLevel = levels.Count / obstaclesByLevel.Count; // 10/3 = 3

            for (int i = 0; i < obstaclesByLevel.Count; i++)
            {
                if (level <= (i + 1) * allLevel)
                {
                    return obstaclesByLevel[i].obstacles;
                }
            }
            return null;
        }

    }
}

