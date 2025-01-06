using System.Collections.Generic;
using UnityEngine;

public class StageFactory
{
    public int Level;
    public int Difficulty;

    public int Width;
    public int Height;
    public int Resolution;
    public Vector2 WellSizeRange;
    public Vector2Int WellCountRange;
    public Vector2 EnemyCountRange;

    public StageFactory(
        int width, 
        int height, 
        int resolution,
        Vector2Int well_count_range,
        Vector2 well_size_range,
        Vector2 enemy_count_range 
    )
    {
        Width = width;
        Height = height;
        Resolution = resolution;
        WellCountRange = well_count_range;
        WellSizeRange = well_size_range;
        EnemyCountRange = enemy_count_range;
    }
    
    public StageFactory(int resolution, int width, int height, SO_DifficultyProfile difficulty, int stage)
    {
        Vector2Int wellCount = new Vector2Int(difficulty.starCountRange.x + stage, difficulty.starCountRange.y + stage);
        Vector2Int enemyCount = new Vector2Int(difficulty.enemyCountRange.x + stage, difficulty.enemyCountRange.y + stage);
        
        Width = width;
        Height = height;
        Resolution = resolution;
        WellCountRange = wellCount;
        WellSizeRange = difficulty.starSizeRange;
        EnemyCountRange = enemyCount;
    }

    public StageInfo create() {

        GravityPlane plane = generateGravityPlane();
        List<Well> wells = generateWells();
        List<Enemy> enemies = generateEnemies(wells);
        plane.PlaceWells(wells);
        return new StageInfo(Width,Height,wells,enemies,plane);
    }

    public GravityPlane generateGravityPlane() {
        return new GravityPlane(Width, Height, Resolution); 
    }

    public List<Enemy> generateEnemies(List<Well> wells) {
        int enemy_count = Random.Range(Mathf.FloorToInt(EnemyCountRange.x), Mathf.CeilToInt(EnemyCountRange.y));
        List<Enemy> enemies = new List<Enemy>();

        Vector2 spawn_point = new Vector2(Width * 0.5f, Height * 0.5f);

        int maxAttempts = 20;

        for (int i = 0; i < enemy_count; i++)
        {
            int attemptCount = 0;
            bool isValidLocation = false;
            Enemy temp_enemy = null;

            while (!isValidLocation && attemptCount < maxAttempts)
            {
                attemptCount++;
                Vector2 enemy_location = new Vector2(Random.Range(0, Width), Random.Range(0, Height));
                temp_enemy = new Enemy((int) enemy_location.x, (int) enemy_location.y);

                if (!IsFarEnoughFromSpawn(temp_enemy, spawn_point) || !IsFarEnoughFromOtherWells(temp_enemy, wells, 0))
                {
                    continue;
                }

                isValidLocation = true;
            }

            if (isValidLocation && temp_enemy != null)
            {
                enemies.Add(temp_enemy);
            }
            else
            {
                i--;
                break;
            }
        }

        return enemies;
    }

    public List<Well> generateWells() 
    {
        int well_count = Random.Range(Mathf.FloorToInt(WellCountRange.x), Mathf.CeilToInt(WellCountRange.y));
        List<Well> wells = new List<Well>();

        Vector2 spawn_point = new Vector2(Width * 0.5f, Height * 0.5f);
        float minDistance = GameManager.Instance.difficultyProfile.minDistanceBetweenStarSurfaces;

        int maxAttempts = 20;

        for (int i = 0; i < well_count; i++)
        {
            int attemptCount = 0;
            bool isValidLocation = false;
            Well temp_well = null;

            while (!isValidLocation && attemptCount < maxAttempts)
            {
                attemptCount++;
                float well_size = Random.Range(Mathf.FloorToInt(WellSizeRange.x), Mathf.CeilToInt(WellSizeRange.y));
                Vector2 well_location = new Vector2(Random.Range(0, Width), Random.Range(0, Height));
                temp_well = new Well(well_location.x, well_location.y, well_size);

                if (!IsWithinBoundaries(temp_well) || !IsFarEnoughFromSpawn(temp_well, spawn_point) || !IsFarEnoughFromOtherWells(temp_well, wells, minDistance))
                {
                    continue;
                }

                isValidLocation = true;
            }

            if (isValidLocation && temp_well != null)
            {
                wells.Add(temp_well);
            }
            else
            {
                break;
            }
        }

        return wells;
    }

    private bool IsWithinBoundaries(Well well) {
        float radius = well.Diameter * 0.5f;

        return well.Position.x - radius >= 0 &&
               well.Position.x + radius <= Width &&
               well.Position.y - radius >= 0 &&
               well.Position.y + radius <= Height;
    }

    private bool IsFarEnoughFromSpawn(Well item, Vector2 spawnPoint)
    {
        float spawnDistance = Vector2.Distance(item.Position, spawnPoint) - (item.Diameter * 0.5f);
        return spawnDistance >= 5; 
    }

    private bool IsFarEnoughFromSpawn(Enemy item, Vector2 spawnPoint)
    {
        float spawnDistance = Vector2.Distance(item.Position, spawnPoint) - (item.Diameter * 0.5f);
        return spawnDistance >= 5;
    }

    private bool IsFarEnoughFromOtherWells(Well candidate, List<Well> existingWells, float minDistance)
    {
        foreach (var well in existingWells)
        {
            float distance = Vector2.Distance(candidate.Position, well.Position) - (candidate.Diameter * 0.5f + well.Diameter * 0.5f);
            if (distance < minDistance)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsFarEnoughFromOtherWells(Enemy candidate, List<Well> existingWells, float minDistance)
    {
        foreach (var well in existingWells)
        {
            float distance = Vector2.Distance(candidate.Position, well.Position) - (candidate.Diameter * 0.5f + well.Diameter * 0.5f);
            if (distance < minDistance)
            {
                return false;
            }
        }
        return true;
    }


    
}
