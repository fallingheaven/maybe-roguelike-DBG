using Event;
using Event.Events;
using UnityEngine;
using Utility.Interface;

namespace FSM.TurnStateNode
{
    public class EmptyState : StateBase
    {
        public override void OnEnter(){}
        
        public override void OnUpdate(){}

        public override void OnExit(){}
    }
        
    public class PreparationState : StateBase
    {
        public override void OnEnter()
        {
            Debug.Log("Prepare");
            BattleManager.Instance.Prepare();
            EventManager.Instance.Invoke(new OnPreparationEvent());
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            BattleManager.Instance.EndPrepare();
        }
    }

    public class BattleState : StateBase
    {
        public override void OnEnter()
        {
            Debug.Log("Battle");
            BattleManager.Instance.Battle();
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }
    
    public class SettleState : StateBase
    {
        public override void OnEnter()
        {
            Debug.Log("Settle");
            BattleManager.Instance.SettleSkill();
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }

    public class EndBattleState : StateBase
    {
        public override void OnEnter()
        {
            Debug.Log("End");
            BattleManager.Instance.EndBattle();
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }
}