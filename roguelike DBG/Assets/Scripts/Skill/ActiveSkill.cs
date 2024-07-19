using System.Collections.Generic;
using System.Linq;
using Character;
using Character.Player;
using UnityEngine;

namespace Skill
{
    [CreateAssetMenu(menuName = "Skill/ActiveSkill")]
    public class ActiveSkill : SkillBase
    {
        public CharacterBase target;
        public float cost;
        public float damage;
        public float[] damageFactor = new float[13];
        public float preparationTime; // 使用技能前需要的时间（受AGI影响）
        
        public List<Buff.Buff> appliedBuff;

        public void Trigger()
        {
            if (target == null)
            {
                if (BattleManager.Instance.selectedCharacter != null)
                    target = BattleManager.Instance.selectedCharacter;
                else
                    target = BattleManager.Instance.RandomTarget();
                
                if (target == null) return;
            }
            
            if (source.info.stat.CurrentMp < cost)
            {
                source.info.stat.CurrentHp -= (cost - source.info.stat.CurrentMp);
                source.info.stat.CurrentMp = 0;
            }
            else
            {
                source.info.stat.CurrentMp -= cost;
            }
            // Debug.Log($"{source} | {source.info.stat.CurrentMp} | {source.info.stat.CurrentMp / source.info.stat.MaxMp}");
            if (target.info.stat.CurrentHp < 0 && target != PlayerManager.Instance.CurrentCharacter)
            {
                target = BattleManager.Instance.RandomTarget();
            }
            
            // Debug.Log($"{source} | {target}");
            
            var finalDamage = damage;
            var i = 0;
            foreach (var factor in damageFactor)
            {
                finalDamage += factor * target.info.stat.statValue[i].Value;
                i++;
            }
            // var finalDamage = damageFactor.Select((factor, i) => factor * target.info.stat.statValue[i].Value).Sum();
            
            foreach (var effect in target.buffs.SelectMany(buff => buff.effects))
            {
                effect?.TriggerPassive(this, ref finalDamage);
            }
            // TODO: 动画
            target.info.stat.CurrentHp -= finalDamage;
            
            if (finalDamage > 0) target.OnHurt();
            if (finalDamage < 0) target.OnHeal();
            
            if (target.info.stat.CurrentHp <= 0)
            {
                target.OnDie();
                return;
            }
            
            foreach (var buff in appliedBuff)
            {
                target.buffManager.AddBuff(buff);
            }
        }

        public float CalTimeCost()
        {
            return preparationTime - source.info.stat.statValue[8].Value;
        }
    }
}