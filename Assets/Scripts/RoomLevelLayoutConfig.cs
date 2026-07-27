using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

[CreateAssetMenu(fileName = "Room Level Layou", menuName = "Custom/Procedural Generation/Room Level Layout Config")]
public class RoomLevelLayoutConfig : ScriptableObject
{
    [SerializeField] int width = 64;
    [SerializeField] int length = 64;

    [SerializeField] RoomTemplate[] roomTemplates;

    [SerializeField] int doorDistanceFromEdge = 1;
    [SerializeField] int hallwayLengthMin = 2;
    [SerializeField] int hallwayLengthMax = 6;
    [SerializeField] int minRoomDistance = 1;

    public int Width => width;
    public int Length => length;

    public RoomTemplate[] RoomTemplates => roomTemplates;

    public int DoorDistanceFromEdge => doorDistanceFromEdge;
    public int HallwayLengthMin => hallwayLengthMin;
    public int HallwayLengthMax => hallwayLengthMax;
    public int MinRoomDistance => minRoomDistance;

    public Dictionary<RoomTemplate, int> GetAvaialbleRooms()
    {
        Dictionary<RoomTemplate, int> availableRooms = new();

        for (int i = 0; i < roomTemplates.Length; i++)
        {
            availableRooms.Add(
                roomTemplates[i], 
                roomTemplates[i].NumberOfRooms
            );
        }

        availableRooms = availableRooms
            .Where(kvp => kvp.Value > 0)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        return availableRooms;
    }
}
