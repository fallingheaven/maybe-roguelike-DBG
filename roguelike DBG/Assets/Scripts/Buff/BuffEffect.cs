using System;
using System.Collections.Generic;
using Character;
using Skill;
using UnityEngine;
using Utility;

namespace Buff
{
    public interface IBuffEffect
    {
        public void Trigger(CharacterBase carrier);

        public void TriggerPassive(SkillBase skill, ref float num);

        public void OnExitEffect();
    }

    public class PassiveEffect : ScriptableObject, IBuffEffect
    {
        public Action triggerAction;
        public Action passiveTriggerAction;
        public Action onExitAction;
        
        public virtual void Trigger(CharacterBase carrier)
        {
            triggerAction?.Invoke();
        }

        public virtual void TriggerPassive(SkillBase skill, ref float num)
        {
            passiveTriggerAction?.Invoke();
        }

        public virtual void OnExitEffect()
        {
            onExitAction?.Invoke();
        }
    }

    public class ActiveEffect : ScriptableObject, IBuffEffect
    {
        public Action triggerAction;
        public Action passiveTriggerAction;
        public Action onExitAction;
        
        public virtual void Trigger(CharacterBase carrier)
        {
            triggerAction?.Invoke();
        }

        public virtual void TriggerPassive(SkillBase skill, ref float num)
        {
            passiveTriggerAction?.Invoke();
        }

        public virtual void OnExitEffect()
        {
            onExitAction?.Invoke();
        }
    }

    public class StatPassiveChangeEffect : PassiveEffect
    {
        public StatEnum changeStat;
        public CalEnum calEnum;
        public float changeNum;
        public SkillType checkedType = SkillType.Null;
        public SkillMode checkedMode = SkillMode.Null;

        private bool _triggered = false;
        private CharacterBase _source;
            
        public override void Trigger(CharacterBase carrier)
        {
            base.Trigger(carrier);
        }

        public override void TriggerPassive(SkillBase skill, ref float damage)
        {
            if ((checkedType != SkillType.Null && (checkedType != skill.type)) ||
                (checkedMode != SkillMode.Null && (checkedMode != skill.mode))) return;
            
            if (changeStat == StatEnum.DAMAGE)
                Cal(ref damage);
            else
            {
                Cal(ref skill.source.info.stat.statValue[(int)changeStat]);
                _source = skill.source;
                _triggered = true;
            }
        }

        public override void OnExitEffect()
        {
            if (_triggered)
            {
                ref var num = ref _source.info.stat.statValue[(int)changeStat];
                num = calEnum switch
                {
                    CalEnum.Plus => new Pair<string, float>(num.Key, num.Value - changeNum),
                    CalEnum.Minus => new Pair<string, float>(num.Key, num.Value + changeNum),
                    CalEnum.Multiply => new Pair<string, float>(num.Key, num.Value / changeNum),
                    CalEnum.Divide => new Pair<string, float>(num.Key, num.Value * changeNum),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            _triggered = false;
        }

        private void Cal(ref float num)
        {
            switch (calEnum)
            {
                case CalEnum.Plus:
                    num += changeNum;
                    break;
                case CalEnum.Minus:
                    num -= changeNum;
                    break;
                case CalEnum.Multiply:
                    num *= changeNum;
                    break;
                case CalEnum.Divide:
                    num /= changeNum;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Cal(ref Pair<string, float> num)
        {
            num = calEnum switch
            {
                CalEnum.Plus => new Pair<string, float>(num.Key, num.Value + changeNum),
                CalEnum.Minus => new Pair<string, float>(num.Key, num.Value - changeNum),
                CalEnum.Multiply => new Pair<string, float>(num.Key, num.Value * changeNum),
                CalEnum.Divide => new Pair<string, float>(num.Key, num.Value / changeNum),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public class StatChangeEffect : ActiveEffect
    {
        public StatEnum changeStat;
        public CalEnum calEnum;
        public float changeNum;
        public bool multiTrigger;
        public int maxTrigger;
        public int currentTrigger;
        private bool _triggered = false;
        private CharacterBase _source;
            
        public override void Trigger(CharacterBase carrier)
        {
            if (_triggered && !multiTrigger) return;
            if (currentTrigger >= maxTrigger) return;
            
            Cal(ref carrier.info.stat.statValue[(int)changeStat]);
            _source = carrier;
            _triggered = true;
            currentTrigger++;
        }

        public override void OnExitEffect()
        {
            if (_triggered)
            {
                ref var num = ref _source.info.stat.statValue[(int)changeStat];
                num = calEnum switch
                {
                    CalEnum.Plus => new Pair<string, float>(num.Key, num.Value - changeNum),
                    CalEnum.Minus => new Pair<string, float>(num.Key, num.Value + changeNum),
                    CalEnum.Multiply => new Pair<string, float>(num.Key, num.Value / changeNum),
                    CalEnum.Divide => new Pair<string, float>(num.Key, num.Value * changeNum),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            _triggered = false;
            currentTrigger = 0;
        }

        private void Cal(ref float num)
        {
            switch (calEnum)
            {
                case CalEnum.Plus:
                    num += changeNum;
                    break;
                case CalEnum.Minus:
                    num -= changeNum;
                    break;
                case CalEnum.Multiply:
                    num *= changeNum;
                    break;
                case CalEnum.Divide:
                    num /= changeNum;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Cal(ref Pair<string, float> num)
        {
            num = calEnum switch
            {
                CalEnum.Plus => new Pair<string, float>(num.Key, num.Value + changeNum),
                CalEnum.Minus => new Pair<string, float>(num.Key, num.Value - changeNum),
                CalEnum.Multiply => new Pair<string, float>(num.Key, num.Value * changeNum),
                CalEnum.Divide => new Pair<string, float>(num.Key, num.Value / changeNum),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public class DamagePerTurnEffect : ActiveEffect
    {
        public float damage;
        public StatEnum changeStat;
        public float changeNum;
        public CalEnum calEnum;
        
        public bool isFatal;

        public override void Trigger(CharacterBase carrier)
        {
            var res = carrier.info.stat.statValue[(int)changeStat];
            if (res.Value < 0 && !isFatal) return;

            carrier.info.stat.statValue[(int)changeStat] = res;
        }
        
        private KeyValuePair<string, float> Cal(KeyValuePair<string, float> num)
        {
            return calEnum switch
            {
                CalEnum.Plus => new KeyValuePair<string, float>(num.Key, num.Value + changeNum),
                CalEnum.Minus => new KeyValuePair<string, float>(num.Key, num.Value - changeNum),
                CalEnum.Multiply => new KeyValuePair<string, float>(num.Key, num.Value * changeNum),
                CalEnum.Divide => new KeyValuePair<string, float>(num.Key, num.Value / changeNum),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}