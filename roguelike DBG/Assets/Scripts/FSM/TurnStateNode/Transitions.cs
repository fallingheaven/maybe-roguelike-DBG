using UnityEngine;
using Utility.Interface;

namespace FSM.TurnStateNode
{
    public class IsEnterBattleCondition : ITransitionCondition
    {
        public bool IsConditionMet()
        {
            return BattleManager.Instance.battling;
        }
    }
    public class IsPlayerDeadCondition : ITransitionCondition
    {
        public bool IsConditionMet()
        {
            // Debug.Log(!BattleManager.Instance.playerAlive);
            return !BattleManager.Instance.playerAlive;
        }
    }

    public class IsEnemyClearCondition : ITransitionCondition
    {
        public bool IsConditionMet()
        {
            // Debug.Log(BattleManager.Instance.EnemyCleared);
            return BattleManager.Instance.EnemyCleared;
        }
    }

    public class IsPlayerPreparedCondition : ITransitionCondition
    {
        public bool IsConditionMet()
        {
            return BattleManager.Instance.Prepared;
        }
    }
    
    public class IsBattleFinishCondition : ITransitionCondition
    {
        public bool IsConditionMet()
        {
            return BattleManager.Instance.BattleFinished;
        }
    }
    
    public class IsSettleFinishCondition : ITransitionCondition
    {
        public bool IsConditionMet()
        {
            // Debug.Log(BattleManager.Instance.SettleEnd);
            return BattleManager.Instance.SettleEnd;
        }
    }
}