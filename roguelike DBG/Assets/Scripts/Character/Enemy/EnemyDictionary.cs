using System.Collections.Generic;
using UnityEngine;

namespace Character.Enemy
{
    [CreateAssetMenu(menuName = "EnemyDictionary")]
    public class EnemyDictionary : ScriptableObject
    {
        public List<EnemyCharacter> enemies;
    }
}