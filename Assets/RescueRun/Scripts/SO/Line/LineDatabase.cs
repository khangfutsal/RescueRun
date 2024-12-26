using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watermelon;
namespace RescueRun
{
    [CreateAssetMenu(menuName = "SO/LineDatabase")]
    public class LineDatabase : ScriptableObject
    {
        [SerializeField] private Lines line;

        public Lines Line => line;
   
    }
}

