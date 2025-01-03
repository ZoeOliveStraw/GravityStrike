using UnityEngine;
using System.Collections.Generic;

public class Point
{
    public Vector2 Position;
    public float MagnitudeMultiplier { get; set; }
    public Vector2  Direction { get; set; }

    // Constructor to initialize the point with coordinates, magnitude, and direction
    public Point(int x, int y)
    {
        Position = new Vector2(x,y);
        MagnitudeMultiplier = 0;
        Direction = new Vector2(0,0);
    }

    public void updateForce(float magnitude_multiplier, Vector2 direction)
    {
        MagnitudeMultiplier = magnitude_multiplier;
        Direction = direction.normalized;
    }

    public Vector2 getForce(float mass)
    {
        return  Direction * mass * MagnitudeMultiplier;
    }
}
