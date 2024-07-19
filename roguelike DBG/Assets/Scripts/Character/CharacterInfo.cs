using System;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Utility.Attribute;

namespace Character
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "characterInfo")]
    public class CharacterInfo : ScriptableObject
    {
        public string characterName;
        public Sprite headPortrait;
        public Sprite battleSprite;
        public EntityType type;
        
        public CharacterStat stat = new CharacterStat(1, 1, 1, 1, 1, 1, 1, 1.0f, 0.0f, 0.02f, 0.0f);
    }

    [Serializable]
    public struct Pair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;
        
        public Pair(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }
        
        public TValue this[TKey key]
        {
            get => Value;
            set => Value = value;
        }
    }
    
    [System.Serializable]
    public struct CharacterStat
    {
        public Pair<string, float>[] statValue;
        [NonSerialized]
        public string[] statName;

        public float MaxHp
        {
            get => statValue[0].Value;
            set => statValue[0] = new Pair<string, float>(statValue[0].Key, value);
        }
        public float CurrentHp
        {
            get => statValue[1].Value;
            set => statValue[1] = new Pair<string, float>(statValue[1].Key, value);
        }
        public float MaxMp
        {
            get => statValue[2].Value;
            set => statValue[2] = new Pair<string, float>(statValue[2].Key, value);
        }
        public float CurrentMp
        {
            get => statValue[3].Value;
            set => statValue[3] = new Pair<string, float>(statValue[3].Key, value);
        }
        public float ATK
        {
            get => statValue[4].Value;
            set => statValue[4] = new Pair<string, float>(statValue[4].Key, value);
        }
        public float MAT
        {
            get => statValue[5].Value;
            set => statValue[5] = new Pair<string, float>(statValue[5].Key, value);
        }
        public float DEF
        {
            get => statValue[6].Value;
            set => statValue[6] = new Pair<string, float>(statValue[6].Key, value);
        }
        public float MDF
        {
            get => statValue[7].Value;
            set => statValue[7] = new Pair<string, float>(statValue[7].Key, value);
        }
        public float AGI
        {
            get => statValue[8].Value;
            set => statValue[8] = new Pair<string, float>(statValue[8].Key, value);
        }

        public float HIT
        {
            get => statValue[9].Value;
            set => statValue[9] = new Pair<string, float>(statValue[9].Key, value);
        } // 命中率
        public float DODGE
        {
            get => statValue[10].Value;
            set => statValue[10] = new Pair<string, float>(statValue[10].Key, value);
        } // 闪避率
        public float CRIT
        {
            get => statValue[11].Value;
            set => statValue[11] = new Pair<string, float>(statValue[11].Key, value);
        } // 暴击率（暴击伤害默认1.5倍）
        
        public float DamageReductionRate
        {
            get => statValue[12].Value;
            set => statValue[12] = new Pair<string, float>(statValue[12].Key, value);
        }// 通用减伤率

        public CharacterStat(int hp, int mp, int atk, int mat, int def, int mdf, int agi, float hit, float dodge, float crit, float drr)
        {
            statValue = new Pair<string, float>[13]
            {
                new Pair<string, float>("MaxHP", hp),
                new Pair<string, float>("CurrentHP", hp),
                new Pair<string, float>("MaxMP", mp),
                new Pair<string, float>("CurrentMP", mp),
                new Pair<string, float>("ATK", atk),
                new Pair<string, float>("MAT", mat),
                new Pair<string, float>("DEF", def),
                new Pair<string, float>("MDF", mdf),
                new Pair<string, float>("AGI", agi),
                new Pair<string, float>("HIT", hit),
                new Pair<string, float>("DODGE", dodge),
                new Pair<string, float>("CRIT", crit),
                new Pair<string, float>("DRR", drr)
            };
            statName = new string[13];
            for (var i = 0; i < statValue.Length; i++)
                statName[i] = statValue[i].Key;
        }
    }
}