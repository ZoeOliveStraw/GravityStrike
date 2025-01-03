using UnityEngine;
using System.Collections.Generic;

public class Point
{
    public Vector2 Position;
    public Vector2 Force { get; set; }

    // Constructor to initialize the point with coordinates, magnitude, and force
    public Point(float x, float y)
    {
        Position = new Vector2(x,y);
        Force = new Vector2(0,0);
    }
}
