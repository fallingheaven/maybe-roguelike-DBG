using Skill;
using TMPro;
using UnityEngine;

namespace UI.Skill
{
    public class ChooseSkillBlock : MonoBehaviour
    {
        public TextMeshProUGUI text;
        
        public void SetName(string s)
        {
            text.text = s;
        }
    }
}