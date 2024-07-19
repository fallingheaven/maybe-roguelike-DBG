namespace Utility
{
    [System.Serializable]
    public enum EntityType
    {
        Player, Npc, NonCharacter, Enemy
    }
    
    [System.Serializable]
    public enum SkillType
    {
        Null, Active, Passive
    }
    
    [System.Serializable]
    public enum SkillEffect
    {
        ApplyEffect, CauseDamage, Heal
    }

    [System.Serializable]
    public enum SkillMode
    {
        Null, Near, Remote
    }

    [System.Serializable]
    public enum StatEnum
    {
        HP = 0, CHP, MP, CMP, ATK, MTK, DFE, MDF, AGI, HIT, DODGE, CRIT, DRR, DAMAGE
    }

    [System.Serializable]
    public enum CalEnum
    {
        Plus, Minus, Multiply, Divide
    }

    public enum ActionType
    {
        Attack, Defense, Empty
    }
}