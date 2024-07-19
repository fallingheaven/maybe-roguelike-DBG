using UnityEngine;
using Utility;
using Random = System.Random;

namespace Character.Enemy
{
    public class EnemySpawner
    {
        private readonly EnemyDictionary EnemyDictionary = 
            Resources.Load<EnemyDictionary>("Data/Character/Enemy/Dictionary/Enemy Dictionary");
        
        private readonly EnemyDictionary BossDictionary = 
            Resources.Load<EnemyDictionary>("Data/Character/Enemy/Dictionary/Boss Dictionary");

        public CharacterBase SpawnEnemy()
        {
            var random = new Random();
            // return ScriptableObjectUtility.Clone(EnemyDictionary.enemies[random.Next(EnemyDictionary.enemies.Count)]);
            return EnemyDictionary.enemies[random.Next(EnemyDictionary.enemies.Count)].DeepCopy().Init();
        }
        
        public CharacterBase SpawnBoss()
        {
            var random = new Random();
            // return ScriptableObjectUtility.Clone(BossDictionary.enemies[random.Next(EnemyDictionary.enemies.Count)]);
            return BossDictionary.enemies[random.Next(EnemyDictionary.enemies.Count)].DeepCopy().Init();
        }
    }
}