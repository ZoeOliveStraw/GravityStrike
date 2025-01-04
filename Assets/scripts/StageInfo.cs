using System.Collections.Generic;
using UnityEngine;

public class StageInfo
{
    public int LevelWidth;
    public int LevelHeight;
    public List<Well> Wells;
    public List<Enemy> Enemies;
    public GravityPlane GravityPlane;

    public int resolution;
    
    public StageInfo(int width, int height, List<Well> wells, List<Enemy> enemies, GravityPlane gravityPlane)
    {
        LevelWidth = width;
        LevelHeight = height;
        Wells = wells;
        Enemies = enemies;
        GravityPlane = gravityPlane;
    }
    
}
