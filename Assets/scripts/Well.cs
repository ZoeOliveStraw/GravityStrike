using UnityEngine;
using System.Collections.Generic;

public class Well
{
    public Vector2 Position;
    public float Mass { get; set; }

    public Well(int x, int y, float mass)
    {
        Position = new Vector2(x,y);
        Mass = mass;
    }

    public void updateForce(float magnitude_multiplier, Vector2 direction)
    {
        MagnitudeMultiplier = magnitude_multiplier;
        Direction = direction.normalized;
    }

    public Vector2 getForce(float mass)
    {
        return new Vector2(mass * MagnitudeMultiplier, direction);
    }
}
