using UnityEngine;

public class GravityPlaneVisualizer : MonoBehaviour
{
    public GravityPlane gravityPlane;  // Reference to the GravityPlane
    public float pointWidth = 0.5f;    // Width of the rectangle
    public float pointHeight = 0.5f;   // Height of the rectangle
    public float sphereRadius = 0.3f;  // Radius of the sphere representing wells
    public Color gridColor = Color.green;  // Color for the grid points
    public Color wellColor = Color.red;   // Color for the wells

    public GameObject spherePrefab;      // Prefab for the sphere representing wells
    public GameObject cubePrefab;
    
    public int xSize;
    public int ySize;
    public int resolution;        // Prefab for the grid points (now a cube)

    public Vector2 well_count_range;
    public Vector2 well_size_range;
    public Vector2 enemy_count_range;

    void Start()
    {

        StageInfo stage = (new StageFactory(
            xSize,
            ySize,
            resolution,
            well_count_range,
            well_size_range,
            enemy_count_range
        )).create();

        gravityPlane = stage.GravityPlane;

        // Create points in the scene as cubes and wells as spheres
        for (int i = 0; i < gravityPlane.Rows; i++)
        {
            for (int j = 0; j < gravityPlane.Cols; j++)
            {
                // Vector2 position = gravityPlane.Points[i, j].Position;
                // CreatePoint(position,gravityPlane.Points[i, j]);  // Create cubes for points
            }
        }

        // Create spheres for wells
        foreach (var well in stage.Wells)
        {
            CreateWell(well.Position,well.Diameter*0.5f);  // Create spheres for wells
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
                // Draw the 4 edges of the rectangle using Debug.DrawLine
                DrawRectangle(gravityPlane.Points[i, j], pointWidth, pointHeight);
            }
        }

        // Draw spheres for wells in Scene view
        foreach (var well in gravityPlane.Wells)
        {
            // You could visualize this as a circle or small sphere in the Scene view
            DrawSphere(well.Position, well.Diameter * 0.5f);
        }
    }

    // Method to draw a rectangle using Debug.DrawLine
    void DrawRectangle(Point point, float width, float height)
    {
        // Calculate the 4 corners of the rectangle
        Vector3 topLeft = new Vector3(point.Position.x - width / 2, 0, point.Position.y + height / 2);
        Vector3 topRight = new Vector3(point.Position.x + width / 2, 0, point.Position.y + height / 2);
        Vector3 bottomLeft = new Vector3(point.Position.x - width / 2, 0, point.Position.y - height / 2);
        Vector3 bottomRight = new Vector3(point.Position.x + width / 2, 0, point.Position.y - height / 2);

        Color pointColor = MapMagnitudeToColor(point.Force.magnitude);

        // Draw lines between the corners to form a rectangle
        Debug.DrawLine(topLeft, topRight, pointColor);
        Debug.DrawLine(topRight, bottomRight, pointColor);
        Debug.DrawLine(bottomRight, bottomLeft, pointColor);
        Debug.DrawLine(bottomLeft, topLeft, pointColor);
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
    void CreateWell(Vector2 position,float radius)
    {
        // Create a new sphere (well) at the specified position
        GameObject sphere = Instantiate(spherePrefab, new Vector3(position.x, 0, position.y), Quaternion.identity);
        
        // Optionally change the color of the sphere (well)
        sphere.GetComponent<Renderer>().material.color = wellColor;
        sphere.transform.localScale = new Vector3(radius, radius, radius);  // Set sphere size
    }
    // Method to Create a point as a 3D rectangle with dynamic color
    void CreatePoint(Vector2 position, Point point)
    {
        GameObject pointObject = Instantiate(cubePrefab, new Vector3(position.x, 0, position.y), Quaternion.identity);
        pointObject.transform.localScale = new Vector3(pointWidth, 1, pointHeight);
        Color pointColor = MapMagnitudeToColor(point.Force.magnitude);
        pointObject.GetComponent<Renderer>().material.color = pointColor;
    }

    // Helper method to map MagnitudeMultiplier to a color
    Color MapMagnitudeToColor(float magnitude)
    {
        float normalizedMagnitude = Mathf.Clamp01(magnitude / 10f);  // Scale the magnitude for color range, adjust as needed
        return Color.Lerp(Color.blue, Color.red, normalizedMagnitude); // Transition from blue to red
    }

}
