using UnityEngine;
using System.Collections.Generic;

public class Well
{
    public Vector2 Position;
    public float Mass { get; set; }
    public float Diameter { get; set; }

    public Well(float x, float y, float mass)
    {
        Position = new Vector2(x,y);
        Mass = mass;
        Diameter = GameManager.Instance.physicsConstants.StarSizePerMass * Mass;
    }
}
