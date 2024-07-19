using System;
using System.Collections.Generic;
using Buff;
using Skill;
using UnityEngine;
using Utility.Attribute;

namespace Character
{
    [System.Serializable]
    public abstract class CharacterBase : ScriptableObject
    {
        [ScriptableObjectField(typeof(CharacterInfo))]
        public CharacterInfo info;
        
        public readonly BuffManager buffManager = new BuffManager();
        // 可使用的技能
        public List<SkillBase> skills = new List<SkillBase>();
        
        public int activeSkillCount = 2;
        protected SkillBase[] _preActiveSkills = new SkillBase[10];
        public SkillBase[] PreActiveSkills => _preActiveSkills;
        
        public int passiveSkillCount = 1;
        protected SkillBase[] _prePassiveSkills = new SkillBase[10];
        public SkillBase[] PrePassiveSkills => _prePassiveSkills;

        [NonSerialized]
        public List<Buff.Buff> buffs = new List<Buff.Buff>();

        public abstract void OnDie();
        public abstract void OnHurt();
        public abstract void OnHeal();

        public abstract CharacterBase Init();
        
        public CharacterBase DeepCopy()
        {
            var clone = Instantiate(this);
            clone.info = Instantiate(this.info);
            
            clone.skills = new List<SkillBase>(skills.Count);
            foreach (var skill in skills)
            {
                clone.skills?.Add(Instantiate(skill));
            }
            
            clone._preActiveSkills = new SkillBase[10];
            clone._prePassiveSkills = new SkillBase[10];

            if (buffs == null)
                clone.buffs = new List<Buff.Buff>();
            else
            {
                clone.buffs = new List<Buff.Buff>(buffs.Count);
                foreach (var buff in buffs)
                {
                    clone.buffs?.Add(Instantiate(buff));
                }
            }
            
            return clone;
        }
        
        public void OnNextTurn()
        {
            info.stat.CurrentMp = Mathf.Min(info.stat.MaxMp, info.stat.CurrentMp + 1);
        }
    }
}
