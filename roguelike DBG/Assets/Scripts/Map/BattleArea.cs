using System;

namespace Map
{
    public class BattleArea : AreaBase
    {
        private void OnEnable()
        {
            StepIn();
        }

        public void StepIn()
        {
            BattleManager.Instance.OnNewBattle();
        }
    }
}