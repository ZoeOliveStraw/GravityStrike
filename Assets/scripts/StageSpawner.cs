using System.Collections.Generic;
using UnityEngine;

public class StageSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private GameObject enemy;

    [SerializeField] private Transform starParent;
    [SerializeField] private Transform enemyParent;

    public void SpawnStage(StageInfo stageInfo)
    {
        SpawnStars(stageInfo.Wells);
        SpawnEnemies(stageInfo.EnemyLocations);
        SpawnPlayer(stageInfo.LevelWidth, stageInfo.LevelHeight);
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

    private void SpawnEnemies(List<Vector2> enemies)
    {
        
    }

    private void SpawnPlayer(int x, int y)
    {
        Vector3 pos = new Vector3((float) x / 2, (float) y / 2, 0);
        Instantiate(playerPrefab, pos, Quaternion.identity);
    }
}
