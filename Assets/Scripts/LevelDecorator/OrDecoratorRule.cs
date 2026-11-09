using System;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;


[Serializable]
[CreateAssetMenu(fileName = "OrDecoratorRule", menuName = "Custom/Procedural Generation/Or Decorator Rule")]
public class OrDecoratorRule : BaseDecoratorRule
{
    [SerializeField] BaseDecoratorRule[] childRules;

    internal override bool CanBeApplied(TileType[,] levelDecorated, Room room)
    {
        foreach (BaseDecoratorRule rule in childRules)
        {
            if (rule.CanBeApplied(levelDecorated, room))
                return true;
        }
        return false;
    }

    internal override void Apply(TileType[,] levelDecorator, Room room, Transform parent)
    {
        List<BaseDecoratorRule> applicableChildRules = new();
        foreach (BaseDecoratorRule rule in childRules)
        {
            if (rule.CanBeApplied(levelDecorator, room))
                applicableChildRules.Add(rule);
        }
        Random random = SharedLevelData.Instance.Rand;
        int randomIndex = random.Next(0, applicableChildRules.Count);
        BaseDecoratorRule selectedRule = applicableChildRules[randomIndex];
        selectedRule.Apply(levelDecorator, room, parent);
    }
}
