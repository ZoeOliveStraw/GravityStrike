using UnityEngine;
using System.Collections.Generic;

public class Enemy
{
    public Vector2 Position;
    public int Diameter = 10;

    public Enemy(int x, int y)
    {
        Position = new Vector2(x,y);
    }
}
