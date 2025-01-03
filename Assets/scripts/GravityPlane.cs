using UnityEngine;
using System.Collections.Generic;

public class GravityPlane
{
    // 2D grid of points
    public Vector2[,] Points { get; private set; }

    // Wells (positions of interest in the plane)
    public List<Vector2> Wells { get; private set; }

    // Grid dimensions
    public int Rows { get; private set; }
    public int Cols { get; private set; }

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

        // Initialize the grid
        Points = new Vector2[Rows, Cols];

        // Initialize wells
        Wells = new List<Vector2>();
    }

    // Method to place wells
    public void PlaceWells(List<Vector2> wells)
    {
        if (wells == null)
        {
            throw new System.ArgumentNullException(nameof(wells));
        }

        Wells.AddRange(wells);
    }

    // Example method to get the nearest well to a point
    public Vector2 GetNearestWell(Vector2 point)
    {
        if (Wells.Count == 0)
        {
            throw new System.InvalidOperationException("No wells have been placed.");
        }

        Vector2 nearest = Wells[0];
        float minDistance = Vector2.Distance(point, nearest);

        foreach (var well in Wells)
        {
            float distance = Vector2.Distance(point, well);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = well;
            }
        }

        return nearest;
    }
}
