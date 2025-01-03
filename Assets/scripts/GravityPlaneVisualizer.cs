using UnityEngine;

public class GravityPlaneVisualizer : MonoBehaviour
{
    public GravityPlane gravityPlane;  // Reference to the GravityPlane
    public float pointWidth = 0.1f;    // Width of the rectangle
    public float pointHeight = 0.1f;   // Height of the rectangle
    public float sphereRadius = 0.3f;  // Radius of the sphere representing wells
    public Color gridColor = Color.green;  // Color for the grid points
    public Color wellColor = Color.red;   // Color for the wells

    public GameObject spherePrefab;      // Prefab for the sphere representing wells
    public GameObject cubePrefab;        // Prefab for the grid points (now a cube)

    void Start()
    {
        // Initialize the GravityPlane with height, width, and resolution
        gravityPlane = new GravityPlane(10, 10, 1); // A 10x10 grid with resolution of 1

        // Initialize wells (for testing purposes, you can adjust well positions and masses)
        gravityPlane.PlaceWells(new Well[] {
            new Well(2, 3, 10),
            new Well(6, 7, 20)
        });

        // Create points in the scene as cubes and wells as spheres
        for (int i = 0; i < gravityPlane.Rows; i++)
        {
            for (int j = 0; j < gravityPlane.Cols; j++)
            {
                Vector2 position = gravityPlane.Points[i, j].Position;
                CreatePoint(position,gravityPlane.Points[i, j]);  // Create cubes for points
            }
        }

        // Create spheres for wells
        foreach (var well in gravityPlane.Wells)
        {
            CreateWell(well.Position);  // Create spheres for wells
        }
    }

    void Update()
    {
        if (gravityPlane == null)
            return;

        // Loop through each point and draw it as a rectangle (4 lines)
        for (int i = 0; i < gravityPlane.Rows; i++)
        {
            for (int j = 0; j < gravityPlane.Cols; j++)
            {
                Vector2 position = gravityPlane.Points[i, j].Position;
                // Draw the 4 edges of the rectangle using Debug.DrawLine
                DrawRectangle(position, pointWidth, pointHeight);
            }
        }

        // Draw spheres for wells in Scene view
        foreach (var well in gravityPlane.Wells)
        {
            // You could visualize this as a circle or small sphere in the Scene view
            DrawSphere(well.Position, sphereRadius);
        }
    }

    // Method to draw a rectangle using Debug.DrawLine
    void DrawRectangle(Vector2 center, float width, float height)
    {
        // Calculate the 4 corners of the rectangle
        Vector3 topLeft = new Vector3(center.x - width / 2, 0, center.y + height / 2);
        Vector3 topRight = new Vector3(center.x + width / 2, 0, center.y + height / 2);
        Vector3 bottomLeft = new Vector3(center.x - width / 2, 0, center.y - height / 2);
        Vector3 bottomRight = new Vector3(center.x + width / 2, 0, center.y - height / 2);

        // Draw lines between the corners to form a rectangle
        Debug.DrawLine(topLeft, topRight, gridColor);
        Debug.DrawLine(topRight, bottomRight, gridColor);
        Debug.DrawLine(bottomRight, bottomLeft, gridColor);
        Debug.DrawLine(bottomLeft, topLeft, gridColor);
    }

    // Method to draw a sphere using Debug.DrawRay (visualizing as a point or circle)
    void DrawSphere(Vector2 position, float radius)
    {
        // Draw rays from the center in all directions to simulate a sphere
        Debug.DrawRay(new Vector3(position.x, 0, position.y), Vector3.up * radius, wellColor);
        Debug.DrawRay(new Vector3(position.x, 0, position.y), Vector3.right * radius, wellColor);
        Debug.DrawRay(new Vector3(position.x, 0, position.y), Vector3.forward * radius, wellColor);
    }

    // Method to instantiate a sphere for wells in the Game view
    void CreateWell(Vector2 position)
    {
        // Create a new sphere (well) at the specified position
        GameObject sphere = Instantiate(spherePrefab, new Vector3(position.x, 0, position.y), Quaternion.identity);
        
        // Optionally change the color of the sphere (well)
        sphere.GetComponent<Renderer>().material.color = wellColor;
        sphere.transform.localScale = new Vector3(sphereRadius, sphereRadius, sphereRadius);  // Set sphere size
    }
// Method to create a point as a 3D rectangle with dynamic color
void CreatePoint(Vector2 position, Point point)
{
    // Create a new cube at the specified position
    GameObject pointObject = Instantiate(cubePrefab, new Vector3(position.x, 0, position.y), Quaternion.identity);

    // Set the scale to form the rectangle (X and Z dimensions)
    pointObject.transform.localScale = new Vector3(pointWidth, 1, pointHeight);

    // Map MagnitudeMultiplier to a color
    Color pointColor = MapMagnitudeToColor(point.MagnitudeMultiplier);

    // Optionally change the color of the point
    pointObject.GetComponent<Renderer>().material.color = pointColor;
}

// Helper method to map MagnitudeMultiplier to a color
Color MapMagnitudeToColor(float magnitude)
{
    // You can use a color gradient or directly map the value to a color range
    // For example, use a simple gradient where low values are blue and high values are red

    // Normalize magnitude to a range from 0 to 1
    float normalizedMagnitude = Mathf.Clamp01(magnitude / 10f);  // Scale the magnitude for color range, adjust as needed

    // Return a color based on the magnitude
    return Color.Lerp(Color.blue, Color.red, normalizedMagnitude); // Transition from blue to red
}

}
