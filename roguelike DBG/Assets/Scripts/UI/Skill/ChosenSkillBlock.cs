using Skill;
using TMPro;
using UnityEngine;
using Utility;

namespace UI.Skill
{
    public class ChosenSkillBlock : MonoBehaviour
    {  
        public SkillType type;
        public int index;
        public SkillBase chosenSkill;
        public TextMeshProUGUI text;
        
        public void SetName(string s)
        {
            // Debug.Log(s);
            text.text = s;
        }
    }
}