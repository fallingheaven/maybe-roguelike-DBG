using Character;
using UnityEngine;

namespace Skill
{
    public abstract class EffectBase : ScriptableObject
    {
        public int applyCount;
        public int currentCount;
        
        public abstract void ApplyEffect(CharacterBase source, CharacterBase target, ref ActiveSkill skill);
    }
}