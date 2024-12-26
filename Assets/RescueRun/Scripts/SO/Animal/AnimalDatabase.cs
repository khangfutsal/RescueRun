using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watermelon;
namespace RescueRun
{
    [CreateAssetMenu(menuName ="SO/AnimalDatabase")]
    public class AnimalDatabase : ScriptableObject
    {
        public List<Animals> animalDatabases;

        public Animals GetAnimalByIndex(int index)
        {
            if(animalDatabases.IsInRange(index)) return animalDatabases[index];
            return animalDatabases.GetRandomItem();
        }
    }
}

