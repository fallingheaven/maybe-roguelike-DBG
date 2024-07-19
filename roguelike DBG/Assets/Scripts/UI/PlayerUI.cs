using System;
using Character.Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Skill
{
    public class PlayerUI : MonoBehaviour
    {
        public Image portrait;
        public Slider hp;
        public Slider mp;

        private void OnEnable()
        {
            portrait.sprite = null;
        }

        private void Update()
        {
            var player = PlayerManager.Instance.CurrentCharacter;
            hp.value = player.info.stat.CurrentHp / player.info.stat.MaxHp;
            mp.value = player.info.stat.CurrentMp / player.info.stat.MaxMp;
        }
    }
}