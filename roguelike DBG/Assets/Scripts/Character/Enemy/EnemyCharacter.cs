using System;
using System.Linq;
using Character.Player;
using Event;
using Event.Events;
using Skill;
using UnityEngine;
using Utility.Interface;
using Random = System.Random;

namespace Character.Enemy
{
    [CreateAssetMenu(menuName = "Character/Enemy")]
    public class EnemyCharacter : CharacterBase
    {
        public Action onDieEvent;
        public Action onHurtEvent;
        public Action onHealEvent;
        
        public EnemyCharacter(EnemyCharacter enemy)
        {
            info = CreateInstance<CharacterInfo>();
        }

        public override CharacterBase Init()
        {
            EventManager.Instance.SubscribeEvent<OnPreparationEvent>(PrepareAction);
            return this;
        }

        public void Deinit()
        {
            EventManager.Instance.UnsubscribeEvent<OnPreparationEvent>(PrepareAction);
        }
        
        public override void OnDie()
        {
            onDieEvent?.Invoke();
        }

        public override void OnHurt()
        {
            onHurtEvent?.Invoke();
        }

        public override void OnHeal()
        {
            onHealEvent?.Invoke();
        }
        
        private void PrepareAction(IEventMessage message)
        {
            // Debug.Log("prepare skill");
            
            for (var i = 0; i < activeSkillCount; i++) PreActiveSkills[i] = null;
            for (var i = 0; i < passiveSkillCount; i++) PrePassiveSkills[i] = null;
            
            var mpSum = 0f;
            var minActiveMp = skills
                .OfType<ActiveSkill>()
                .Aggregate(((skill, min) => min.cost < skill.cost? min : skill))
                .cost;
            var minPassiveMp = skills
                .OfType<ActiveSkill>()
                .Aggregate(((skill, min) => min.cost < skill.cost? min : skill))
                .cost;
            
            for (var i = 1; i <= new Random().Next(activeSkillCount + 1); i++)
            {
                if (minActiveMp + mpSum > info.stat.CurrentMp) break;

                var availableSkills = skills
                    .OfType<ActiveSkill>()
                    .Where(skill => skill.cost + mpSum <= info.stat.CurrentMp)
                    .ToArray();
                
                var skill = availableSkills[new Random().Next(availableSkills.Length)];
                var activeSkill = Instantiate(skill);
                activeSkill.source = this;
                activeSkill.target = PlayerManager.Instance.CurrentCharacter;
                PreActiveSkills[i - 1] = activeSkill;
                mpSum += skill.cost;
                // Debug.Log(skill);
            }
            
            for (var i = 1; i <= new Random().Next(passiveSkillCount + 1); i++)
            {
                if (minPassiveMp + mpSum > info.stat.CurrentMp) break;

                var availableSkills = skills
                    .OfType<PassiveSkill>()
                    .Where(skill => skill.cost + mpSum <= info.stat.CurrentMp)
                    .ToArray();
                
                var skill = availableSkills[new Random().Next(availableSkills.Length)];
                var passiveSkill = Instantiate(skill);
                passiveSkill.source = this;
                PrePassiveSkills[i - 1] = passiveSkill;
                mpSum += skill.cost;
                // Debug.Log(skill);
            }
            
            EventManager.Instance.Invoke(new EnemySkillPreparedEvent()
            {
                enemy = this
            });
        }
    }
}