using System;
using Event;
using Event.Events;
using Skill;
using UnityEngine;
using Utility;
using Utility.Interface;

namespace Character.Player
{
    [CreateAssetMenu(menuName = "Character/Player")]
    public class PlayerCharacter : CharacterBase
    {
        public override CharacterBase Init()
        {
            buffManager.carrier = this;
            EventManager.Instance.SubscribeEvent<ChooseSkillEvent>(ChooseSkill);
            
            return this;
        }

        private void ChooseSkill(IEventMessage message)
        {
            if (message is not ChooseSkillEvent msg) return;
            var skill = msg.skill;
            var index = msg.index;
            
            // Debug.Log(skill.skillName);
            if (index > activeSkillCount && skill.type == SkillType.Passive)
            {
                var passiveSkill = Instantiate(skill) as PassiveSkill;
                if (passiveSkill != null) passiveSkill.source = this;
                _prePassiveSkills[index - activeSkillCount] = passiveSkill;
            }
            else
            {
                var activeSkill = Instantiate(skill) as ActiveSkill;
                if (activeSkill != null)
                {
                    activeSkill.source = this;
                    activeSkill.target = msg.target;
                }
                _preActiveSkills[index] = activeSkill;
            }
        }

        public override void OnDie()
        {
            BattleManager.Instance.playerAlive = false;
        }

        public override void OnHurt()
        {
            
        }

        public override void OnHeal()
        {
            
        }
    }
}