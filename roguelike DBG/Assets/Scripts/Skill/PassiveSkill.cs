using System.Collections.Generic;
using Event;
using Event.Events;
using UnityEngine;
using Utility;
using Utility.Interface;

namespace Skill
{
    [CreateAssetMenu(menuName = "Skill/PassiveSkill")]
    public class PassiveSkill : SkillBase
    {
        public float cost;
        public List<EffectBase> effects;

        public void Init()
        {
            EventManager.Instance.SubscribeEvent<BeforeAttackEvent>(BeforeAttack);
            EventManager.Instance.SubscribeEvent<AfterAttackEvent>(AfterAttack);
        }

        public void DisInit()
        {
            EventManager.Instance.UnsubscribeEvent<BeforeAttackEvent>(BeforeAttack);
            EventManager.Instance.UnsubscribeEvent<AfterAttackEvent>(AfterAttack);
        }
        
        private void BeforeAttack(IEventMessage message)
        {
            if (message is not BeforeAttackEvent msg) return;

            if (mode == SkillMode.Null || type == SkillType.Null ||
                msg.skill.mode == mode || msg.skill.type == type)
            {
                foreach (var effect in effects)
                {
                    effect.ApplyEffect(msg.skill.target, msg.skill.source, ref msg.skill);
                }
            }
        }
        
        private void AfterAttack(IEventMessage message)
        {
            if (message is not BeforeAttackEvent msg) return;

            if (mode == SkillMode.Null || type == SkillType.Null ||
                msg.skill.mode == mode || msg.skill.type == type)
            {
                foreach (var effect in effects)
                {
                    effect.ApplyEffect(msg.skill.target, msg.skill.source, ref msg.skill);
                }
            }
        }
    }
}