using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Buff
{
    public class Buff : ScriptableObject
    {
        public string buffName;
        public int durationTurn;
        public int triggerFrequency;
        public List<IBuffEffect> effects = new List<IBuffEffect>();
        private int _restTurn;

        public Buff CloneInstantiate()
        {
            return Instantiate(this);
        }
        
        public void OnEnterBuff(CharacterBase carrier)
        {
            _restTurn = durationTurn;
        }

        public void OnTriggerBuff(CharacterBase carrier)
        {
            _restTurn--;
            if ((durationTurn - _restTurn) % triggerFrequency == 0)
            {
                foreach (var effect in effects)
                {
                    effect.Trigger(carrier);
                }
                
                if (_restTurn < 0)
                    OnExitBuff(carrier);
            }
        }

        public void OnExitBuff(CharacterBase carrier)
        {
            foreach (var effect in effects)
            {
                effect.Trigger(carrier);
            }
        }
    }
}