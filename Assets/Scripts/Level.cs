using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    int width, length;
    List<Room> rooms;
    List<Hallway> hallways;

    public int Width { get { return width; } }
    public int Length { get { return length; } }


    public Level(int width, int length)
    {
        this.width = width;
        this.length = length;

        rooms = new List<Room>();
        hallways = new List<Hallway>();
    }
}
