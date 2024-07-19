using System;
using System.Linq;
using Character.Enemy;
using Event;
using Event.Events;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Utility.Interface;

namespace UI
{
    public class EnemyUI : MonoBehaviour
    {
        private EnemyCharacter _enemy;
        public GameObject actionIcon;
        public GameObject actionIconPrefab;
        public Slider hpSlider;

        private void OnEnable()
        {
            EventManager.Instance.SubscribeEvent<EnemySkillPreparedEvent>(ShowActionIcon);
        }

        private void OnDisable()
        {
            EventManager.Instance.UnsubscribeEvent<EnemySkillPreparedEvent>(ShowActionIcon);
            
            if (_enemy == null) return;
            
            _enemy.onHurtEvent -= OnHpChanged;
            _enemy.onHealEvent -= OnHpChanged;
            _enemy.onDieEvent -= OnDie;
        }

        public void Selected()
        {
            BattleManager.Instance.selectedCharacter = _enemy;
        }

        public void Load(EnemyCharacter enemyCharacter)
        {
            _enemy = enemyCharacter;
            {
                _enemy.onHurtEvent += OnHpChanged;
                _enemy.onHealEvent += OnHpChanged;
                _enemy.onDieEvent += OnDie;
            }
            var sprite = _enemy.info.battleSprite;

            GetComponent<Image>().sprite = sprite;

            var spriteHeight = sprite.rect.height;
            var spriteWidth = sprite.rect.width;
            var spriteAspectRatio = spriteHeight / spriteWidth;
            
            var rectTransform = GetComponent<RectTransform>();
            var sizeDelta = rectTransform.sizeDelta;
            sizeDelta = new Vector2(sizeDelta.y, sizeDelta.y * spriteAspectRatio);
            rectTransform.sizeDelta = sizeDelta;
        }

        private void ShowActionIcon(IEventMessage message)
        {
            if (message is not EnemySkillPreparedEvent msg) return;

            var enemy = msg.enemy;
            if (enemy != _enemy) return;
            
            for (var i = 0; i < actionIcon.transform.childCount; i++)
            {
                Destroy(actionIcon.transform.GetChild(i).gameObject);
            }
            
            var skills = enemy.PreActiveSkills
                .Concat(enemy.PrePassiveSkills)
                .Where(skill => skill != null)
                .ToArray();
            
            // Debug.Log(skills.Count());
            
            foreach (var skill in skills)
            {
                switch (skill.actionType)
                {
                    case ActionType.Attack:
                        Instantiate(actionIconPrefab, actionIcon.transform).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Attack");
                        break;
                    case ActionType.Defense:
                        Instantiate(actionIconPrefab, actionIcon.transform).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Defense");
                        break;
                    case ActionType.Empty:
                        Instantiate(actionIconPrefab, actionIcon.transform).GetComponent<Image>().sprite = null;
                        break;
                }
            }
        }

        private void OnHpChanged()
        {
            var info = _enemy.info.stat;
            hpSlider.value = info.CurrentHp / info.MaxHp;
        }

        private void OnDie()
        {
            _enemy.Deinit();
            Destroy(gameObject);
        }
    }
}