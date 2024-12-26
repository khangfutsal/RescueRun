using System;
using System.Collections;
using System.Collections.Generic;
using RescueRun;
using UnityEngine;
namespace RescueRun
{
    [Serializable]
    public class BuffData
    {
        public string name;
        public Sprite sprite;
        public bool isPeaking;
        public List<BuffCurrency> currencies;
    }

}
