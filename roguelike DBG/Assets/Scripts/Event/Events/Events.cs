using Character;
using Character.Enemy;
using Skill;
using Utility.Interface;

namespace Event.Events
{
    public class OnPreparationEvent : IEventMessage
    {
        
    }

    public class EnemySkillPreparedEvent : IEventMessage
    {
        public EnemyCharacter enemy;
    }
    
    public class ChooseSkillEvent : IEventMessage
    {
        public SkillBase skill;
        public int index; // 在哪个技能栏
        public CharacterBase target = null;
    }

    public class BeforeAttackEvent : IEventMessage
    {
        public ActiveSkill skill;

        public BeforeAttackEvent(ActiveSkill skill)
        {
            this.skill = skill;
        }
    }

    public class AfterAttackEvent : IEventMessage
    {
        public ActiveSkill skill;
        
        public AfterAttackEvent(ActiveSkill skill)
        {
            this.skill = skill;
        }
    }

    public class BattleStartEvent : IEventMessage
    {
        public CharacterCollection collection;

        public BattleStartEvent(CharacterCollection collection)
        {
            this.collection = collection;
        }
    }
}