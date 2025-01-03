using UnityEngine;

public class Point 
{
    public int x
    public int y
    public Vector2 force {get, set}

    // constructor
    public Point(int x, int y) 
    {
        UpdateForce(0,0);
    }

    public void UpdateForce(float magnitude, float direction)
    {
        force = new Vector2(magnitude, direction);
    }
}
