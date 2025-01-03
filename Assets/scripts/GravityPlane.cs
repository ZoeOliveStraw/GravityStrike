using UnityEngine;
using System.Collections.Generic;

public class GravityPlane
{
    // 2D grid of points
    public Point[,] Points { get; private set; }

    // Wells (positions of interest in the plane)
    public List<Well> Wells { get; private set; }

    // Grid dimensions
    public int Rows { get; private set; }
    public int Cols { get; private set; }
    public float Resolution { get; private set; }
    public float GravitationalConstant = 1;

    // Constructor
    public GravityPlane(int height, int width, float res)
    {
        if (res <= 0)
        {
            throw new System.ArgumentException("Resolution must be greater than zero.");
        }

        // Calculate rows and columns based on resolution
        Rows = (int)(height / res);
        Cols = (int)(width / res);
        Resolution = res;

        // Initialize the grid points
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                Points[i, j] = new Point(j, i);
            }
        }

        // Initialize wells
        Wells = new ();
    }

    // Method to place wells
    public void PlaceWells(Well[] wells)
    {
        Wells.AddRange(wells);

        // recalculate forces
        foreach (var point in Points)
        {
            foreach (var well in Wells)
            {
                // Calculate distance from point to well
                float distance = Vector2.Distance(point.Position, well.Position);
                Vector2 direction = (well.Position - point.Position).normalized;
                float magnitude_multiplier = GravitationalConstant * well.Mass / (distance*distance);
                point.updateForce(magnitude_multiplier,direction);
            }
        }
    }

    // Example method to get the nearest point to an object
    public Point GetNearestPoint(float x, float y)
    {
        // Clamp the position to grid boundaries
        int row = Mathf.Clamp(Mathf.RoundToInt(y / Resolution), 0, Rows - 1);
        int col = Mathf.Clamp(Mathf.RoundToInt(x / Resolution), 0, Cols - 1);

        // Return the corresponding grid point
        return Points[row, col];
    }
}