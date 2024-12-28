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

            if (levels[level].totalSpawnObstacles != null)
            {
                return levels[level].totalSpawnObstacles;
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

