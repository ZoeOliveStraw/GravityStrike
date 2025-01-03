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
    
    public StageInfo()
    {
        LevelWidth = 30;
        LevelHeight = 30;
        Wells = new List<Well>();
        Well well1 = new Well(5,10,1);
        Wells.Add(well1);
        Well well2 = new Well(20,10,2);
        Wells.Add(well2);
        Well well3 = new Well(25,5,3);
        Wells.Add(well3);
        Well well4 = new Well(5,25,4);
        Wells.Add(well4);
        Well well5 = new Well(25,25,5);
        Wells.Add(well5);
        EnemyLocations = new List<Vector2>();
    }
}
