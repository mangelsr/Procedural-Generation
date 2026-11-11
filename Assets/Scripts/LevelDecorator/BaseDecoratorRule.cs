using UnityEngine;

public abstract class BaseDecoratorRule : ScriptableObject
{
    [SerializeField, EnumFlags] RoomType roomTypes;
    public RoomType RoomTypes => roomTypes;

    internal abstract bool CanBeApplied(TileType[,] levelDecorated, Room room);

    internal abstract void Apply(TileType[,] levelDecorator, Room room, Transform parent);
}
