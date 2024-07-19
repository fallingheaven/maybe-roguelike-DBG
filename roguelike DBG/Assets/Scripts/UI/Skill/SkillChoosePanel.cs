using System;
using System.Collections.Generic;
using Character;
using Character.Player;
using Event;
using Event.Events;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Utility.Interface;

namespace UI.Skill
{
    public class SkillChoosePanel : MonoBehaviour
    {
        public GameObject viewport;
        public GameObject skillBlock;
        private CharacterBase _player;
        private ChosenSkillBlock _currentBlock;

        private bool _passiveSkillBlock;
        private List<GameObject> _blocks = new List<GameObject>();

        private void OnDisable()
        {
            foreach(var block in _blocks)
            {
                Destroy(block);
            }
            _blocks.Clear();
        }

        public void Init(ChosenSkillBlock block)
        {
            // Debug.Log(block.index);

            if (isActiveAndEnabled)
            {
                if (_currentBlock != null && _currentBlock.index == block.index)
                {
                    gameObject.SetActive(false);
                    _currentBlock = null;
                    return;
                }
                else
                {
                    _currentBlock = block;
                    return;
                }
            }
            else
            {
                _currentBlock = block;
            }
            
            switch (block.type)
            {
                case SkillType.Active:
                    _passiveSkillBlock = false;
                    break;
                case SkillType.Passive:
                    _passiveSkillBlock = true;
                    break;
                case SkillType.Null:
                    break;
                default:
                    Debug.LogWarning("出错");
                    break;
            }
            
            _player = PlayerManager.Instance.CurrentCharacter;
            _player.skills.Sort(((skill1, skill2) => skill1.type - skill2.type));
            
            foreach (var skill in _player.skills)
            {
                // Debug.Log(skill.skillName);
                if (skill.type == SkillType.Active && _passiveSkillBlock) continue;
                
                var button = Instantiate(skillBlock, viewport.transform);
                _blocks.Add(button);
                button.GetComponent<ChooseSkillBlock>().SetName(skill.name);
                
                var chooseSkillEvent = new ChooseSkillEvent
                {
                    index = _currentBlock.index, skill = skill, target = BattleManager.Instance.selectedCharacter
                };
                
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    // Debug.Log("click");
                    EventManager.Instance.Invoke(chooseSkillEvent);
                    _currentBlock.SetName(skill.skillName);
                    gameObject.SetActive(false);
                });
            }

            gameObject.SetActive(true);
        }
    }
}