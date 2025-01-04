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
    public float GravitationalConstant = GameManager.Instance.physicsConstants.GravityStrength;

    // Constructor
    public GravityPlane(int height, int width, int res)
    {
        if (res <= 0)
        {
            throw new System.ArgumentException("Resolution must be greater than zero.");
        }

        // Calculate rows and columns based on resolution
        Rows = (int)(height * res);
        Cols = (int)(width * res);
        Resolution = res;
        Points = new Point[Rows,Cols];

        // Initialize the grid points
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                Points[i, j] = new Point(
                    (float) width * ((float) j / (float) Cols), 
                    (float) height * ((float) i / (float) Rows)
                );
            }
        }

        // Initialize wells
        Wells = new ();
    }

    // Method to place wells
    public void PlaceWells(List<Well> wells)
    {
        Wells.AddRange(wells);

        float scale;
        float distance;
        Vector2 direction;

        // recalculate forces
        foreach (var point in Points)
        {
            Vector2 total = new Vector2(0,0);
        
            foreach (var well in Wells)
            {
                // Calculate distance from point to well
                distance = Vector2.Distance(point.Position, well.Position);
                direction = (well.Position - point.Position).normalized;
                scale = GravitationalConstant * well.Mass / (distance*distance);
                total = total + scale * direction;
            }
            point.Force = total;
        }
    }

    // Example method to get the nearest point to an object
    public Vector2 GetGravityAtPos(float x, float y)
    {
        // Clamp the position to grid boundaries
        int row = Mathf.Clamp(Mathf.RoundToInt(y / Resolution), 0, Rows - 1);
        int col = Mathf.Clamp(Mathf.RoundToInt(x / Resolution), 0, Cols - 1);

        // Return the corresponding grid point
        return Points[row, col].Force;
    }

    public Vector2 GetGravityAtPos(Vector2 pos)
    {
        // Clamp the position to grid boundaries
        int row = Mathf.Clamp(Mathf.RoundToInt(pos.y * Resolution), 0, Rows - 1);
        int col = Mathf.Clamp(Mathf.RoundToInt(pos.x * Resolution), 0, Cols - 1);
        
        Point point = Points[row, col];
        Debug.LogWarning($"Position given: {pos}, Index Accessed: {row}, {col}");
        
        // Return the corresponding grid point
        return point.Force;
    }
}