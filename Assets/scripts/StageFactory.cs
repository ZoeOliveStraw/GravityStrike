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
    
    public StageFactory(int resolution, int width, int height, SO_DifficultyProfile difficulty)
    {
        Width = width;
        Height = height;
        Resolution = resolution;
        WellCountRange = difficulty.starCountRange;
        WellSizeRange = difficulty.starSizeRange;
        EnemyCountRange = difficulty.enemyCountRange;
    }

    public StageInfo create() {

        GravityPlane plane = generateGravityPlane();
        List<Well> wells = generateWells();
        List<Enemy> enemies = generateEnemies();
        plane.PlaceWells(wells);
        return new StageInfo(Width,Height,wells,enemies,plane);
    }

    public GravityPlane generateGravityPlane() {
        return new GravityPlane(Width, Height, Resolution); 
    }

    public List<Enemy> generateEnemies() {
        return new List<Enemy> {};
    }

    public List<Well> generateWells() {
        int well_count = Random.Range(WellCountRange.x, WellCountRange.y);
        
        List<Well> wells = new List<Well>();

        bool has_appropriate_location;
        Vector2 well_location;
        Vector2 spawn_point = new Vector2(Width*0.5f,Height*0.5f);
        float distance;
        float well_size;
        float space_between_wells;
        float space_from_spawn;
        float space_from_top;
        float space_from_bottom;
        float space_from_left;
        float space_from_right;
        

        for (int i = 0; i < well_count; i++) {

            has_appropriate_location = true;

            well_size = Random.Range(Mathf.FloorToInt(WellSizeRange.x), Mathf.CeilToInt(WellSizeRange.y));
            well_location = new Vector2(Random.Range(0,Width),Random.Range(0,Height));
            Well temp_well = new Well(well_location.x,well_location.y,well_size);

            // calculate distance from each other well to determine if its too close
            int count = 0;
            foreach (var well in wells) {
                distance = Vector2.Distance(well_location, well.Position);
                space_between_wells = distance - well.Diameter * 0.5f - temp_well.Diameter * 0.5f;
                if (space_between_wells < GameManager.Instance.difficultyProfile.minDistanceBetweenStarSurfaces) {
                    has_appropriate_location = false;
                    break;
                }
                space_from_spawn = Vector2.Distance(well_location, spawn_point)-well.Diameter*0.5f;
                space_from_top = Height - (well_location.y + well.Diameter*0.5f);
                space_from_bottom = well_location.y - well.Diameter*0.5f;
                space_from_left = well_location.x - well.Diameter*0.5f;
                space_from_right = Width - (well_location.x + well.Diameter*0.5f);

                if (
                    space_from_spawn < 5
                    || space_from_bottom < 5
                    || space_from_top < 5
                    || space_from_left < 5
                    || space_from_right < 5
                ) {
                    has_appropriate_location = false;
                    break;
                }
            }

            if (has_appropriate_location) {
               wells.Add(temp_well);
            } else {
                i--; // try again
            }
            count++;
            if (count > 20) {
                Debug.LogWarning($"could not do wells");
                break;
            }
        }

        return wells;
    }

    
}
