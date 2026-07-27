using System;
using UnityEngine;

[Serializable]
public class RoomTemplate
{
    [SerializeField] string name;
    [SerializeField] int numberOfRooms;

    [SerializeField] int roomWidthMin = 3;
    [SerializeField] int roomWidthMax = 5;
    [SerializeField] int roomLengthMin = 3;
    [SerializeField] int roomLengthMax = 5;

    public int NumberOfRooms => numberOfRooms;
    public int RoomWidthMin => roomWidthMin;
    public int RoomWidthMax => roomWidthMax;
    public int RoomLengthMin => roomLengthMin;
    public int RoomLengthMax => roomLengthMax;
}
