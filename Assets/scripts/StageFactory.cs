using System.Collections.Generic;
using UnityEngine;

public class StageFactory
{
    public int Width;
    public int Height;
    public int Resolution;

    public StageFactory(int width, int height, int resolution)
    {
        Width = width;
        Height = height;
        Resolution = resolution;
    }

    public StageInfo Create() {
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
        // determine number of wells and strength
        return new List<Well> {
            new Well(2, 3, 10),
            new Well(6, 7, 20)
        };
    }


}
