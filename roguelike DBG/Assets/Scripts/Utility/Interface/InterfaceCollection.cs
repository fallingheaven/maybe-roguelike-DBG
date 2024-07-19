using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Utility.Interface
{
    /// <summary>
    /// 事件消息接口
    /// </summary>
    public interface IEventMessage
    {
        
    }

    /// <summary>
    /// 状态机节点接口
    /// </summary>
    public abstract class StateBase
    {
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
        
        public StateBase nextState;
    }
    
    /// <summary>
    /// 状态转移条件判断接口
    /// </summary>
    public interface ITransitionCondition
    {
        public bool IsConditionMet();
    }

    /// <summary>
    /// 复合状态转移条件判断抽象类
    /// </summary>
    public abstract class CompositeTransitionCondition : ITransitionCondition
    {
        public readonly List<ITransitionCondition> subConditions;

        protected CompositeTransitionCondition(List<ITransitionCondition> conditions)
        {
            this.subConditions = conditions;
        }
        
        public virtual bool IsConditionMet()
        {
            return subConditions.All(subCondition => subCondition.IsConditionMet());
        }
    }
}
