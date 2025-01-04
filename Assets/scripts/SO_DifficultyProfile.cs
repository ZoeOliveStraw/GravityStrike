using UnityEngine;

[CreateAssetMenu(fileName = "SO_DifficultyProfile", menuName = "Difficulty Profile")]
public class SO_DifficultyProfile : ScriptableObject
{
    [SerializeField] private Vector2Int starCountRange;
    [SerializeField] private Vector2 starSizeRange;
    [SerializeField] private Vector2Int enemyCountRange;
    [SerializeField] private float minDistanceBetweenStarSurfaces;

    public int GetStarCount()
    {
        return Random.Range(starCountRange.x, starCountRange.y);
    }

    public float GetStarSize()
    {
        return Random.Range(starSizeRange.x, starSizeRange.y);
    }

    public int GetEnemyCount()
    {
        return Random.Range(enemyCountRange.x, enemyCountRange.y);
    }
}
