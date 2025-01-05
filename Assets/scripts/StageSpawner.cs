using System.Collections.Generic;
using UnityEngine;

public class StageSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject gridIntersectionPrefab;
    [SerializeField] private GameObject gridPointPrefab;

    [SerializeField] private Color GravityColor0;
    [SerializeField] private Color GravityColor1;

    [SerializeField] private Transform starParent;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private Transform gridPointParent;
    public GameObject SpawnStage(StageInfo stageInfo)
    {
        SpawnGrid(stageInfo.GravityPlane);
        SpawnStars(stageInfo.Wells);
        SpawnEnemies(stageInfo.Enemies);
        GameManager.Instance.HUD.txtEnemyCount.text = $"Enemies: {stageInfo.Enemies.Count}";
        return SpawnPlayer(stageInfo.LevelWidth, stageInfo.LevelHeight);
    }
    
    private void SpawnGrid(GravityPlane plane)
    {
        for(int i = 0; i < plane.Points.GetLength(0); i++)
        {
            for (int j = 0; j < plane.Points.GetLength(1); j++)
            {
                Point p = plane.Points[i, j];
                if (p.Position.x % 1 == 0 && p.Position.y % 1 == 0)
                {
                    GameObject go = Instantiate(gridIntersectionPrefab, p.Position, Quaternion.identity, gridPointParent);
                    go.GetComponent<SpriteRenderer>().color = MapMagnitudeToColor(p.Force.magnitude);
                }
                else
                {
                    GameObject go = Instantiate(gridPointPrefab, p.Position, Quaternion.identity, gridPointParent);
                    go.GetComponent<SpriteRenderer>().color = MapMagnitudeToColor(p.Force.magnitude);
                }
                
            }
        }
    }

    private void SpawnStars(List<Well> wells)
    {
        foreach (var star in wells)
        {
            float starSize = GameManager.Instance.physicsConstants.StarSizePerMass * star.Mass;
            var go = Instantiate(starPrefab, star.Position, Quaternion.identity, starParent);
            go.transform.localScale = new Vector3(starSize, starSize, 1);
            
        }
    }
    
    private Color MapMagnitudeToColor(float magnitude)
    {
        float normalizedMagnitude = Mathf.Clamp01(magnitude / 20f);
        return Color.Lerp(GravityColor0, GravityColor1, normalizedMagnitude);
    }

    private void SpawnEnemies(List<Enemy> enemies)
    {
        foreach(Enemy enemy in enemies) {
            Instantiate(enemyPrefab, enemy.Position, Quaternion.identity, enemyParent);
        }
    }

    private GameObject SpawnPlayer(int x, int y)
    {
        Vector3 pos = new Vector3((float) x / 2, (float) y / 2, 0);
        return Instantiate(playerPrefab, pos, Quaternion.identity);
    }
}
