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
}
