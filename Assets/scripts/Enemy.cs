using UnityEngine;
using System.Collections.Generic;

public class Enemy
{
    public Vector2 Position;
    public int AttackMultiplier;

    public Enemy(int x, int y, int attack_multiplier)
    {
        Position = new Vector2(x,y);
        AttackMultiplier = attack_multiplier;
    }
}
