using UnityEngine;

public class BattleInput : MonoBehaviour
{
    public void StartBattle()
    {
        BattleManager.Instance.EndPrepare();
    }
}