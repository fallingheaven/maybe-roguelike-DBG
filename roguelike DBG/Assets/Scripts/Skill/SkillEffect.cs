using System;
using Character;
using UnityEngine;
using Utility;

namespace Skill
{
    
    [CreateAssetMenu(menuName = "Effects/ChangeDamageEffect")]
    public class ChangeDamageEffect : EffectBase
    {
        public CalEnum changeMode;
        public float changeAmount;
        
        public override void ApplyEffect(CharacterBase source, CharacterBase target, ref ActiveSkill skill)
        {
            MathMethod.Cal(ref skill.damage, changeAmount, changeMode, false);
        }
    }

    [CreateAssetMenu(menuName = "Effects/ChangeStatEffect")]
    public class ChangeStatEffect : EffectBase
    {
        public int statIndex;
        public CalEnum changeMode;
        public float changeAmount;
        public bool reverse;

        public override void ApplyEffect(CharacterBase source, CharacterBase target, ref ActiveSkill skill)
        {
            if (currentCount <= 0) return;
            
            MathMethod.Cal(ref target.info.stat.statValue[statIndex].Value, changeAmount, changeMode, reverse);
        }
    }

    [CreateAssetMenu(menuName = "Effects/DealDamageEffect")]
    public class DealDamageEffect : EffectBase
    {
        public int damageAmount;

        public override void ApplyEffect(CharacterBase source, CharacterBase target, ref ActiveSkill skill)
        {
            target.info.stat.CurrentHp -= damageAmount;
        }
    }

    [CreateAssetMenu(menuName = "Effects/ApplyBuffEffect")]
    public class ApplyBuffEffect : EffectBase
    {
        public Buff.Buff buff;

        public override void ApplyEffect(CharacterBase source, CharacterBase target, ref ActiveSkill skill)
        {
            target.buffManager.AddBuff(buff);
        }
    }

    [CreateAssetMenu(menuName = "Effects/UseSkillEffect")]
    public class UseSkillEffect : EffectBase
    {
        public ActiveSkill usedSkill;

        public override void ApplyEffect(CharacterBase source, CharacterBase target, ref ActiveSkill skill)
        {
            usedSkill.source = source;
            usedSkill.target = target;
            BattleManager.Instance.InsertSkill(usedSkill);
        }
    }
}