using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RescueRun
{
    [Serializable]
    public class AnimalData
    {
        public GameObject animalObj;
        public Vector2 position;
    }
    [Serializable]
    public class Animals
    {
        public List<AnimalData> animals;
    }

}
