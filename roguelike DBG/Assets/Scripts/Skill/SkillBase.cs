using Character;
using UnityEngine;
using Utility;

namespace Skill
{
    public abstract class SkillBase : ScriptableObject
    {
        public string skillName;
        public SkillType type;
        public SkillMode mode;
        public ActionType actionType;
        public CharacterBase source;
    }
}