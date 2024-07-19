using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Character;
using Utility.CustomClass;

namespace Character.Player
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        public List<PlayerCharacter> characters = new List<PlayerCharacter>();
        private CharacterBase _currentCharacter;

        public CharacterBase CurrentCharacter
        {
            get
            {
                if (_currentCharacter == null)
                {
                    _currentCharacter = Resources.Load<PlayerCharacter>("Data/Character/Player/Player1").DeepCopy().Init();
                }
                
                return _currentCharacter;
            }
        }

        private void Start()
        {
            Debug.Log(CurrentCharacter);
            // _currentCharacter = Resources.Load<PlayerCharacter>("Data/Character/Player/Player1").DeepCopy().Init();
            characters = Resources.LoadAll<PlayerCharacter>("Data/Character/Player").ToList();
        }

        // 设置当前角色
        public void SetCurrentCharacter(PlayerCharacter character)
        {
            if (characters.Contains(character))
            {
                _currentCharacter = character;
            }
            else
            {
                Debug.LogError("没这个角色");
            }
        }
    }

}