using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watermelon;
namespace RescueRun
{
    [CreateAssetMenu(menuName ="SO/BuffDatabase")]
    public class BuffDatabase : ScriptableObject
    {
        [SerializeField] private List<BuffData> buffs;
        public List<BuffData> Buffs => buffs;

        public BuffData GetBuffByName(string name)
        {
            for(int i = 0; i < buffs.Count; i++)
            {
                BuffData buff = buffs[i];
                if (buff.name == name) return buff;
            }
            return buffs.GetRandomItem();
        }

    }
}

