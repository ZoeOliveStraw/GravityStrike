using System.Collections.Generic;
using UnityEngine;

public class StageInfo
{
    public int LevelWidth;
    public int LevelHeight;
    public List<Well> Wells;
    public List<Vector2> EnemyLocations;

    public StageInfo(int width, int height)
    {
        LevelWidth = width;
        LevelHeight = height;
    }
    
    public StageInfo(int width, int height, List<Well> wells)
    {
        LevelWidth = width;
        LevelHeight = height;
        Wells = wells;
    }
    
    public StageInfo(int width, int height, List<Well> wells, List<Vector2> enemies)
    {
        LevelWidth = width;
        LevelHeight = height;
        Wells = wells;
        EnemyLocations = enemies;
    }
}
