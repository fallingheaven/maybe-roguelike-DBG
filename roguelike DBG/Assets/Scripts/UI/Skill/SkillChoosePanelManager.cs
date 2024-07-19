using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Character.Player;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Skill
{
    public class SkillChoosePanelManager : MonoBehaviour
    {
        public GameObject skillChoosePanel;
        public GameObject viewport;
        public GameObject chosenSkillBlock;
        private CharacterBase _player;

        private List<GameObject> _blocks = new List<GameObject>();

        private void OnEnable()
        {
            _player = PlayerManager.Instance.CurrentCharacter;
            var activeSkillCount = _player.activeSkillCount;
            var passiveSkillCount = _player.passiveSkillCount;
            
            var i = 0;
            for (; i < activeSkillCount; i++)
            {
                var block = Instantiate(chosenSkillBlock, viewport.transform);
                _blocks.Add(block);
                var chosenBlock = block.GetComponent<ChosenSkillBlock>();
                chosenBlock.index = i;
                chosenBlock.type = SkillType.Active;
                chosenBlock.SetName("Empty");
                
                block.GetComponent<Button>().onClick.AddListener(() => OpenSkillChoosePanel(chosenBlock));
            }

            for (; i - activeSkillCount < passiveSkillCount; i++)
            {
                var block = Instantiate(chosenSkillBlock, viewport.transform);
                _blocks.Add(block);
                var chosenBlock = block.GetComponent<ChosenSkillBlock>();
                chosenBlock.index = i;
                chosenBlock.type = SkillType.Passive;
                chosenBlock.SetName("Empty");
                
                block.GetComponent<Button>().onClick.AddListener(() => OpenSkillChoosePanel(chosenBlock));
            }
        }

        private void OnDisable()
        {
            foreach(var block in _blocks)
            {
                Destroy(block);
            }
            _blocks.Clear();
        }

        private void OpenSkillChoosePanel(ChosenSkillBlock block)
        {
            skillChoosePanel.GetComponent<SkillChoosePanel>().Init(block);
            // TODO: 打开动画
        }
    }
}
