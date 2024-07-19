using System.Collections.Generic;
using System.Linq;
using FSM.TurnStateNode;
using UnityEngine;
using Utility.Interface;

namespace FSM
{
    public class StateMachine
    {
        private StateBase _currentState;
        private readonly Dictionary<StateBase, List<Transition>> _transitions = new Dictionary<StateBase, List<Transition>>();

        private EmptyState _emptyState;

        public StateMachine(EmptyState emptyState)
        {
            _emptyState = emptyState;
            _currentState = emptyState;
        }
        
        public void Update()
        {
            // foreach (var transition in _transitions.SelectMany(pair => pair.Value))
            // {
            //     Debug.Log(transition.Condition);
            // }
            if (!_transitions.ContainsKey(_currentState)) return;
            
            // Debug.Log(_currentState);
            
            // 查找满足条件的优先级最高的目标状态
            var transferableState = _transitions[_currentState].Where(transition => transition.Condition.IsConditionMet());
            var transitions = transferableState.ToList();
            if (transitions.Count < 1) return;
            
            // foreach (var t in transitions)
            // {
            //     Debug.Log($"{_currentState}, {t.ToState}, {t.Condition}, {t.Condition.IsConditionMet()}");
            // }
            var sortedTransition = transitions.Aggregate((max, next) => next.Priority > max.Priority ? next : max);
            
            if (sortedTransition != null)
            {
                _currentState = sortedTransition.ToState;
                _currentState.OnEnter();
            }
            
            _currentState.OnUpdate();
        }

        /// <summary>
        /// 更新状态，包括强制和非强制
        /// </summary>
        /// <param name="state"></param>
        /// <param name="force"></param>
        public void SetState(StateBase state, bool force = true)
        {
            if (!force)
            {
                if (_transitions[_currentState].Where(transition => transition.ToState == state)
                    .Any(transition => transition.Condition.IsConditionMet()))
                {
                    _currentState = state;
                    _currentState.OnEnter();
                }
            }
            else
            {
                _currentState = state;
                _currentState.OnEnter();
            }
        }

        /// <summary>
        /// 添加状态转移
        /// </summary>
        /// <param name="fromState"></param>
        /// <param name="toState"></param>
        /// <param name="condition"></param>
        /// <param name="priority"></param>
        public void AddTransition(StateBase fromState, StateBase toState, ITransitionCondition condition, int priority)
        {
            if (!_transitions.ContainsKey(fromState))
                _transitions[fromState] = new List<Transition>();
            
            _transitions[fromState].Add(new Transition(toState, condition, priority));
        }

        public void ResetState()
        {
            _currentState = _emptyState;
        }
    }

    public class Transition
    {
        public StateBase ToState { get; }
        public ITransitionCondition Condition { get; }
        public int Priority { get; }

        public Transition(StateBase state, ITransitionCondition condition, int priority)
        {
            ToState = state;
            Condition = condition;
            Priority = priority;
        }
    }
}