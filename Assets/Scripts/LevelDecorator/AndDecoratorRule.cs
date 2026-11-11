using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "AndDecoratorRule", menuName = "Custom/Procedural Generation/And Decorator Rule")]
public class AndDecoratorRule : BaseDecoratorRule
{
    [SerializeField] BaseDecoratorRule[] childRules;

    internal override bool CanBeApplied(TileType[,] levelDecorated, Room room)
    {
        if (childRules == null || childRules.Length == 0) return false;
        foreach (BaseDecoratorRule rule in childRules)
        {
            if (rule == null || !rule.CanBeApplied(levelDecorated, room))
                return false;
        }
        return true;
    }


    internal override void Apply(TileType[,] levelDecorator, Room room, Transform parent)
    {
        if (childRules == null) return;
        foreach (BaseDecoratorRule rule in childRules)
        {
            if (rule != null && rule.CanBeApplied(levelDecorator, room))
            {
                rule.Apply(levelDecorator, room, parent);
            }
        }
    }
}
