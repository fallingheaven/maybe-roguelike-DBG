using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Character;
using Character.Enemy;
using Character.Player;
using Event;
using Event.Events;
using FSM;
using FSM.TurnStateNode;
using Skill;
using UnityEngine;
using UnityEngine.UI;
using Utility.CustomClass;
using Random = System.Random;

public class BattleManager : Singleton<BattleManager>
{
    public int turnCount;
    public bool playerAlive;
    private StateMachine _battleStateMachine;
    private EmptyState _emptyState;
    private CharacterCollection _collection;
    private EnemySpawner _enemySpawner;
    private Stack<ActiveSkill> _skillToUse;
    private Stack<ActiveSkill> _skillToDeal; // 用于插队

    public CharacterBase selectedCharacter;
        
    // 战斗状态
    public bool battling;

    private bool _prepared;
    public bool Prepared => _prepared;

    public bool EnemyCleared
    {
        get
        {
            return !_collection.collection.Any(character => character is EnemyCharacter);
        }
    }

    private bool _battleFinished;
    public bool BattleFinished => _battleFinished;
    
    private bool _settleEnd;
    public bool SettleEnd => _settleEnd;

    private void Awake()
    {
        _emptyState = new EmptyState();
        _battleStateMachine = new StateMachine(_emptyState);
        _collection = new CharacterCollection();
        _enemySpawner = new EnemySpawner();
        _skillToUse = new Stack<ActiveSkill>();
        _skillToDeal = new Stack<ActiveSkill>();

        InitStateMachine();
    }

    private void InitStateMachine()
    {
        var preparationState = new PreparationState();
        var battleState = new BattleState();
        var settleState = new SettleState();
        var endBattleState = new EndBattleState();
        
        _battleStateMachine.AddTransition(preparationState, battleState, new IsPlayerPreparedCondition(), 1);
        
        _battleStateMachine.AddTransition(battleState, settleState, new IsBattleFinishCondition(), 1);
        
        _battleStateMachine.AddTransition(settleState, preparationState, new IsSettleFinishCondition(), 1);

        _battleStateMachine.AddTransition(settleState, endBattleState, new IsEnemyClearCondition(), 2);
        _battleStateMachine.AddTransition(settleState, endBattleState, new IsPlayerDeadCondition(), 2);

        _battleStateMachine.AddTransition(_emptyState, preparationState, new IsEnterBattleCondition(), 1);
    }

    private void Update()
    {
        _battleStateMachine.Update();
    }

    /// <summary>
    /// 新战斗开始后，更新回合信息
    /// </summary>
    public void OnNewBattle()
    {
        turnCount = 0;
        playerAlive = true;
        battling = true;
        
        _collection.Add(PlayerManager.Instance.CurrentCharacter);
        var enemyCount = new Random().Next(3) + 1;
        for (var i = 0; i < enemyCount; i++)
        {
            var enemy = _enemySpawner.SpawnEnemy();
            // Debug.Log(enemy);
            _collection.Add(enemy);
        }
        
        foreach (var character in _collection.collection)
        {
            for (var i = 0; i < character.PreActiveSkills.Length; i++)
            {
                character.PreActiveSkills[i] = null;
            }

            for (var i = 0; i < character.PrePassiveSkills.Length; i++)
            {
                character.PrePassiveSkills[i] = null;
            }
        }
        
        var message = new BattleStartEvent(_collection);
        EventManager.Instance.Invoke(message);
    }

    public void Prepare()
    {
        turnCount++;
        _prepared = false;
        _battleFinished = false;
        _settleEnd = false;

        foreach (var character in _collection.collection)
        {
            character.OnNextTurn();
        }
    }

    public void EndPrepare()
    {
        _prepared = true;
    }
    
    public void Battle()
    {
        ProcessPassiveSkill();
        ProcessActiveSkill();
        
        DealSkills();
    }

    private void ProcessPassiveSkill()
    {
        foreach (var skill in _collection.collection.SelectMany(character => character.PrePassiveSkills))
        {
            var passiveSkill = skill as PassiveSkill;
            if (passiveSkill != null) passiveSkill.Init();
        }
    }

    private void ProcessActiveSkill()
    {
        var skillList = _collection.collection
            .Where(character => character != null && character.PreActiveSkills != null)
            .SelectMany(character => character.PreActiveSkills)
            .OfType<ActiveSkill>()
            .OrderByDescending(skill => skill.CalTimeCost())
            .ToList();
        
        if (!skillList.Any()) return;

        _skillToUse = new Stack<ActiveSkill>(skillList);
    }


    private async void DealSkills()
    {
        while (_skillToUse.Count > 0)
        {
            if (EnemyCleared || !playerAlive) break;
                
            var skill = _skillToUse.Pop();
            _skillToDeal.Push(skill);
            
            var beforeAttackEvent = new BeforeAttackEvent(skill);
            await EventManager.Instance.InvokeEvent(beforeAttackEvent);

            while (_skillToDeal.Count > 0)
            {
                if (EnemyCleared || !playerAlive) break;
                var skillToDeal = _skillToDeal.Pop();
                skillToDeal.Trigger();
                await Task.Delay(500);
            }
            
            var afterAttackEvent = new AfterAttackEvent(skill);
            await EventManager.Instance.InvokeEvent(afterAttackEvent);
        }
        
        _battleFinished = true;
    }

    public void InsertSkill(ActiveSkill skill)
    {
        _skillToDeal.Push(skill);
    }

    public void SettleSkill()
    {
        foreach (var skill in _collection.collection.SelectMany(character => character.PrePassiveSkills))
        {
            var passiveSkill = skill as PassiveSkill;
            if (passiveSkill != null)passiveSkill.DisInit();
        }

        _settleEnd = true;
    }
    
    public void EndBattle()
    {
        _collection.Clear();
        _skillToUse.Clear();
        _skillToDeal.Clear();
        _battleStateMachine.SetState(_emptyState);
        Debug.Log("Battle End");
    }

    public EnemyCharacter RandomTarget()
    {
        var target = _collection.collection
            .OfType<EnemyCharacter>()
            .Where(enemy => enemy.info.stat.CurrentHp > 0)
            .OrderBy(enemy => enemy.info.stat.CurrentHp)
            .ToArray();

        return target.Length > 0 ? target[0] : null;
    }
}

