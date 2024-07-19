using System;
using System.Linq;
using Character.Enemy;
using Event;
using Event.Events;
using UnityEngine;
using UnityEngine.UI;
using Utility.Interface;

namespace UI
{
    public class EnemyUIManager : MonoBehaviour
    {
        public GameObject enemyPrefab;

        private void OnEnable()
        {
            EventManager.Instance.SubscribeEvent<BattleStartEvent>(DrawEnemy);
        }

        private void DrawEnemy(IEventMessage message)
        {
            if (message is not BattleStartEvent msg) return;

            var collection = msg.collection;
            foreach (var character in collection.collection)
            {
                if (character is not EnemyCharacter enemyCharacter) continue;
                
                var enemy = Instantiate(enemyPrefab, transform);
                enemy.GetComponent<EnemyUI>().Load(enemyCharacter);
            }
        }
    }
}