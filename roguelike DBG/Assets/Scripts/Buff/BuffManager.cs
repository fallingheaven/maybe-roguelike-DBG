using System.Collections.Generic;
using System.Linq;
using Character;

namespace Buff
{
    public class BuffManager
    {
        public CharacterBase carrier;
        private readonly HashSet<Buff> _buffs = new HashSet<Buff>();

        public void AddBuff(Buff newBuff)
        {
            var buff = _buffs.SingleOrDefault(b => b.buffName == newBuff.buffName);
            if (buff != null)
            {
                buff = newBuff;
                newBuff.OnEnterBuff(carrier);
            }
            else
            {
                _buffs.Add(newBuff);
                newBuff.OnEnterBuff(carrier);
            }
        }

        public void RemoveBuff(Buff removedBuff)
        {
            var foundBuff = _buffs.SingleOrDefault(b => b.buffName == removedBuff.buffName);
            if (foundBuff == null) return;
            
            _buffs.Remove(removedBuff);
            foundBuff.OnExitBuff(carrier);
        }

        public void OnNewTurn()
        {
            foreach (var buff in _buffs)
            {
                buff.OnTriggerBuff(carrier);
            }
        }
    }
}